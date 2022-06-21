#nullable enable
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.InGame
{
    [RequireComponent(typeof(Camera))]
    // ReSharper disable once UnusedMember.Global
    internal class CameraController : MonoBehaviour
    {
        public float CameraMaxPositionX = 10;
        public float CameraMaxPositionY = 10;
        public float CameraMinPositionX = -10;
        public float CameraMinPositionY = -10;

        public float MaxOrthographicSize = 5;
        public float MinOrthographicSize = 3;

        private Camera _camera = null!;
        private Vector3 _lastDragPosition;

        //public float DragScalar = 18.5f;

        // ReSharper disable once UnusedMember.Local
        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            // Pinch to zoom
            if (Input.touchCount == 2)
            {

                // get current touch positions
                var tZero = Input.GetTouch(0);
                var tOne = Input.GetTouch(1);
                // get touch position from the previous frame
                var tZeroPrevious = tZero.position - tZero.deltaPosition;
                var tOnePrevious = tOne.position - tOne.deltaPosition;

                var oldTouchDistance = Vector2.Distance(tZeroPrevious, tOnePrevious);
                var currentTouchDistance = Vector2.Distance(tZero.position, tOne.position);

                // get offset value
                var deltaDistance = oldTouchDistance - currentTouchDistance;
                var scrollAcc = _camera.orthographicSize + deltaDistance * 0.01f;
                scrollAcc = Mathf.Clamp(scrollAcc, MinOrthographicSize, MaxOrthographicSize);

                _camera.orthographicSize = scrollAcc;
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _lastDragPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
                }

                if (Input.GetMouseButton(0))
                {
                    var newDragPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
                    var delta = _lastDragPosition - newDragPosition;
                    delta = new Vector3(delta.x * _camera.orthographicSize * _camera.aspect * 2, delta.y * _camera.orthographicSize * 2, delta.z);
                    _lastDragPosition = newDragPosition;
                    _camera.transform.position += delta;

                    var position = _camera.transform.position;
                    position.x = Mathf.Clamp(position.x, CameraMinPositionX, CameraMaxPositionX);
                    position.y = Mathf.Clamp(position.y, CameraMinPositionY, CameraMaxPositionY);
                    _camera.transform.position = position;
                }
            }

            {
                var scrollAcc = _camera.orthographicSize - Input.mouseScrollDelta.y;
                scrollAcc = Mathf.Clamp(scrollAcc, MinOrthographicSize, MaxOrthographicSize);

                _camera.orthographicSize = scrollAcc;
            }
        }

        // ReSharper disable once UnusedMember.Local
        private void OnDrawGizmos()
        {
            var point1 = new Vector2(CameraMinPositionX, CameraMinPositionY);
            var point2 = new Vector2(CameraMaxPositionX, CameraMaxPositionY);
            var point3 = new Vector2(CameraMaxPositionX, CameraMinPositionY);
            var point4 = new Vector2(CameraMinPositionX, CameraMaxPositionY);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(point1, point3);
            Gizmos.DrawLine(point1, point4);
            Gizmos.DrawLine(point2, point3);
            Gizmos.DrawLine(point2, point4);
        }
    }
}
