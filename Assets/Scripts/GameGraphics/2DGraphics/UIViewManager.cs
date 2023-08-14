/**
 * UI表现管理器
 */

using System;
using UnityEngine;
using GameBaseFramework.Base;
using GameBaseFramework.Patterns;
using System.Collections.Generic;
using GameUnityFramework.Resource;
using GameLogics;
using System.Collections;

namespace GameGraphics
{
    public class CloseAllViewCommond : Command
    {
        public string name;
        public string except;
    }

    public class ShowOrHideAllViewCommond : Command
    {
        public string Name;
        public bool IsShow;
    }

    public class UIViewManager : ICommand
    {
        /// <summary>
        /// 2DUI根节点
        /// </summary>
        private Transform _2DUIGraphicRoot;
        /// <summary>
        /// UI优先级根节点
        /// </summary>
        private List<Transform> _2DUIPriorityNodeList;
        /// <summary>
        /// 需要删除的UIView列表
        /// </summary>
        private List<int> _removeUIViews = new List<int>();
        /// <summary>
        /// 待添加的UIView列表
        /// </summary>
        private List<UIViewBase> _addUIViews = new List<UIViewBase>();
        /// <summary>
        /// 当前已经存在的所有UIView
        /// </summary>
        private Dictionary<int, UIViewBase> __curUIViews = new Dictionary<int, UIViewBase>();
        private Dictionary<int, UIViewBase> _curUIViews
        {
            get
            {
                return __curUIViews;
            }
            set
            {
                __curUIViews=value;
                RefreshUp();
            }
        }
        public string upView = "";
        
