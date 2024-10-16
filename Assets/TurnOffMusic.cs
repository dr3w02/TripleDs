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

        private void OnTriggerEnter(Collider other)
        {
            MusicPrefab.SetActive(false);
            MusicBoxParent.SetActive(false);
            TimerController.SetActive(true);
            flashLightInHand.SetActive(false);
        }
        private void OnTriggerStay(Collider other)
        {
            MusicPrefab.SetActive(false);
            MusicBoxParent.SetActive(false);
            TimerController.SetActive(true);
            flashLightInHand.SetActive(false);
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
