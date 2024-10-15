using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class FallingClockSounds : MonoBehaviour
    {
        FallingClock clock;

        public AudioSource clockFall;
        public AudioSource grandfatherDingDong;

        private void Start()
        {
            clock = GetComponentInParent<FallingClock>();
        }

        public void PlayClockBigHit()
        {
            clockFall.Play();
        }

        public void PlayDingDong()
        {
            grandfatherDingDong.Play();
            StopCameraShake();
        }

        public void StopCameraShake()
        {
            clock.CamShake = false;
            clock.StopShake();
        }
    }
}
