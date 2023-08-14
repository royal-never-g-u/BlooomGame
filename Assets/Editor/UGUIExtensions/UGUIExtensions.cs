/**
 * UGUI编辑拓展
 */

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using GameUnityFramework.Extensions;

namespace Editor.UGUIExtensions
{
    public class UGUIExtensions
    {
        [MenuItem("GameObject/UI/Scroll View Ex")]
        static void CreatScrollViewEx()
        {
            var parent = Selection.activeGameObject;
            EditorApplication.ExecuteMenuItem("GameObject/UI/Scroll View");
            var scrollRect = Selection.activeGameObject.GetComponent<ScrollRect>();
            if (parent != null) scrollRect.transform.SetParent(parent.transform);
            var conent = scrollRect.content;
            var viewport = scrollRect.viewport;
            var horizontalScrollbar = scrollRect.horizontalScrollbar;
            var horizontalScrollbarVisibility = scrollRect.horizontalScrollbarVisibility;
            var horizontalScrollbarSpacing = scrollRect.horizontalScrollbarSpacing;
            var verticalScrollbar = scrollRect.verticalScrollbar;
            var verticalScrollbarVisibility = scrollRect.verticalScrollbarVisibility;
            var verticalScrollbarSpacing = scrollRect.verticalScrollbarSpacing;
            GameObject.DestroyImmediate(scrollRect);
            var scrollRectEx = Selection.activeGameObject.AddComponent<ScrollRectEx>();
            scrollRectEx.content = conent;
            scrollRectEx.viewport = viewport;
            scrollRectEx.horizontalScrollbar = horizontalScrollbar;
            scrollRectEx.horizontalScrollbarVisibility = horizontalScrollbarVisibility;
            scrollRectEx.horizontalScrollbarSpacing = horizontalScrollbarSpacing;
            scrollRectEx.verticalScrollbar = verticalScrollbar;
            scrollRectEx.verticalScrollbarVisibility = verticalScrollbarVisibility;
            scrollRectEx.verticalScrollbarSpacing = verticalScrollbarSpacing;
        }
    }
}
