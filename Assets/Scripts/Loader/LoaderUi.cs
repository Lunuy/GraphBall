#nullable enable
using System.Collections;
using Assets.Scripts.Utility;
using TMPro;
using UnityEngine;
// ReSharper disable ForCanBeConvertedToForeach

namespace Assets.Scripts.Loader
{
    public class LoaderUi : MonoBehaviour
    {
        public GameObject UiRoot = null!;
        public TMP_Text LoadingTitleText = null!;
        public TMP_Text LoadingSubtitleText = null!;

        public bool Showing { get; private set; }

        public string Subtitle
        {
            get => LoadingSubtitleText.text;
            set => LoadingSubtitleText.text = value;
        }

        private static readonly string[] LoadingTexts = {"Loading", "Loading.", "Loading..", "Loading..." };
        private Coroutine? _currentAnimateCoroutine;

        // ReSharper disable once UnusedMember.Local
        private void Awake() => UiRoot.SetActive(Showing);

        public void ShowLoadingScreen()
        {
            if (Showing) return;
            Showing = true;

            UiRoot.SetActive(true);
            if (_currentAnimateCoroutine != null)
            {
                StopCoroutine(_currentAnimateCoroutine);
            }
            _currentAnimateCoroutine = StartCoroutine(AnimateLoadingText());
        }

        public void HideLoadingScreen()
        {
            if (!Showing) return;
            Showing = false;

            UiRoot.SetActive(false);
            if (_currentAnimateCoroutine == null) return;
            StopCoroutine(_currentAnimateCoroutine);
            _currentAnimateCoroutine = null;
        }

        private IEnumerator AnimateLoadingText()
        {
            for (;;)
            {
                for (var i = 0; i < LoadingTexts.Length; ++i)
                {
                    LoadingTitleText.text = LoadingTexts[i];
                    yield return YieldInstructionCache.WaitForSecondsRealtime(0.2f);
                }
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}
