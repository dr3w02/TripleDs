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


        private void Start()
        {
            PlayerDead.SetActive(false);
            BathroomDeathCam.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
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
