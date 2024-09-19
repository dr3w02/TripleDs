using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;
using UnityEngine.UIElements.Experimental;

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
        public GameObject FourtySix;
        // Player
        // Transforms of character
        public Transform player;
        // Physical GameObject
        public GameObject mCharacter;/// <summary>
        /// //check you!!!!!!!!!!!!!!!!!!!!!!!!
        /// </summary>
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

        [SerializeField] private float WaitTime = 0.5f;

        public bool running;

        public bool Chase;


        // Music Box Logic
        public bool MusicPlay;
    
        bool alreadyAttacked;

        public void Start()
        {
            OrphanDeathCam.SetActive(false);

            FourtySix.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

           
            if (MusicPlay)
            {
                Sleeping();
                running = false;
            }

            else if (!MusicPlay)
            {
                running = true;
                Chase = false;
            }


            if (!MusicPlay && PlayerDect.CanDetectPlayer() && !PlayerDect.CanAttackPlayer())
            {
                    running = false; // Stop running and start chasing
                    Chase = true;
                    Chasing();
                    Debug.Log("CHASING");
                   
            }


            else if (!MusicPlay && PlayerDect.CanDetectPlayer() && PlayerDect.CanAttackPlayer())
            {
                Chase = false;
                running = false;
                AttackPlayer();
                Debug.Log("CATTACKINGHH");
               
            }

            else if (!MusicPlay && !PlayerDect.CanDetectPlayer() && !PlayerDect.CanAttackPlayer())
            {
                Debug.Log("Running");
                running = true;
                
            }


        }

        public void Sleeping()
        {
            //Debug.Log("Sleeping");

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
            if (running == true)
            {
                Debug.Log("Running");
                Vector3 destination = waypointsOrphan[index].transform.position;
                Vector3 newPos = Vector3.MoveTowards(transform.position, waypointsOrphan[index].transform.position, speed * Time.deltaTime);

                transform.position = newPos;

                orphanAnim.SetBool("RunFWD", true);
                orphanAnim.SetBool("Idel", false);
                orphanAnim.SetBool("Attack", false);

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
            
        }

     

        public void Chasing()
        {
            if  (Chase == true)
            {
                if (player == null || orphanAnim == null || orphan == null)
                {
                    Debug.LogError("Missing references in Chasing method");
                    return;
                }

                Debug.Log("Chase");

                orphanAnim.SetBool("RunFWD", true);
                orphanAnim.SetBool("Idel", false);
                orphanAnim.SetBool("Attack", false);


                Vector3 newPos = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                transform.position = newPos;

                Vector3 directionToTarget = player.transform.position - orphan.transform.position;
                directionToTarget.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                orphan.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, targetRotation, Time.deltaTime * 300f);
            }
           


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

             orphanAnim.SetBool("Attack", true);

             orphanAnim.SetBool("RunFWD", false);
                
             OrphanDeathCam.SetActive(true);

             FourtySix.SetActive(true);

             StartCoroutine(WaitBetweenFadeInOut());

             Debug.Log("Switch Camera");
            
        }

        public IEnumerator WaitBetweenFadeInOut()
        {

            mCharacter.SetActive(false);
            yield return new WaitForSeconds(WaitTime);

            

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
            FourtySix.SetActive(false);
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
                    //GetComponent<NavMeshAgent>().isStopped = false;
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
