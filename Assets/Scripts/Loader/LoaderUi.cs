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
        public CanvasGroup UiRoot = null!;
        public TMP_Text LoadingTitleText = null!;
        public TMP_Text LoadingSubtitleText = null!;

        public bool Showing { get; private set; }

        public string Subtitle
        {
            get => LoadingSubtitleText.text;
            set => LoadingSubtitleText.text = value;
        }

        private static readonly string[] LoadingTexts = {"Loading", "Loading.", "Loading..", "Loading..." };
        private Coroutine? _currentTextAnimateCoroutine;
        private Coroutine? _currentFadeAnimateCoroutine;

        // ReSharper disable once UnusedMember.Local
        private void Awake() => UiRoot.gameObject.SetActive(Showing);

        public void ShowLoadingScreen()
        {
            if (Showing) return;
            Showing = true;

            if (_currentFadeAnimateCoroutine != null) StopCoroutine(_currentFadeAnimateCoroutine);
            _currentFadeAnimateCoroutine = StartCoroutine(ShowCanvas());

            if (_currentTextAnimateCoroutine != null) StopCoroutine(_currentTextAnimateCoroutine);
            _currentTextAnimateCoroutine = StartCoroutine(AnimateLoadingText());
        }

        public void HideLoadingScreen()
        {
            if (!Showing) return;
            Showing = false;

            if (_currentFadeAnimateCoroutine != null) StopCoroutine(_currentFadeAnimateCoroutine);
            _currentFadeAnimateCoroutine = StartCoroutine(HideCanvas());

            if (_currentTextAnimateCoroutine == null) return;
            StopCoroutine(_currentTextAnimateCoroutine);
            _currentTextAnimateCoroutine = null;
        }

        private IEnumerator ShowCanvas()
        {
            UiRoot.gameObject.SetActive(true);
            for (var alpha = UiRoot.alpha; alpha < 1f; alpha += Time.deltaTime / 1f)
            {
                UiRoot.alpha = alpha;
                yield return null;
            }
            
            UiRoot.alpha = 1f;
        }

        private IEnumerator HideCanvas()
        {
            for (var alpha = UiRoot.alpha; alpha > 0f; alpha -= Time.deltaTime / 1f)
            {
                UiRoot.alpha = alpha;
                yield return null;
            }

            UiRoot.alpha = 0f;
            UiRoot.gameObject.SetActive(false);
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
