using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class TurnOffMusic : MonoBehaviour
    {
        public GameObject MusicPrefab;
        public GameObject MusicBoxParent;
        public GameObject TimerController;
        public GameObject flashLightInHand;

        public bool Dead; 
        public void Start()
        {
            MusicBoxParent.SetActive(false);
        }
        public void Update()
        {
            if (Dead)
            {
                MusicPrefab.SetActive(false);
                MusicBoxParent.SetActive(false);

                TimerController.SetActive(false);

                Dead = false;
              
            }
        }

        private void OnTriggerStay(Collider other)
        {
            MusicPrefab.SetActive(true);
            MusicBoxParent.SetActive(true);

            TimerController.SetActive(true);
            flashLightInHand.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            MusicPrefab.SetActive(false);
            MusicBoxParent.SetActive(false);
            TimerController.SetActive(true);
            flashLightInHand.SetActive(false);
        }
    }
}
