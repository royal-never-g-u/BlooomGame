/**
 * ScrollRect的扩展
 */

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameUnityFramework.Extensions
{
    public class ScrollRectEx : ScrollRect
    {
        public Action StartAction;
        public Action EndAction;
        /// <summary>
        /// 拖到底部（或最右边等）回调
        /// </summary>
        public Action OnDragBottomCallback { get; set; }
        /// <summary>
        /// 是否倒着操作
        /// </summary>
        public bool IsReverse { get; set; } = false;
        /// <summary>
        /// 是否拖到底部标记
        /// </summary>
        private bool _dragTag = false;

        /// <summary>
        /// 起始拖动 
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            _dragTag = false;
            StartAction?.Invoke();
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            EndAction?.Invoke();
        }
        public RectTransform GetChildByName(Transform myTransform, string str)
        {
            if (myTransform.childCount > 0)
            {
                foreach (Transform child in myTransform)
                {
                    if (child.name.Contains(str))
                    {
                        return child.GetComponent<RectTransform>();
                    }
                    else
                    {
                        return GetChildByName(child, str);
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 检测是否拖动底部
        /// </summary>
        protected virtual void Update()
        {
            if (content == null)
            {
                content = GetChildByName(transform, "Content");
            }
            if (viewport == null)
            {
                viewport = GetChildByName(transform, "Viewport");
            }
            if (!_dragTag)
            {
                if (horizontal)
                {
                    var scrollTransform = (RectTransform)transform;
                    if (content != null)
                    {
                        var contentWidth = content.rect.width;

                        if (IsReverse)
                        {
                            if (content.localPosition.x >= -contentWidth)
                            {
                                _dragTag = true;
                                OnDragBottomCallback?.Invoke();
                            }
                        }
                        else
                        {
                            if (content.localPosition.x <= scrollTransform.rect.width - contentWidth)
                            {
                                _dragTag = true;
                                OnDragBottomCallback?.Invoke();
                            }
                        }
                    }
                }
                if (vertical)
                {
                    var scrollTransform = (RectTransform)transform;
                    if (content != null)
                    {
                        var contentHeight = content.rect.height;
                        if (IsReverse)
                        {
                            if (content.localPosition.y <= -contentHeight)
                            {
                                _dragTag = true;
                                OnDragBottomCallback?.Invoke();
                            }
                        }
                        else
                        {
                            if (content.localPosition.y >= contentHeight - scrollTransform.rect.height)
                            {
                                _dragTag = true;
                                OnDragBottomCallback?.Invoke();
                            }
                        }
                    }
                }
            }
        }
    }
}