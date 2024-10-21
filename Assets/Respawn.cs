using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer
{
    public class Respawn : MonoBehaviour
    {
        public bool respawnStart;
        public GameObject Player;
        [SerializeField] private GameObject _checkpointsParent;
        public GameObject[] _checkPointsArray;

        public int savedCheckpointIndex = -1;

        private Vector3 _startingPoint;
        public GameObject _startingPointGameobject;


        public Animator AnimCharacter;

        private const string SAVE_CHECKPOINT_INDEX = "Last_checkpoint_index";

        public RBController playerscript;
        public Rigidbody _rigidbody;

        public bool wakeup;

        public FinalBossAnim finalBossReset;
        public GameObject playerAnim;

        private void Awake()
        {
            loadCheckPoints();
            Debug.LogWarning("CheckpointScript");

        }
    


        // Start is called before the first frame update
        void Start()
        {     
            savedCheckpointIndex = PlayerPrefs.GetInt(SAVE_CHECKPOINT_INDEX, -1);
            if (savedCheckpointIndex != -1)
            {
               playerscript.TurnOffMovement();
                _startingPoint = _checkPointsArray[savedCheckpointIndex].transform.position;

                _startingPointGameobject = _checkPointsArray[savedCheckpointIndex];


                AnimCharacter.SetBool("isWakeUp", true);

                StartCoroutine(WaitTime());


            }
            else
            {
                _startingPoint = gameObject.transform.position;
                _startingPointGameobject = null;
                Debug.Log("NoCheckpoint");
            }

            if(respawnStart)
                RespawnPlayer();
        }


        private IEnumerator WaitTime()
        {
            yield return new WaitForSeconds(4f);
            AnimCharacter.SetBool("isWakeUp", false); 
            playerscript.Enabled();
            

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

        public bool resetbook;

        public void RespawnPlayer()
        {
            resetbook = true;
            Player.SetActive(true);
            transform.position = _startingPoint;

            playerAnim.transform.position = transform.position;

            playerscript.Enabled();
            playerscript.StopCrouch();
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            Debug.Log("WorkingReset");

            if (finalBossReset.bossReset)
            {
                finalBossReset.ResetWholeBossFight();

            }
            

        }


        //CheckPonts
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("CheckPoint"))
            {
                savedCheckpointIndex = Array.FindIndex(_checkPointsArray, match => match == other.gameObject);

                if (savedCheckpointIndex != -1)
                {
                    PlayerPrefs.SetInt(SAVE_CHECKPOINT_INDEX, savedCheckpointIndex);
                    _startingPoint = _checkPointsArray[savedCheckpointIndex].transform.position;
                    _startingPointGameobject = _checkPointsArray[savedCheckpointIndex];
                    // other.gameObject.SetActive(false);


                    Debug.Log("ChangedChckpoint");
                }
            }
            

        }

    }
}
