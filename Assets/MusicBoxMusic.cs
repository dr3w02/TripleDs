using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class MusicBoxMusic : MonoBehaviour
    {
        public AudioSource windUP;
        public AudioSource windDOWN;

        public bool audioDownPlay;

        public void Update()
        {
            if (audioDownPlay)
            {
                windDOWN.Play();
                windUP.Pause();
                Debug.Log("WindDown");
            }
            else if (!audioDownPlay)
            {
                windDOWN.Pause();
                windUP.Play();
                Debug.Log("WindUp");
            }
        }
       
          
    }
}
