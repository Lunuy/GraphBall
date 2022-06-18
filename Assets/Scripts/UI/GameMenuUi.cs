#nullable enable
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class GameMenuUi : MonoBehaviour
    {
        public RectTransform LevelSelectPanel = null!;

        private Vector2 _levelSelectPanelOpenPosition;
        private bool _selectLevelOpened;

        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            _levelSelectPanelOpenPosition = LevelSelectPanel.anchoredPosition;
            LevelSelectPanel.anchoredPosition = _levelSelectPanelOpenPosition + Vector2.down * 1000;
        }

        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            if (!Input.anyKeyDown) return;
            if (_selectLevelOpened) return;
            _selectLevelOpened = true;
            StartCoroutine(AnimateMoveY(LevelSelectPanel, _levelSelectPanelOpenPosition.y));
        }

        private static IEnumerator AnimateMoveY(RectTransform transform, float target)
        {
            var targetVector = new Vector2(transform.anchoredPosition.x, target);

            const float smoothTime = 0.3f;
            var velocity = Vector2.zero;
            while (!Mathf.Approximately(transform.anchoredPosition.y, target))
            {
                transform.anchoredPosition = Vector2.SmoothDamp(transform.anchoredPosition, targetVector, ref velocity, smoothTime);
                yield return null;
            }
            transform.anchoredPosition = targetVector;
        }
    }
}
