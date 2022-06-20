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

        }

        private void OnStopButtonClick()
        {

        }

        private void OnPauseButtonClick()
        {
            
        }

        private void OnResumeButtonClick()
        {

        }
    }
}
