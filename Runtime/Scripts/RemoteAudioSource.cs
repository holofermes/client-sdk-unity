using System;
using UnityEngine;

namespace LiveKit
{
    [RequireComponent(typeof(AudioSource), typeof(AudioFilter))]
    public class RemoteAudioSource : BaseAudioSource
    {
        [SerializeField] private AudioSource source;
        private AudioFilter _audioFilter;

        public override bool IsPlaying => source.isPlaying;
        public override RtcAudioSourceType AudioSourceType => RtcAudioSourceType.AudioSourceCustom;
        public override uint Channels => 2;
        public AudioSource Source => source;
        public override event Action<float[], int, int> AudioRead;
        private Action<float[], int, int> _audioRead;

        private void Awake()
        {
            if(source == null) source = GetComponent<AudioSource>();
            _audioFilter = GetComponent<AudioFilter>();
        }

        private void AudioFilterOnAudioRead(float[] data, int channels, int samplerate)
        {
            AudioRead?.Invoke(data, channels, samplerate);
        }

        public override void Play()
        {
            _audioFilter.AudioRead += AudioFilterOnAudioRead;
            source.Play();
        }

        public override void Stop()
        {
            _audioFilter.AudioRead -= AudioFilterOnAudioRead;
            source.Stop();
        }

    }
}