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
            //if (audioDownPlay)
            //{
            //    if (!windDOWN.isPlaying)
            //    {
            //        windDOWN.Play();
            //    }
            //    windUP.Pause();
            //    Debug.Log("WindDown");
            //}
            //else if (!audioDownPlay)
            //{
            //    windDOWN.Pause();
            //    if (!windUP.isPlaying)
            //    {
            //        windUP.Play();
            //    }
            //    Debug.Log("WindUp");
            //}
        }

        public void PlayWindUp()
        {
            Debug.Log($"<color=green> Wind up </color>");
            windDOWN.Stop();
            windUP.Play();
        }

        public void PlayWindDown()
        {
            Debug.Log($"<color=#FFC0CB> Wind down </color>");
            windDOWN.Play();
            windUP.Stop();
        }
      
          
    }
}
