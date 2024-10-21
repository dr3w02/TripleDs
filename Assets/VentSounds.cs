using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class VentSounds : MonoBehaviour
    {
        public AudioSource VentBang;
        public void OnTriggerEnter(Collider other)
        {
            VentBang.Play();
        }

        public void OnTriggerExit(Collider other)
        {
            VentBang.Stop();
        }
    }
}
