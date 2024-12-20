using System;
using System.Collections;
using UnityEngine;

namespace LiveKit
{
    [RequireComponent(typeof(AudioSource), typeof(AudioFilter))]
    public class MicrophoneSource : BaseAudioSource
    {
        [SerializeField] private AudioSource source;
        private AudioFilter _audioFilter;

        public override bool IsPlaying => source.isPlaying;
        public override RtcAudioSourceType AudioSourceType => RtcAudioSourceType.AudioSourceMicrophone;
        public override uint Channels => 2;
        public override event Action<float[], int, int> AudioRead;

        public AudioSource Source => source;
        private Action<float[], int, int> _audioRead;
        private string _deviceName;

        private void Awake()
        {
            if(source == null) source = GetComponent<AudioSource>();
            _audioFilter = GetComponent<AudioFilter>();
        }

        private void AudioFilterOnAudioRead(float[] data, int channels, int samplerate)
        {
            AudioRead?.Invoke(data, channels, samplerate);
        }

        public void Configure(string device, bool loop, int lenghtSec, int frequency)
        {
            _deviceName = device;
            Source.clip = Microphone.Start(device, loop, lenghtSec, frequency);
            Source.loop = true;
        }
        
        public override IEnumerator Prepare(float timeout = 0)
        {
            return new WaitUntil(() => Microphone.GetPosition(_deviceName) > 0);
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