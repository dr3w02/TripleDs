using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlayerCheckPoint : MonoBehaviour
    {
        [SerializeField] private GameObject _checkpointsParents;
        public GameObject[] _checkPointsArray;
        
        public GameObject flag;
        private Vector3 spawnPoint;
        void Start()
        {
            spawnPoint = gameObject.transform.position;
        }


       
        // Update is called once per frame
        void Update()
        {

            if (gameObject.transform.position.y < 20f)
            {
                gameObject.transform.position = spawnPoint;
            }


        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("CheckPoint"))
            {
                spawnPoint = flag.transform.position;
                Destroy(flag);
            }
        }

    }
}
