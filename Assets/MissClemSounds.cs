using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class MissClemSounds : MonoBehaviour
    {
        public AudioSource gruntSound;
        //public AudioSource neckCrackSound;

        public void Grunt()
        {
            gruntSound.Play();
        }
    }
}
