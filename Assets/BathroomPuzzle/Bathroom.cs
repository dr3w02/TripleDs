using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Bathroom : MonoBehaviour
    {
        public GameObject PlayerDead;
        public Collider ElectricColliderTrigger;

        public GameObject Character;
        public characterMovement characterMain;

        public GameObject BathroomDeathCam;

        public Animator animator1, animator2, animator3, animator4, animator5, animator6;
        public Collider lever1, lever2, lever3, lever4, lever5, lever6;
        public AudioClip leverSoundEffect;

        private void Start()
        {
            if (PlayerDead != null)
                PlayerDead.SetActive(false);

            if (BathroomDeathCam != null)
                BathroomDeathCam.SetActive(false);
        }

        private void OnTriggerEnter(Collider ElectricColliderTrigger)
        {
            if (ElectricColliderTrigger.CompareTag("Player"))
            {
                Character.SetActive(false);
                PlayerDead.SetActive(true);
                BathroomDeathCam.SetActive(true);

                if (characterMain != null)
                {
                    characterMain.enabled = false;
                }
            }
        }

    }
}
