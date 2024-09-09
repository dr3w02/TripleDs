using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class TurnOnMusic : MonoBehaviour
    {
        public GameObject MusicPrefab;
        public GameObject MusicBoxParent;

        private void OnTriggerEnter(Collider other)
        {
            MusicPrefab.SetActive(true);
            MusicBoxParent.SetActive(true);
        }
        private void OnTriggerStay(Collider other)
        {
            MusicPrefab.SetActive(true);
            MusicBoxParent.SetActive(true);
        }
        private void OnTriggerExit(Collider other)
        {
            MusicPrefab.SetActive(true);
            MusicBoxParent.SetActive(true);
        }
    }
}
