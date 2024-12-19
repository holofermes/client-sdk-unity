using System;
using UnityEngine;

namespace LiveKit
{
    public abstract class BaseAudioSource : MonoBehaviour
    {
        public abstract event Action<float[], int, int> AudioRead;
        public abstract uint Channels { get; }
        public abstract bool IsPlaying { get; }
        public abstract void Play();
        public abstract void Stop();
    }
}