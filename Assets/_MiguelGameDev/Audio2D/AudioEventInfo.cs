using UnityEngine;

namespace MiguelGameDev
{
    [System.Serializable]
    public class AudioEventInfo
    {
        [SerializeField] EAudioEvent _event;
        [SerializeField] EAudioChannel _channel;
        [SerializeField] bool _isOneShot;
        [SerializeField] AudioClip _clip;
        [SerializeField] float _volume = 1f;
        [SerializeField] float _fadeOutDuration = 0;
        [SerializeField] float _fadeInDuration = 0;

        public EAudioEvent Event => _event;
        public EAudioChannel Channel => _channel;
        public bool IsOneShot => _isOneShot;
        public AudioClip Clip => _clip;
        public float Volume => _volume;
        public float FadeOutDuration => _fadeOutDuration;
        public float FadeInDuration => _fadeInDuration;
    }
}
