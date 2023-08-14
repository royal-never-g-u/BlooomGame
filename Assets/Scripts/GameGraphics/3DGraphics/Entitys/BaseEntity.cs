/**
 * 实体基类
 * 简单处理：每个实体只能有一种类型的组件
 */

using System;
using GameBaseFramework.Base;
using System.Collections.Generic;
using GameBaseFramework.Patterns;

namespace GameGraphics.HomeWorld
{
    public class BaseEntity : ICommand
    {
        /// <summary>
        /// 全局唯一Id
        /// </summary>
        private static long _uniqueId = 0;
        /// <summary>
        /// 唯一Id
        /// </summary>
        public long Id { get; private set; }
        /// <summary>
        /// 配置表Id
        /// 如果为0表示为自定义实体
        /// </summary>
        public int TableId { get; private set; }
        /// <summary>
        /// 当前状态
        /// </summary>
        public ECommonStatus Status { get; private set; }
        /// <summary>
        /// 当前组件列表
        /// </summary>
        protected List<BaseComponent> _componentList = new List<BaseComponent>();
        /// <summary>
        /// 组件集合
        /// </summary>
        protected Dictionary<long, BaseComponent> _componentDict = new Dictionary<long, BaseComponent>();
        /// <summary>
        /// 待删除组件列表
        /// </summary>
        protected List<int> _removeComponentList = new List<int>();

        /// <summary>
        /// 自增唯一Id
        /// </summary>
        public BaseEntity(int tableId = 0)
        {
            Id = _uniqueId++;
            TableId = tableId;
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T AddComponent<T>() where T : BaseComponent, new()
        {
            var component = new T();
            AddComponent(component);
            return component;
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        private void AddComponent<T>(T component) where T : BaseComponent
        {
            var typeId = TypeId.GetId<T>();
            _componentList.Add(component);
            _componentDict.Add(typeId, component);
            component.Awake(this);
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public BaseComponent GetComponent<T>() where T : BaseComponent
        {
            var typeId = TypeId.GetId<T>();
            if (_componentDict.TryGetValue(typeId, out var component))
            {
                return component;
            }
            return null;
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Destroy()
        {
            Status = ECommonStatus.Destroy;
            for (var i = 0; i < _componentList.Count; i++)
            {
                var component = _componentList[i];
                component.Destroy();
            }
        }

        /// <summary>
        /// 驱动更新
        /// </summary>
        public void Update(float deltaTime)
        {
            var count = _componentList.Count;
            for (var i = 0; i < count; i++)
            {
                var component = _componentList[i];
                if (component.Status == ECommonStatus.Destroy)
                {
                    _removeComponentList.Add(i);
                }
                else
                {
                    component.Update(deltaTime);
                }
            }
            if (_removeComponentList.Count > 0)
            {
                for (var i = _removeComponentList.Count - 1; i >= 0; i--)
                {
                    var removeIndex = _removeComponentList[i];
                    _componentList.RemoveAt(removeIndex);
                }
                _removeComponentList.Clear();
            }
        }
    }
}

