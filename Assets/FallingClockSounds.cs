using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class FallingClockSounds : MonoBehaviour
    {
        public AudioSource clockFall;
        public AudioSource grandfatherDingDong;

        public void PlayClockBigHit()
        {
            clockFall.Play();
        }

        public void PlayDingDong()
        {
            grandfatherDingDong.Play();
        }
    }
}
