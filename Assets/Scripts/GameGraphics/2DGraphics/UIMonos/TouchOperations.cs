/**
 * 触摸操作扩展
 * 主要是包含2点触摸事件、3D物件点击选中
 */

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameGraphics
{
    public class TouchOperations : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        /// <summary>
        /// 选中的模型
        /// </summary>
        private Transform _modelTransform;
        /// <summary>
        /// 射线选中的地面位置
        /// </summary>
        private Vector3 _hitFloorPoint;
        /// <summary>
        /// 选中回调
        /// </summary>
        public Action<Transform, Vector3> OnSelectAction;

        /// <summary>
        /// 检测2点触摸事件
        /// </summary>
        private void Update()
        {
            if (Input.touchCount == 2)
            {
                var touchZero = Input.GetTouch(0);
                var touchOne = Input.GetTouch(1);
                // 获取第一个触点的位置和上一帧的位置
                var touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                var touchZeroCurPos = touchZero.position;
                // 获取第二个触点的位置和上一帧的位置
                var touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
                var touchOneCurPos = touchOne.position;
                // 计算触点的移动距离
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZeroCurPos - touchOneCurPos).magnitude;
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
                // 控制相机远近
                Camera.main.transform.position -= Camera.main.transform.forward * deltaMagnitudeDiff / GlobalConfig.ScrollWheelSpeed;
            }
            else
            {
                var scrollDelta = Input.GetAxis("Mouse ScrollWheel");
                Camera.main.transform.position += Camera.main.transform.forward * scrollDelta * GlobalConfig.ScrollWheelSpeed;
            }
        }

        /// <summary>
        /// OnBeginDrag的时候检测是否有选择3D物体
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerDown(PointerEventData eventData)
        {
            if (Input.touchCount > 1)
            {
                return;
            }
            var modelRaycastHit = Raycast(eventData.position, "Building");
            if (modelRaycastHit != null)
            {
                _modelTransform = modelRaycastHit.Value.transform;
                var floorRaycastHit = Raycast(eventData.position, "Floor");
                if (floorRaycastHit != null)
                {
                    _hitFloorPoint = floorRaycastHit.Value.point;
                }
                OnSelectAction?.Invoke(_modelTransform, Vector3.zero);
            }
            else
            {
                OnSelectAction?.Invoke(null, Vector3.zero);
            }
        }

        /// <summary>
        /// OnDrag的时候检测是否有选择3D物体
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            if (Input.touchCount > 1)
            {
                return;
            }
            if (_modelTransform != null)
            {
                var floorRaycastHit = Raycast(eventData.position, "Floor");
                if (floorRaycastHit != null)
                {
                    var offset = floorRaycastHit.Value.point - _hitFloorPoint;
                    _hitFloorPoint = floorRaycastHit.Value.point;
                    OnSelectAction?.Invoke(_modelTransform, new Vector3(offset.x, 0, offset.z));
                }
            }
            else
            {
                Camera.main.transform.position -= Camera.main.transform.up * eventData.delta.y * 0.01f;
                Camera.main.transform.position -= Camera.main.transform.right * eventData.delta.x * 0.01f;
            }
        }

        /// <summary>
        /// OnEndDrag的时候检测是否有选择3D物体
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerUp(PointerEventData eventData)
        {
            //var modelRaycastHit = Raycast(eventData.position, "Building");
            //if (modelRaycastHit != null)
            //{
            //    OnSelectAction?.Invoke(modelRaycastHit.Value.transform, Vector3.zero);
            //}
            _modelTransform = null;
        }

        /// <summary>
        /// 射线检测
        /// </summary>
        /// <param name="position"></param>
        /// <param name="action"></param>
        private RaycastHit? Raycast(Vector2 position, string raycastLayer)
        {
            var ray = Camera.main.ScreenPointToRay(position);
            var layer = 1 << LayerMask.NameToLayer(raycastLayer);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layer))
            {
                //Debug.Log($"physics raycast => {hit.collider.name} {hit.point}");
                return hit;
            }
            return null;
        }
    }
}
