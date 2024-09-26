using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Platformer
{
    public class Respawn : MonoBehaviour
    {


        [SerializeField] private GameObject _checkpointsParent;
        public GameObject[] _checkPointsArray;

        private Vector3 _startingPoint;


        private const string SAVE_CHECKPOINT_INDEX = "Last_checkpoint_index";

        public RBController playerscript;
        public Rigidbody _rigidbody;

        
        private void Awake()
        {
            loadCheckPoints();
            Debug.LogWarning("CheckpointScript");
        }


        // Start is called before the first frame update
        void Start()
        {
            int savedCheckpointIndex = -1;
            savedCheckpointIndex = PlayerPrefs.GetInt(SAVE_CHECKPOINT_INDEX, -1);
            if (savedCheckpointIndex != -1)
            {
                _startingPoint = _checkPointsArray[savedCheckpointIndex].transform.position;
                
                
            }
            else
            {
                _startingPoint = gameObject.transform.position;
                Debug.Log("NoCheckpoint");
            }

            RespawnPlayer();
        }


       

        
        // Update is called once per frame
        void Update()
        {
           if (transform.position.y <= -10f)          
           {
               RespawnPlayer(); // INCASE PLAYER FALLS THROUGH GROUND 
          }
        }



        private void loadCheckPoints()
        {
            _checkPointsArray = new GameObject[_checkpointsParent.transform.childCount];

            int index = 0;

            foreach (Transform singleCheckpoint in _checkpointsParent.transform)
            {
                _checkPointsArray[index] = singleCheckpoint.gameObject;


                index++;
            }
        }

        public void RespawnPlayer()
        {

            gameObject.transform.position = _startingPoint;

            //playerscript.Enabled();
            playerscript.enabled = true;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            Debug.Log("WorkingReset");

            

        }


        //CheckPonts
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("CheckPoint"))
            {
                int checkPointIndex = -1;
                checkPointIndex = Array.FindIndex(_checkPointsArray, match => match == other.gameObject);

                if (checkPointIndex != -1)
                {
                    PlayerPrefs.SetInt(SAVE_CHECKPOINT_INDEX, checkPointIndex);
                    _startingPoint = other.gameObject.transform.position;
                    other.gameObject.SetActive(false);


                    Debug.Log("ChangedChckpoint");
                }
            }
            

        }

    }
}
