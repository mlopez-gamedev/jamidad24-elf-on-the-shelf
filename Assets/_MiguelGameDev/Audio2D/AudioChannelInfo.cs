using UnityEngine;
using UnityEngine.Audio;

namespace MiguelGameDev
{
    [System.Serializable]
    public struct AudioChannelInfo
    {
        [SerializeField] EAudioChannel _channel;
        [SerializeField] AudioMixerGroup _mixerGroup;
        [SerializeField] bool _loop;

        public EAudioChannel Channel => _channel;
        public AudioMixerGroup MixerGroup => _mixerGroup;
        public bool Loop => _loop;
    }
}
