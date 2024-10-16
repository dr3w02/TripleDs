using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class LampSounds : MonoBehaviour
    {

        public AudioSource lampShake1Sound;
        public AudioSource lampShake2Sound;


        public AudioSource lampShatterSound;
        public AudioSource lampGhostSound;

        public void LampShake1()
        {
            lampShake1Sound.Play();
        }

        public void LampShake2()
        {
            lampShake2Sound.Play();
        }

        public void LampShatter()
        {
            lampShatterSound.Play();
        }

        public void LampGhost()
        {
            lampGhostSound.Play();
        }
    }
}
