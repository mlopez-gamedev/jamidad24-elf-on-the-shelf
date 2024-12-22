using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace MiguelGameDev
{
    public class Audio2dService : GlobalMonoSingleton<Audio2dService>
    {
        [SerializeField]
        private AudioChannelInfo[] _audioChannels;

        [SerializeField]
        private AudioEventInfo[] _audioEventClips;

        private Dictionary<EAudioChannel, AudioChannel> _audioChannelSources;
        private Dictionary<EAudioEvent, AudioEventInfo> _audioIdToClip;


        protected override void Awake()
        {
            base.Awake();
            Setup();
        }

        private void Setup()
        {
            _audioChannelSources = new Dictionary<EAudioChannel, AudioChannel>();
            foreach (AudioChannelInfo audioChannel in _audioChannels)
            {
                Assert.IsNotNull(audioChannel.MixerGroup);
                Assert.IsFalse(_audioChannelSources.ContainsKey(audioChannel.Channel));

                _audioChannelSources.Add(audioChannel.Channel, new AudioChannel(
                    audioChannel,
                    gameObject));
            }

            _audioIdToClip = new Dictionary<EAudioEvent, AudioEventInfo>();
            foreach (AudioEventInfo audioEventClip in _audioEventClips)
            {
                Assert.IsFalse(_audioIdToClip.ContainsKey(audioEventClip.Event));
                _audioIdToClip.Add(audioEventClip.Event, audioEventClip);
            }
        }

        public (AudioSource, AudioClip) PlayAudio(EAudioEvent audioEvent)
        {
            Assert.IsTrue(_audioIdToClip.ContainsKey(audioEvent), $"Audio Event {audioEvent} doesn't exist");
            var audioInfo = _audioIdToClip[audioEvent];

            Assert.IsTrue(_audioChannelSources.ContainsKey(audioInfo.Channel));
            var audioChannel = _audioChannelSources[audioInfo.Channel];

            if (audioInfo.Clip == null)
            {
                Debug.LogError($"Audio Event {audioEvent} clip is null");
            }

            return audioChannel.Play(audioInfo);
        }


        public UniTask StopAudio(EAudioEvent audioEvent)
        {
            Assert.IsTrue(_audioIdToClip.ContainsKey(audioEvent), $"Audio Event {audioEvent} doesn't exist");
            var audioInfo = _audioIdToClip[audioEvent];

            Assert.IsTrue(_audioChannelSources.ContainsKey(audioInfo.Channel));
            var audioChannel = _audioChannelSources[audioInfo.Channel];

            return audioChannel.StopAudio(audioInfo);
        }
    }
}
