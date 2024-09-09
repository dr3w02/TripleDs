using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

namespace Platformer
{
    public class OrphanWaypointFollow : MonoBehaviour
    {
        public List<GameObject> waypointsOrphan;
        public float speed = 2;
        int index = 0;
        public bool isLoop = true;
        public GameObject orphan;
        public Animator orphanAnim;
        public GameObject sleepingPoint;

        // Player
        // Transforms of character
        readonly Transform player;
        // Physical GameObject
        public GameObject mCharacter;
        // Script
        public characterMovement characterMain;
        // Player

        // Handle the fade out and in
        [SerializeField] private bool fadeIn = false;
        [SerializeField] private bool fadeOut = true;
        [SerializeField] public CanvasGroup myUIGroup;
        [SerializeField] private float fadeWaitTime = 4f;

        public GameObject OrphanDeathCam;
        public PlayerDetector PlayerDect;
        //readonly NavMeshAgent orphanNav;

        [SerializeField] private float WaitTime = 3f;

  

        // Music Box Logic
        public bool MusicPlay;
    
        bool alreadyAttacked;

        public void Start()
        {
            OrphanDeathCam.SetActive(false);
          
        }

        // Update is called once per frame
        void Update()
        {


            // && !PlayerDect.CanAttackPlayer()
            if (PlayerDect.CanDetectPlayer())
            {
                Chasing();
                Debug.Log("CHASING");
            }

            if (PlayerDect.CanDetectPlayer() && PlayerDect.CanAttackPlayer())
            {
                AttackPlayer();
                Debug.Log("CATTACKINGHH");
            }

            if (MusicPlay)
            {
                Sleeping();
            }
            else
            {
                Running();
            }
        }

        public void Sleeping()
        {
            Debug.Log("Sleeping");

            Vector3 destinationSleep = sleepingPoint.transform.position;

            Vector3 newPos = Vector3.MoveTowards(transform.position, sleepingPoint.transform.position, speed * Time.deltaTime);

            transform.position = newPos;

            float distanceSleep = Vector3.Distance(transform.position, destinationSleep);

            orphanAnim.SetBool("RunFWD", true);
            orphanAnim.SetBool("Idel", false);

            if (distanceSleep == 0)
            {
                orphanAnim.SetBool("RunFWD", false);
                orphanAnim.SetBool("Idel", true);
            }

            Vector3 directionToTarget = sleepingPoint.transform.position - orphan.transform.position;

            directionToTarget.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            orphan.transform.rotation = Quaternion.RotateTowards(orphan.transform.rotation, targetRotation, Time.deltaTime * 300f);
        }

        public void Running()
        {
            Debug.Log("Running");
            Vector3 destination = waypointsOrphan[index].transform.position;
            Vector3 newPos = Vector3.MoveTowards(transform.position, waypointsOrphan[index].transform.position, speed * Time.deltaTime);

            transform.position = newPos;

            orphanAnim.SetBool("RunFWD", true);
            orphanAnim.SetBool("Idel", false);

            float distance = Vector3.Distance(transform.position, destination);

            if (distance <= 0.0001)
            {
                if (index < waypointsOrphan.Count - 1)
                {
                    index++;
                }
                else if (isLoop)
                {
                    index = 0;
                }
            }

            Vector3 directionToTarget = waypointsOrphan[index].transform.position - orphan.transform.position;
            directionToTarget.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            orphan.transform.rotation = Quaternion.RotateTowards(orphan.transform.rotation, targetRotation, Time.deltaTime * 300f);
        }

     

        public void Chasing()
        {
            
            
            Debug.Log("Chase");
            orphanAnim.SetBool("RunFWD", true);
            orphanAnim.SetBool("Idel", false);

            // orphanNav.SetDestination(player.position);
            //
            ///Setting it manually
            //  Vector3 directionToPlayer = (player.transform.position - orphan.transform.position).normalized;
            // orphan.transform.position = Vector3.MoveTowards(orphan.transform.position, player.transform.position, speed * Time.deltaTime);

           // Vector3 newPosition = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            //transform.position = newPosition;

           // Vector3 directionToTarget = player.transform.position - orphan.transform.position;
           // directionToTarget.y = 0;

           // Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
           // orphan.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, targetRotation, Time.deltaTime * 300f);


            if (!alreadyAttacked)
            {
                // Attack animation logic here

                alreadyAttacked = true;

                ResetAttack();
            }
        }

        public void AttackPlayer()
        {
       
                OrphanDeathCam.SetActive(false);
                characterMain.TurnOffMovement();

                //orphanNav.isStopped = true;
                OrphanDeathCam.SetActive(true);

                orphanAnim.SetBool("RunFWD", true);

                StartCoroutine(WaitBetweenFadeInOut());

                Debug.Log("Switch Camera");
            
        }

        public IEnumerator WaitBetweenFadeInOut()
        {
            yield return new WaitForSeconds(WaitTime);

            mCharacter.SetActive(false);
            Debug.Log("Switch Camera");

            fadeIn = true;

            while (fadeIn)
            {
                if (myUIGroup.alpha < 1)
                {
                    myUIGroup.alpha += Time.deltaTime;
                    yield return null; // Wait for the next frame
                }
                else
                {
                    fadeIn = false;
                }
            }

            characterMain.RespawnPlayer();

            // Wait for the specified amount of time
            yield return new WaitForSeconds(fadeWaitTime);

            fadeOut = true;
            while (fadeOut)
            {
                if (myUIGroup.alpha > 0)
                {
                    myUIGroup.alpha -= Time.deltaTime;
                    yield return null; // Wait for the next frame
                    OrphanDeathCam.SetActive(false);
                    mCharacter.SetActive(true);
                    characterMain.Enabled();
                    GetComponent<NavMeshAgent>().isStopped = false;
                }
                else
                {
                    fadeOut = false;
                }
            }
        }

        private void ResetAttack()
        {
            alreadyAttacked = false;
        }
    }
}
