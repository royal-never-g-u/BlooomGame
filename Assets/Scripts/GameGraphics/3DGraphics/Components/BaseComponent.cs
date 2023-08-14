/**
 * 组件基类 
 */

using System;
using GameBaseFramework.Base;
using System.Collections.Generic;
using GameBaseFramework.Patterns;

namespace GameGraphics.HomeWorld
{
    public class BaseComponent : ITypeId, ICommand
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
        /// 绑定的实体
        /// </summary>
        public BaseEntity Entity { get; private set; }
        /// <summary>
        /// 组件状态
        /// </summary>
        public ECommonStatus Status { get; private set; }

        /// <summary>
        /// 构造
        /// </summary>
        public BaseComponent()
        {
            Id = _uniqueId++;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public void Awake(BaseEntity entity)
        {
            Entity = entity;
            OnAwake();
        }

        /// <summary>
        /// 初始化完成
        /// </summary>
        protected virtual void OnAwake() { }

        /// <summary>
        /// 驱动更新
        /// </summary>
        public virtual void Update(float deltaTime) { }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Destroy()
        {
            Status = ECommonStatus.Destroy;
            OnDestroy();
        }

        /// <summary>
        /// 完成销毁
        /// </summary>
        protected virtual void OnDestroy() { }
    }
}
