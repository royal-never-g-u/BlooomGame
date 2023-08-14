/**
 * UI视图基类 
 */

using System;
using GameLogics;
using UnityEngine;
using UnityEngine.UI;
using GameBaseFramework.Patterns;
using System.Collections.Generic;
using GameBaseFramework.Timer;
using GameUnityFramework.Resource;
using System.Collections;
using UnityEngine.AddressableAssets;
using GameBaseFramework.Base;
using System.Linq;

namespace GameGraphics
{
    public enum EUIViewState
    {
        Open,
        Close
    }

    /// <summary>
    /// UI界面协程
    /// </summary>
    public class ViewCoroutine : MonoBehaviour { }

    public class UIViewBase : CustomYieldInstruction, ICommand, ITimer
    {
        /// <summary>
        /// UIView的状态
        /// </summary>
        public EUIViewState State { get; private set; }
        public override bool keepWaiting
        {
            get
            {
                if (State == EUIViewState.Open)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        /// <summary>
        /// 页面存在的类型
        /// </summary>
        public GameLogics.EUIViewOpenType OpenType { get; set; }
        /// <summary>
        /// 页面名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 预制体实例引用
        /// </summary>
        protected GameObject _root;
        /// <summary>
        /// 父节点
        /// </summary>
        protected Transform _parent;
        /// <summary>
        /// 需要管理的子界面
        /// </summary>
        protected List<UIViewBase> _childs = new List<UIViewBase>();
        /// <summary>
        /// 要用到的协程
        /// </summary>
        protected ViewCoroutine _coroutine;
        /// <summary>
        /// 指令回调
        /// </summary>
        protected Dictionary<int, Delegate> _commandHandlers;
        /// <summary>
        /// 唯一Id
        /// </summary>
        private static int _uniqueId = 0;
        /// <summary>
        /// UIView关闭时的事件回调，一般用于刷新前一个页面
        /// </summary>
        public Action _EndEvent = null;

        public int Id { get; private set; }
        public int ViewPriority;
        private bool isActive=false;

        /// <summary>
        /// 构造
        /// </summary>
        public UIViewBase()
        {
            Id = _uniqueId++;
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        public void Open(Transform parent, string name, int priority, object data = null)
        {
            _parent = parent;
            Name = name;
            UnityObjectManager.Instance.SyncOrAsyncLoad(GetPrefabPath(), _parent, (go) =>
            {
                _root = go;
                _root.name = name;
                State = EUIViewState.Open;
                BindWidgets();
                OnEnter(data);
                ViewPriority = priority * 10000 + _root.transform.GetSiblingIndex();
            });
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="name"></param>
        /// <param name="openType"></param>
        public void Open(string name, EUIViewOpenType openType)
        {
            Open(name, null, openType);
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="name"></param>
        /// <param name="priority"></param>
        public void Open(string name, EUIViewPriority priority)
        {
            Open(name, null, EUIViewOpenType.Overlying, priority);
        }

        /// <summary>
        /// UI直接相互直接打开接口
        /// </summary>
        public void Open(string name, object data = null, EUIViewOpenType openType = EUIViewOpenType.Replace, EUIViewPriority priority = EUIViewPriority.Level5)
        {
            var command = new GameLogics.UIViewOpenCommand();
            command.Name = name;
            command.Data = data;
            command.ViewOpenType = openType;
            command.ViewPriority = priority;
            this.SendCommand(command);
        }

        /// <summary>
        /// 根据泛型打开界面
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="openType"></param>
        public void Open<T>(object data = null, EUIViewOpenType openType = EUIViewOpenType.Replace, EUIViewPriority priority = EUIViewPriority.Level5, Action CloseCallback = null) where T : UIViewBase
        {
            var command = new UIViewOpenCommand();
            command.ViewType = typeof(T);
            var typeNames = command.ViewType.ToString().Split(".");
            command.Name = typeNames[typeNames.Length - 1];
            command.Data = data;
            command.ViewOpenType = openType;
            command.ViewPriority = priority;
            command._EndEvent = CloseCallback;
            this.SendCommand(command);
        }

        /// <summary>
        /// 关闭指定泛型的界面
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Close<T>() where T : UIViewBase
        {
            var command = new UIViewCloseCommand();
            var typeNames = typeof(T).ToString().Split(".");
            command.Name = typeNames[typeNames.Length - 1];
            this.SendCommand(command);
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        public void Close()
        {
            State = EUIViewState.Close;
            if (_EndEvent != null) { Debug.Log(_EndEvent.ToJson()); _EndEvent.Invoke(); } //调用EndEvent
            this.UnbindSelfAllCommands();
            this.ClearSelfTimers();
            foreach (var child in _childs)
            {
                child.Close();
            }
            _childs.Clear();
            OnExit();
            if (_root != null)
            {
                GameObject.Destroy(_root);
                Addressables.ReleaseInstance(_root);
            }
        }

        public void ShowOrHide(bool isShow)
        {
            _root.SetActive(isShow);
        }

        public void Close(string name)
        {
            var command = new GameLogics.UIViewCloseCommand();
            command.Name = name;
            this.SendCommand(command);
        }

        /// <summary>
        /// 协程
        /// </summary>
        /// <param name="routine"></param>
        public void StartCoroutine(IEnumerator routine)
        {
            if (_coroutine == null)
            {
                _coroutine = _root.AddComponent<ViewCoroutine>();
            }
            _coroutine.StartCoroutine(routine);
        }

        /// <summary>
        /// 添加子界面
        /// </summary>
        public UIViewBase AddView(string name, Transform parent, object data = null)
        {
            if (UIViewConfig.NameToTypeDict.TryGetValue(name, out var type))
            {
                var view = Activator.CreateInstance(type) as UIViewBase;
                view.Open(parent, name, 0, data);
                _childs.Add(view);
                return view;
            }
            return null;
        }
        public T AddView<T>(Transform parent,object data = null) where T : UIViewBase,new()
        {
            var t=new T();
            t.Open(parent, t.Name, 0, data);
            _childs.Add(t);
            return t;
        }

        /// <summary>
        /// 绑定所有控件
        /// </summary>
        protected virtual void BindWidgets() { }

        /// <summary>
        /// 预制体路径
        /// </summary>
        /// <returns></returns>
        protected virtual string GetPrefabPath() { return ""; }

        /// <summary>
        /// Update
        /// </summary>
        public virtual void Update() {
            try
            {
                if (MainGraphic.UIViewManager == null) return;

                if (MainGraphic.UIViewManager.upView != _root.name)
                {
                    isActive = false;
                }
                else
                {
                    if (isActive == false)
                    {
                        isActive = true;
                        OnActive();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.StackTrace + "\n" + e.Message);
            }
            
        }
        public virtual void OnActive()
        {

        }
        /// <summary>
        /// 进入界面
        /// </summary>
        protected virtual void OnEnter(object data = null) { }
        
        /// <summary>
        /// 激活状态
        /// </summary>
        public virtual void Enable(bool active)
        {
            if (active)
            {
                if (_commandHandlers != null)
                {
                    this.BindCommandHanders(_commandHandlers);
                }
                foreach (var child in _childs)
                {
                    child.Enable(true);
                }
            }
            else
            {
                _commandHandlers = this.UnbindSelfAllCommands(false);
                foreach (var child in _childs)
                {
                    child.Enable(false);
                }
            }
        }

        /// <summary>
        /// 退出界面
        /// </summary>
        protected virtual void OnExit() { }

        #region 通用的UI辅助方法
        /// <summary>
        /// 通用的改变互斥按钮状态方法
        /// </summary>
        /// <param name=""></param>
        protected virtual void ChangeMutexBtnState(Button button)
        {
            if (button.transform.parent != null)
            {
                var buttons = button.transform.parent.GetComponentsInChildren<Button>();
                for (var i = 0; i < buttons.Length; i++)
                {
                    buttons[i].interactable = true;
                }
            }
            button.interactable = false;
        }
        /// <summary>
        /// 通用的使用道具方法
        /// </summary>
        /// <param name="id"></param>
        /// <param name="action"></param>
        protected void CloseAllUp(string myName)
        {
            List<GameObject> a=new List<GameObject>();
            bool isBreak = false;
            foreach(Transform trans in _root.transform.parent.parent)
            {
                foreach(Transform trans1 in trans)
                {
                    if (trans1.name != myName)
                    {
                        trans1.gameObject.SetActive(false);
                    }
                    else
                    {
                        isBreak = true;
                        break;
                    }
                }
                if (isBreak)
                {
                    break;
                }
            }
        }
        protected void OpenAllUp(string myName)
        {
            List<GameObject> a = new List<GameObject>();
            bool isBreak = false;
            foreach (Transform trans in _root.transform.parent.parent)
            {
                foreach (Transform trans1 in trans)
                {
                    if (trans1.name != myName)
                    {
                        trans1.gameObject.SetActive(true);
                    }
                    else
                    {
                        isBreak = true;
                        break;
                    }
                }
                if (isBreak)
                {
                    break;
                }
            }
        }
        protected void DestroyAllUp(string myName)
        {
            this.SendCommand(new CloseAllViewCommond()
            {
                except = "UIViewMain",
            });
        }
        #endregion
    }
}