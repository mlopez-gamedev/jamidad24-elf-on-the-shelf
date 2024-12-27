using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace MiguelGameDev
{
    public class AudioChannel
    {
        private AudioChannelInfo _channelInfo;
        private GameObject _container;
        private AudioSource _audioSource;

        public AudioChannel(AudioChannelInfo channelInfo, GameObject container)
        {
            _channelInfo = channelInfo;
            _container = container;
            _audioSource = CreateAudioSource();
        }

        private AudioSource CreateAudioSource()
        {
            var audioSource = _container.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = _channelInfo.MixerGroup;
            audioSource.loop = _channelInfo.Loop;
            return audioSource;
        }

        public (AudioSource, AudioClip) Play(AudioEventInfo audioInfo)
        {
            if (audioInfo.IsOneShot)
            {
                _audioSource.PlayOneShot(audioInfo.Clip, audioInfo.Volume);
                return (_audioSource, audioInfo.Clip);
            }

            _audioSource.clip = audioInfo.Clip;
            if (audioInfo.FadeOutDuration == 0)
            {
                _audioSource.volume = audioInfo.Volume;
                _audioSource.Play();
            }
            else
            {
                _audioSource.volume = 0;
                _audioSource.Play();
                _audioSource.DOFade(audioInfo.Volume, audioInfo.FadeOutDuration);
            }

            return (_audioSource, audioInfo.Clip);
        }

        public async UniTask StopAudio(AudioEventInfo audioInfo)
        {
            if (!_audioSource.isPlaying)
            {
                return;
            }

            if (_audioSource.clip != audioInfo.Clip)
            {
                return;
            }

            if (audioInfo.FadeInDuration > 0)
            {
                await _audioSource.DOFade(0, audioInfo.FadeInDuration).AsyncWaitForCompletion();
                if (_audioSource.clip != audioInfo.Clip)
                {
                    return;
                }
            }
            _audioSource.Stop();
        }
    }
}
