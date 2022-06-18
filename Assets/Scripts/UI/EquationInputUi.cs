#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
// ReSharper disable ForCanBeConvertedToForeach

namespace Assets.Scripts.UI
{
    // ReSharper disable once UnusedMember.Global
    public class EquationInputUi : MonoBehaviour
    {
        public RectTransform InputPanel = null!;
        public TMP_InputField EquationInputField = null!;
        public TMP_Text LeftCharacterText = null!;
        public TMP_Text ParseErrorText = null!;

        private bool _showing;
        private const int HideHeight = 200;
        private Vector2 _panelShowPosition;

        private EquationInputContext? _equationInputContext;

        // ReSharper disable once UnusedMember.Local
        private void Awake()
        {
            EquationInputField.onValueChanged.AddListener(OnEquationInputChanged);
        }

        private void Start() 
        {
            _panelShowPosition = InputPanel.anchoredPosition;

            if (_equationInputContext != null) return;
            InputPanel.anchoredPosition = _panelShowPosition + Vector2.down * HideHeight;
            InputPanel.gameObject.SetActive(false);

            //ChangeContext(new EquationInputContext
            //{
            //    ErrorList = new List<string> {"Error: a", "Error: b"},
            //    InputEquation = "cos(x)",
            //    MaxCharacterCount = 10
            //});
        }

        // ReSharper disable once UnusedMember.Local
        private void OnDestroy()
        {
            EquationInputField.onValueChanged.RemoveListener(OnEquationInputChanged);

            InputPanel = null!;
            EquationInputField = null!;
            LeftCharacterText = null!;
            ParseErrorText = null!;

            _equationInputContext = null;
        }

        private bool _ignoreEvent;
        private void OnEquationInputChanged(string text)
        {
            if (_ignoreEvent) return;
            if (_equationInputContext == null) return;

            var inputText = EquationInputField.text;
            var textCount = CountTextExceptWhitespace(inputText);
            var maxCharCount = _equationInputContext.MaxCharacterCount;

            if (maxCharCount < textCount)
            {
                _ignoreEvent = true;
                var builder = new StringBuilder();
                var charCount = 0;
                for (var i = 0; i < inputText.Length; ++i)
                {
                    if (inputText[i] != ' ') charCount += 1;
                    if (maxCharCount < charCount) break;
                    builder.Append(inputText[i]);
                }
                LeftCharacterText.text =
                    maxCharCount + "/" + maxCharCount;
                EquationInputField.text = builder.ToString();
                _ignoreEvent = false;
            }
            else
            {
                LeftCharacterText.text =
                    textCount + "/" + maxCharCount;
            }

            _equationInputContext.InputEquation = EquationInputField.text;
        }

        public void ChangeContext(EquationInputContext? context)
        {
            if (_equationInputContext != null)
            {
                _equationInputContext.OnInputChanged = null;
                _equationInputContext.OnErrorListUpdate = null;
                _equationInputContext.OnMaxCharacterCountChanged = null;
            }

            _equationInputContext = context;

            if (_equationInputContext == null)
            {
                HideUi();
            }
            else
            {
                EquationInputField.text = _equationInputContext.InputEquation;
                SetErrorList(_equationInputContext.ErrorList);
                SetMaxCharacterCount(_equationInputContext.MaxCharacterCount);

                _equationInputContext.OnErrorListUpdate = SetErrorList;
                _equationInputContext.OnMaxCharacterCountChanged = SetMaxCharacterCount;

                ShowUi();
            }
        }

        private void ShowUi()
        {
            if (_showing) return;
            _showing = true;

            InputPanel.gameObject.SetActive(true);
            StartCoroutine(AnimateMoveY(InputPanel, _panelShowPosition.y));
        }

        private void HideUi()
        {
            if (!_showing) return;
            _showing = false;

            StartCoroutine(AnimateMoveY(InputPanel, _panelShowPosition.y + HideHeight, () =>
            {
                InputPanel.gameObject.SetActive(false);
            }));
        }

        private static IEnumerator AnimateMoveY(RectTransform transform, float target, Action? onComplete = null)
        {
            var targetVector = new Vector2(transform.anchoredPosition.x, target);
            while (!Mathf.Approximately(transform.anchoredPosition.y, target))
            {
                transform.anchoredPosition = Vector2.Lerp(transform.anchoredPosition, targetVector, Time.deltaTime * 10); 
                yield return null;
            }
            transform.anchoredPosition = targetVector;

            onComplete?.Invoke();
        }

        private void SetErrorList(IReadOnlyList<string>? errorList)
        {
            if (errorList == null)
            {
                ParseErrorText.text = string.Empty;
            }
            else
            {
                var builder = new StringBuilder();
                for (var i = 0; i < errorList.Count; ++i) builder.AppendLine(errorList[i]);
                ParseErrorText.text = builder.ToString();
            }
        }

        private static uint CountTextExceptWhitespace(string text)
        {
            var count = 0u;
            for (var i = 0; i < text.Length; ++i) if (text[i] != ' ') count++;
            return count;
        }

        private void SetMaxCharacterCount(uint value)
        {
            LeftCharacterText.text = CountTextExceptWhitespace(EquationInputField.text) + "/" + value;
        }
    }
}
