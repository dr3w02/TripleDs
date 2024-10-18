using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class BlackbeakSounds : MonoBehaviour
    {
        public AudioSource stompSound;
        public AudioSource bbDeadSound;

        public void Stomp()
        {
            stompSound.Play();
        }

        public void BBDead()
        {
            bbDeadSound.Play();
        }
    }
}
