using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class BlackbeakSounds : MonoBehaviour
    {
        public AudioSource stompSound;
        //public AudioSource neckCrackSound;

        public void Stomp()
        {
            stompSound.Play();
        }
    }
}
