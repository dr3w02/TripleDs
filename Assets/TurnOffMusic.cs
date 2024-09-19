using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class TurnOffMusic : MonoBehaviour
    {
        public GameObject MusicPrefab;
        public GameObject MusicBoxParent;

        private void OnTriggerEnter(Collider other)
        {
            MusicPrefab.SetActive(false);
            MusicBoxParent.SetActive(false);
        }
        private void OnTriggerStay(Collider other)
        {
            MusicPrefab.SetActive(false);
            MusicBoxParent.SetActive(false);
        }
        private void OnTriggerExit(Collider other)
        {
            MusicPrefab.SetActive(false);
            MusicBoxParent.SetActive(false);
        }
    }
}
