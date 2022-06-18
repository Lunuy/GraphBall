#nullable enable
using UnityEngine;

namespace Assets.Scripts.InGame
{
    [RequireComponent(typeof(Camera))]
    internal class CameraController : MonoBehaviour
    {
        public float CameraMaxPositionX = 10;
        public float CameraMaxPositionY = 10;
        public float CameraMinPositionX = -10;
        public float CameraMinPositionY = -10;

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
            if (Input.GetMouseButtonDown(0))
            {
                _lastDragPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
            }

            // ReSharper disable once InvertIf
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
