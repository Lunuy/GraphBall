#nullable enable
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class GlobalSound : MonoBehaviour
    {
        private AudioSource? _audioSource;
        private Coroutine? _coroutine;
        private const float AudioFadeInDuration = 1f;
        private const float AudioFadeOutDuration = 1f;

        // ReSharper disable once UnusedMember.Local
        private void Awake() => _audioSource = GetComponent<AudioSource>();

        public void ChangeSound(AudioClip audioClip)
        {
            if (_audioSource!.isPlaying && _audioSource.clip == audioClip)
                return;
            if (_audioSource.isPlaying)
            {
                if (_coroutine != null)
                    StopCoroutine(_coroutine);
                _coroutine = StartCoroutine(FadeAudio(_audioSource, false, AudioFadeOutDuration, () =>
                {
                    _audioSource.Stop();
                    _audioSource.clip = audioClip;
                    _audioSource.Play();
                    if (_coroutine != null)
                        StopCoroutine(_coroutine);
                    _coroutine = StartCoroutine(FadeAudio(_audioSource, true, AudioFadeInDuration, null));
                }));
            }
            else
            {
                _audioSource.volume = 1f;
                _audioSource.clip = audioClip;
                _audioSource.Play();
            }
        }

        public void Stop()
        {
            if (!_audioSource!.isPlaying) return;
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(FadeAudio(_audioSource, false, AudioFadeOutDuration,
                () => { _audioSource.Stop(); }));
        }

        public static IEnumerator FadeAudio(AudioSource audioSource, bool isFadeIn, float duration, Action? onComplete)
        {
            audioSource.volume = isFadeIn ? 0 : 1;
            var elapsedTime = 0f;
            while (elapsedTime <= duration)
            {
                yield return null;
                elapsedTime += Time.unscaledDeltaTime;
                audioSource.volume = Mathf.Lerp(isFadeIn ? 0 : 1, isFadeIn ? 1 : 0, elapsedTime / duration);
            }
            onComplete?.Invoke();
        }
    }
}
