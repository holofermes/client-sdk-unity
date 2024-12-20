using System;
using System.Collections;
using UnityEngine;

namespace LiveKit
{
    public abstract class BaseAudioSource : MonoBehaviour
    {
        public abstract event Action<float[], int, int> AudioRead;
        public abstract RtcAudioSourceType AudioSourceType { get; }
        public abstract uint Channels { get; }
        public abstract bool IsPlaying { get; }
        public virtual IEnumerator Prepare(float timeout = 0) { yield break;  }
        public abstract void Play();
        public abstract void Stop();
    }
}