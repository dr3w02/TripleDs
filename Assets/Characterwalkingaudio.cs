using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Characterwalkingaudio : MonoBehaviour
    {
        [SerializeField]
        private AudioClip WoodenFootsteps;

        [SerializeField]
        private AudioClip TileFootSteps;

        [SerializeField]
        private AudioClip CarpetFootSteps;

        [SerializeField]
        private AudioClip[] FootstepsArray;

        public void walkingonwoodfunction1()
        {
            AudioSource.PlayClipAtPoint(WoodenFootsteps, transform.position);
        }

        public void walkingontilefunction2()
        {
            AudioSource.PlayClipAtPoint(TileFootSteps, transform.position);
        }

        public void walkingoncarpetfunction3()
        {
            AudioSource.PlayClipAtPoint(CarpetFootSteps, transform.position);
        }

        public void RandomWoodenFootstepsFunctions()
        {
            int randomType = UnityEngine.Random.Range(0, 2);
            AudioSource.PlayClipAtPoint(FootstepsArray[randomType], transform.position);
        }
    }
}
