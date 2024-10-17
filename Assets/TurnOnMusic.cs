using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class TurnOnMusic : MonoBehaviour
    {
        public GameObject MusicPrefab;
        public GameObject MusicBoxParent;
        public GameObject TimerController;
      

        private void OnTriggerEnter(Collider other)
        {
            MusicPrefab.SetActive(true);
            MusicBoxParent.SetActive(true);

            TimerController.SetActive(true);
        }
        private void OnTriggerStay(Collider other)
        {
            MusicPrefab.SetActive(true);
            MusicBoxParent.SetActive(true);
            TimerController.SetActive(true);
            Debug.Log("IN MSUIC EAREA");
        }
        private void OnTriggerExit(Collider other)
        {
            MusicPrefab.SetActive(true);
            MusicBoxParent.SetActive(true);
        }

        



    }
}
