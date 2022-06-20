#nullable enable
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class SimulationControlUi : MonoBehaviour
    {
        public event Action OnPlayButtonClicked = delegate { };
        public event Action OnStopButtonClicked = delegate { };
        public event Action OnPauseButtonClicked = delegate { };
        public event Action OnResumeButtonClicked = delegate { };

        public Button PlayButton = null!;
        public Button StopButton = null!;
        public Button PauseButton = null!;
        public Button ResumeButton = null!;

        private enum ControlState
        {
            Stopped,
            Playing,
            Paused
        }

        public bool CanPlay
        {
            get => _canPlay;
            set
            {
                _canPlay = value;
                PlayButton.interactable = value;
            }
        }

        private ControlState _state = ControlState.Stopped;
        private bool _canPlay = true;

        // ReSharper disable once UnusedMember.Local
        private void Awake()
        {
            PlayButton.onClick.AddListener(OnPlayButtonClick);
            StopButton.onClick.AddListener(OnStopButtonClick);
            PauseButton.onClick.AddListener(OnPauseButtonClick);
            ResumeButton.onClick.AddListener(OnResumeButtonClick);
        }

        // ReSharper disable once UnusedMember.Local
        private void OnDestroy()
        {
            if (PlayButton) PlayButton.onClick.RemoveListener(OnPlayButtonClick);
            if (StopButton) StopButton.onClick.RemoveListener(OnStopButtonClick);
            if (PauseButton) PauseButton.onClick.RemoveListener(OnPauseButtonClick);
            if (ResumeButton) ResumeButton.onClick.RemoveListener(OnResumeButtonClick);
        }

        private void OnPlayButtonClick()
        {
            if (_state is not ControlState.Stopped) return;
            _state = ControlState.Playing;

            PlayButton.gameObject.SetActive(false);
            StopButton.gameObject.SetActive(true);
            PauseButton.gameObject.SetActive(true);
            ResumeButton.gameObject.SetActive(false);
            OnPlayButtonClicked.Invoke();
        }

        private void OnStopButtonClick()
        {
            if (_state is not (ControlState.Playing or ControlState.Paused)) return;
            _state = ControlState.Stopped;

            PlayButton.gameObject.SetActive(true);
            StopButton.gameObject.SetActive(false);
            PauseButton.gameObject.SetActive(false);
            ResumeButton.gameObject.SetActive(false);
            OnStopButtonClicked.Invoke();
        }

        private void OnPauseButtonClick()
        {
            if (_state is not ControlState.Playing) return;
            _state = ControlState.Paused;

            PlayButton.gameObject.SetActive(false);
            StopButton.gameObject.SetActive(true);
            PauseButton.gameObject.SetActive(false);
            ResumeButton.gameObject.SetActive(true);
            OnPauseButtonClicked.Invoke();
        }

        private void OnResumeButtonClick()
        {
            if (_state is not ControlState.Paused) return;
            _state = ControlState.Playing;

            PlayButton.gameObject.SetActive(false);
            StopButton.gameObject.SetActive(true);
            PauseButton.gameObject.SetActive(true);
            ResumeButton.gameObject.SetActive(false);
            OnResumeButtonClicked.Invoke();
        }
    }
}