        public void RefreshUp()
        {
            foreach(Transform trans in _2DUIGraphicRoot)
            {
                if (trans.childCount > 0)
                {
                    foreach (Transform trans1 in trans)
                    {
                        if (trans1.gameObject.activeSelf)
                        {
                            upView = trans1.name;
                        }
                    }
                }  
            }
        }
        /// <summary>
        /// 构造绑定指令回调
        /// </summary>
        public UIViewManager()
        {
            this.BindCommand<UIViewOpenCommand>(OpenView);
            this.BindCommand<UIViewCloseCommand>(CloseView);
            this.BindCommand<CloseAllViewCommond>(CloseAllView);
            this.BindCommand<ShowOrHideAllViewCommond>(ShowOrHideAllView);
            _2DUIGraphicRoot = GameObject.Find("GameEntrance/2DUIGraphicRoot").transform;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            foreach (var item in _curUIViews)
            {
                if (item.Value.State == EUIViewState.Open)
                {
                    item.Value.Update();
                }
                else
                {
                    _removeUIViews.Add(item.Key);
                    var view = GetMaxPriorityView(item.Value.Name, item.Value);
                    if (view != null) view.Enable(true);
                }
            }
            if (_removeUIViews.Count > 0)
            {
                for (var i = 0; i < _removeUIViews.Count; i++)
                {
                    _curUIViews.Remove(_removeUIViews[i]);
                }
                _removeUIViews.Clear();
            }
            if (_addUIViews.Count > 0)
            {
                for (var i = 0; i < _addUIViews.Count; i++)
                {
                    var view = _addUIViews[i];
                    if (_curUIViews.ContainsKey(view.Id))
                    {
                        _curUIViews[view.Id] = view;
                    }
                    else
                    {
                        _curUIViews.Add(view.Id, view);
                    }
                }
                _addUIViews.Clear();
            }
            RefreshUp();
        }
        /// <summary>
        /// 根据界面名，查找对应界面指定的优先级节点
        /// </summary>
        /// <param name="uiViewName"></param>
        /// <returns></returns>
        private Transform Get2DUIPriorityNode(int priority)
        {
            if (_2DUIPriorityNodeList == null)
            {
                _2DUIPriorityNodeList = new List<Transform>();
                for (var i = 1; i <= (int)EUIViewPriority.Max; i++)
                {
                    _2DUIPriorityNodeList.Add(_2DUIGraphicRoot.Find("PriorityNode" + i));
                }
            }
            return _2DUIPriorityNodeList[priority - 1];
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="command"></param>
        private void CloseView(UIViewCloseCommand command)
        {
            foreach (var item in _curUIViews)
            {
                if (item.Value.Name == command.Name)
                {
                    item.Value.Close();
                    return;
                }
            }
        }

        public void CloseAllView(CloseAllViewCommond command)
        {
            if (string.IsNullOrEmpty(command.name))
            {
                foreach (var item in _curUIViews)
                {
                    if (item.Value.Name != command.except && item.Value.OpenType != EUIViewOpenType.Forever)
                    {
                        item.Value.Close();
                    }
                }
            }
            else
            {
                foreach (var item in _curUIViews)
                {
                    if (item.Value.Name == command.name)
                    {
                        item.Value.Close();
                    }
                }
            }
        }

        public void ShowOrHideAllView(ShowOrHideAllViewCommond command)
        {
            foreach (var item in _curUIViews)
            {
                if (item.Value.Name == command.Name)
                {
                    item.Value.ShowOrHide(command.IsShow);
                }
            }
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="eUIViewOpenType"></param>
        /// <param name="eViewPriority"></param>
        public void OpenView(Type type, string name, object data, EUIViewOpenType eUIViewOpenType, EUIViewPriority eViewPriority, Action _EndEvent = null)
        {
            switch (eUIViewOpenType)
            {
                case EUIViewOpenType.Replace:
                    //关闭除永久存在之外的所有UIView，再打开当前界面
                    foreach (var item in _curUIViews)
                    {
                        if (item.Value.Name == name)
                        {
                            return;
                        }
                        if (item.Value.OpenType != EUIViewOpenType.Forever)
                        {
                            item.Value.Close();
                        }
                    }
                    break;
                case EUIViewOpenType.Overlying:
                    //当前同名的最高层级界面
                    var curMaxPriorityView = GetMaxPriorityView(name);
                    if (curMaxPriorityView != null)
                    {
                        curMaxPriorityView.Enable(false);
                    }
                    break;
                case EUIViewOpenType.Forever:
                    //if (_curUIViews.ContainsKey(name))
                    //{
                    //    return;
                    //}
                    break;
            }
            var parent = Get2DUIPriorityNode((int)eViewPriority);
            var view = Activator.CreateInstance(type) as UIViewBase;
            view.OpenType = eUIViewOpenType;
            view._EndEvent = _EndEvent;
            _addUIViews.Add(view);
            view.Open(parent, name, (int)eViewPriority, data);
        }

        private UIViewBase GetMaxPriorityView(string name, UIViewBase view = null)
        {
            UIViewBase curMaxPriorityView = null;
            foreach (var item in _curUIViews)
            {
                if (item.Value.Name == name)
                {
                    if (view == null || view != item.Value)
                    {
                        if (curMaxPriorityView == null)
                        {
                            curMaxPriorityView = item.Value;
                        }
                        else
                        {
                            var maxPriority = curMaxPriorityView.ViewPriority;
                            var curPriority = item.Value.ViewPriority;
                            if (curPriority > maxPriority)
                            {
                                curMaxPriorityView = item.Value;
                            }
                        }
                    }
                }
            }
            return curMaxPriorityView;
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="command"></param>
        private void OpenView(UIViewOpenCommand command)
        {
            if (command.ViewType != null)
            {
                OpenView(command.ViewType, command.Name, command.Data, command.ViewOpenType, command.ViewPriority, command._EndEvent);
                return;
            }

            if (UIViewConfig.NameToTypeDict.TryGetValue(command.Name, out var type))
            {
                OpenView(type, command.Name, command.Data, command.ViewOpenType, command.ViewPriority);
            }
            else
            {
                Debuger.LogError($"open view ==> not find {command.Name}");
            }
        }
    }
}
