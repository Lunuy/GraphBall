#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    public class AudioPlayTrigger : MonoBehaviour
    {
        public AudioClip? AudioClip;
        public GlobalSound? GlobalSound;
        public bool PlayOnStart = true;

        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            if (AudioClip == null) return;
            if (GlobalSound == null) return;
            if (!PlayOnStart) return;

            GlobalSound.ChangeSound(AudioClip);
        }

        public void SetSceneData(IReadOnlyDictionary<string, List<Object>> dict)
        {
            GlobalSound = (GlobalSound)dict["GlobalSound"][0];
        }
    }
}
