#nullable enable
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(GridLayoutGroup), typeof(RectTransform))]
    // ReSharper disable once UnusedMember.Global
    public class GridLayoutAutoScalar : MonoBehaviour
    {
        public bool MatchWithPreferredWidth;
        public bool MatchWithPreferredHeight;

        private GridLayoutGroup _gridLayoutGroup = null!;
        private RectTransform _rectTransform = null!;

        // ReSharper disable once UnusedMember.Local
        private void Awake()
        {
            _gridLayoutGroup = GetComponent<GridLayoutGroup>();
            _rectTransform = GetComponent<RectTransform>();
        }

        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            _rectTransform.sizeDelta = new Vector2(
                MatchWithPreferredWidth ? _gridLayoutGroup.preferredWidth : _rectTransform.sizeDelta.x,
                MatchWithPreferredHeight ? _gridLayoutGroup.preferredHeight : _rectTransform.sizeDelta.y);
        }
    }
}
