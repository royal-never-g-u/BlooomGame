/**
 * 指令管理
 */

using System;
using System.Collections.Generic;
using GameBaseFramework.Base;

namespace GameBaseFramework.Patterns
{
    /// <summary>
    /// 指令接口 
    /// </summary>
    public interface ICommand { }


    public static class CommandManager
    {
        /// <summary>
        /// 指令唯一索引
        /// </summary>
        private static int _uniqueIndex = 0;
        /// <summary>
        /// 指令链表
        /// </summary>
        private static LinkedList<Command> _commandLinkedList = new LinkedList<Command>();
        /// <summary>
        /// 指令接受器集合
        /// commandTypeId -> List<Handler>
        /// </summary>
        private static Dictionary<int, LinkedList<Delegate>> _commandHandlerListDict = new Dictionary<int, LinkedList<Delegate>>();
        /// <summary>
        /// 持有者的回调列表
        /// </summary>
        private static Dictionary<ICommand, LinkedList<Delegate>> _ownerHandersDict = new Dictionary<ICommand, LinkedList<Delegate>>();

        /// <summary>
        /// 绑定指令接收回调
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="command"></param>
        /// <param name="handler"></param>
        public static void BindCommand<T>(this ICommand self, Action<T> handler) where T : Command
        {
            if (handler == null) return;
            if (!_ownerHandersDict.TryGetValue(self, out var handlers))
            {
                handlers = new LinkedList<Delegate>();
                _ownerHandersDict.Add(self, handlers);
            }
            if (handlers.Contains(handler))
            {
                return;
            }
            handlers.AddLast(handler);

            var commandTypeId = TypeId.GetId<T>();
            if (!_commandHandlerListDict.TryGetValue(commandTypeId, out var list))
            {
                list = new LinkedList<Delegate>();
                _commandHandlerListDict.Add(commandTypeId, list);
            }
            list.AddLast(handler);
        }

        /// <summary>
        /// 接触指令回调绑定
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="command"></param>
        /// <param name="handler"></param>
        private static void UnbindCommand<T>(this ICommand self, T command, Action<T> handler) where T : Command
        {
            var commandTypeId = TypeId.GetId<T>();
            if (_commandHandlerListDict.TryGetValue(commandTypeId, out var list))
            {
                list.Remove(handler);
            }
        }

        /// <summary>
        /// 绑定指令集
        /// </summary>
        /// <param name=""></param>
        public static void BindCommandHanders(this ICommand self, Dictionary<int, Delegate> handers)
        {
            foreach(var item in handers)
            {
                if (!_ownerHandersDict.TryGetValue(self, out var selfHandlers))
                {
                    selfHandlers = new LinkedList<Delegate>();
                    _ownerHandersDict.Add(self, selfHandlers);
                }
                selfHandlers.AddLast(item.Value);

                var commandTypeId = item.Key;
                if (!_commandHandlerListDict.TryGetValue(commandTypeId, out var list))
                {
                    list = new LinkedList<Delegate>();
                    _commandHandlerListDict.Add(commandTypeId, list);
                }
                list.AddLast(item.Value);
            }
        }

        /// <summary>
        /// 解绑自身上的所有指令
        /// </summary>
        public static Dictionary<int, Delegate> UnbindSelfAllCommands(this ICommand self, bool clear = true)
        {
            Dictionary<int, Delegate> typeIdToHandlers = null;
            if (_ownerHandersDict.TryGetValue(self, out var handlers))
            {
                foreach (var hander in handlers)
                {
                    foreach (var item in _commandHandlerListDict)
                    {
                        if (item.Value.Remove(hander))
                        {
                            if (!clear)
                            {
                                if (typeIdToHandlers == null) typeIdToHandlers = new Dictionary<int, Delegate>();
                                typeIdToHandlers.Add(item.Key, hander);
                            }
                        }
                    }
                }
                _ownerHandersDict.Remove(self);
            }
            return typeIdToHandlers;
        }

        /// <summary>
        /// 添加指令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        public static void SendCommand<T>(this ICommand self, T command) where T : Command
        {
            command.TypeId = TypeId.GetId<T>();
            if (command.SyncExecute)
            {
                HandleCommand(command);
            }
            else
            {
                command.Index = ++_uniqueIndex;
                var current = _commandLinkedList.First;
                while (current != null)
                {
                    if ((int)current.Value.Priority < (int)command.Priority)
                    {
                        _commandLinkedList.AddBefore(current, command);
                        return;
                    }
                    current = current.Next;
                }
                _commandLinkedList.AddLast(command);
            }
        }

        /// <summary>
        /// 处理指令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        public static void HandleCommand<T>(T command) where T : Command
        {
            var commandTypeId = command.TypeId;
            if (_commandHandlerListDict.TryGetValue(commandTypeId, out var list))
            {
                var current = list.First;
                while (current != null)
                {
                    current.Value.DynamicInvoke(command);
                    current = current.Next;
                }
            }
        }

        /// <summary>
        /// 更新指令
        /// </summary>
        public static void Update()
        {
            if (_commandLinkedList.Count > 0)
            {
                var current = _commandLinkedList.First;
                while (current != null)
                {
                    try
                    {
                        HandleCommand(current.Value);
                    }
                    catch (Exception e)
                    {
                        Debuger.LogError(e.InnerException.StackTrace);
                    }
                    current = current.Next;
                }
                _commandLinkedList.Clear();
            }
        }
    }
}
