/**
 * 定时器
 */

using System;
using GameBaseFramework.Base;

namespace GameBaseFramework.Timer
{

    public interface ITimer { }

    public class Timer
    {
        /// <summary>
        /// 循环次数 -1表示无限循环
        /// </summary>
        private int _loop;
        /// <summary>
        /// 持续时间
        /// </summary>
        private float _duration;
        /// <summary>
        /// 剩余时间
        /// </summary>
        private float _leftTime;
        /// <summary>
        /// 控制运行
        /// </summary>
        private bool _isRunning;
        /// <summary>
        /// 是否结束
        /// </summary>
        private bool _isOver;
        /// <summary>
        /// 定时更新行为 返回剩余时间
        /// </summary>
        private Action _callback;
        /// <summary>
        /// 构造者
        /// </summary>
        private ITimer _owner;
        //========================================================
        public bool IsOver { get { return _isOver; } }
        public ITimer Owner { get { return _owner; } }

        /// <summary>
        /// 初始化
        /// </summary>
        public Timer(ITimer owner, float duration, Action callback, int loop)
        {
            _owner = owner;
            _duration = duration;
            _callback = callback;
            _loop = loop;
            _isRunning = true;
            _isOver = false;
            _leftTime = duration;
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
            _isOver = true;
        }

        /// <summary>
        /// 驱动
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            if (!_isRunning)
            {
                return;
            }
            _leftTime -= deltaTime;
            if (_leftTime <= 0)
            {
                try
                {
                    _callback?.Invoke();
                }
                catch (Exception e)
                {
                    Debuger.LogError(e.Message + e.StackTrace);
                }
                if (_loop > 0)
                {
                    _loop--;
                    _leftTime += _duration;
                }
                if (_loop == 0)
                {
                    Stop();
                    return;
                }
                if (_loop < 0)
                {
                    _leftTime += _duration;
                }
            }
        }
    }
}

