/**
 * 定时器管理
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace GameBaseFramework.Timer
{
    public static class TimerManager
    {
        /// <summary>
        /// 定时器链表
        /// </summary>
        private static HashSet<Timer> _timerList = new HashSet<Timer>();
        /// <summary>
        /// 要删除的定时器列表
        /// </summary>
        private static List<Timer> _removeTimerList = new List<Timer>();

        /// <summary>
        /// 创建定时器
        /// </summary>
        /// <param name="self"></param>
        /// <param name="duration"></param>
        /// <param name="finishAction"></param>
        /// <param name="loop"></param>
        public static void CreateTimer(this ITimer self, float duration, Action finishAction, int loop = 1)
        {
            var timer = new Timer(self, duration, finishAction, loop);
            _timerList.Add(timer);
        }

        /// <summary>
        /// 创建定时器
        /// </summary>
        /// <param name="self"></param>
        /// <param name="interval"></param>
        /// <param name="duration"></param>
        /// <param name="updateAction"></param>
        /// <param name="finishAction"></param>
        public static void CreateTimer(this ITimer self, float interval, float duration, Action<float> updateAction, Action finishAction)
        {
            var loop = (int)(duration / interval);
            self.CreateTimer(interval, () =>
            {
                duration -= interval;
                updateAction?.Invoke(duration);
                loop--;
                if (loop == 0)
                {
                    finishAction?.Invoke();
                }
            }, loop);
        }

        /// <summary>
        /// 延迟
        /// </summary>
        /// <param name="self"></param>
        /// <param name="handler"></param>
        public static void Delay(this ITimer self, float duration, Action finishAction)
        {
            var timer = new Timer(self, duration, finishAction, 1);
            _timerList.Add(timer);
        }

        /// <summary>
        /// 清除所有绑定在self上的定时器
        /// </summary>
        /// <param name="self"></param>
        public static void ClearSelfTimers(this ITimer self)
        {
            foreach (var timer in _timerList)
            {
                if (timer.Owner == self)
                {
                    timer.Stop();
                }
            }
        }

        /// <summary>
        /// 时间驱动
        /// </summary>
        /// <param name="deltaTime"></param>
        public static void Update(float deltaTime)
        {
            var tempI = _timerList.Count;
            for(int i=0;i<tempI;i++)
            //foreach (var item in _timerList)
            {
                var item = _timerList.ToList()[i];
                if (!item.IsOver)
                {
                    item.Update(deltaTime);
                }
                else
                {
                    _removeTimerList.Add(item);
                }
            }
            if (_removeTimerList.Count > 0)
            {
                for (var i = 0; i < _removeTimerList.Count; i++)
                {
                    _timerList.Remove(_removeTimerList[i]);
                }
                _removeTimerList.Clear();
            }
        }
    }
}