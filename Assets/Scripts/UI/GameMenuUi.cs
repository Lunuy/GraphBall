#nullable enable
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI
{
    // ReSharper disable once UnusedMember.Global
    public class GameMenuUi : MonoBehaviour
    {
        public RectTransform LevelSelectPanel = null!;

        private Vector2 _levelSelectPanelOpenPosition;
        private bool _selectLevelOpened;

        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            _levelSelectPanelOpenPosition = LevelSelectPanel.anchoredPosition;
        }

        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            if (Input.anyKeyDown)
            {
                if (!_selectLevelOpened)
                {
                    _selectLevelOpened = true;
                    StartCoroutine(AnimateMoveY(LevelSelectPanel, _levelSelectPanelOpenPosition));
                }
            }

            if (!_selectLevelOpened)
            {
                LevelSelectPanel.anchoredPosition = _levelSelectPanelOpenPosition + Vector2.down * LevelSelectPanel.rect.height;
            }
        }

        private static IEnumerator AnimateMoveY(RectTransform transform, Vector2 target)
        {
            const float smoothTime = 0.3f;
            var velocity = Vector2.zero;
            while (transform.anchoredPosition != target)
            {
                transform.anchoredPosition = Vector2.SmoothDamp(transform.anchoredPosition, target, ref velocity, smoothTime);
                yield return null;
            }
            transform.anchoredPosition = target;
        }
    }
}
