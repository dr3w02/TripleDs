using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

namespace Platformer
{
    public class OrphanWaypointFollow : MonoBehaviour
    {
        public List <GameObject> waypointsOrphan;
        public float speed = 2;
        int index = 0;
        public bool isLoop = true;
        public GameObject orphan;
        public Animator orphanAnim;
        public GameObject sleepingPoint;

       
        /// <summary>
        // Music Box Logic 

        public bool MusicPlay;


        /// </summary>
        // Start is called before the first frame update
        void Start()
        {
            
        }
        
        // Update is called once per frame
         void Update()
         {
            if (MusicPlay)
            {
                Sleeping();
            }

            else if (!MusicPlay)
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

                else
                {
                    if (isLoop)
                    {
                        index = 0;
                    }

                }

            }

            Vector3 directionToTarget = waypointsOrphan[index].transform.position - orphan.transform.position;

            directionToTarget.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            orphan.transform.rotation = Quaternion.RotateTowards(orphan.transform.rotation, targetRotation, Time.deltaTime * 300f);


           
        }

        public void Attacking(PlayerDetectorOrphan Detection)
        {

            if (Detection.CanAttackPlayer)
            {

            }


        }


    }
    
   
    
        
    
}
