using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


namespace Platformer
{
    public class PlayerDetector : MonoBehaviour
    {
        [SerializeField] float detectionAngle = 0f; //detection cone in front of ememy
        [SerializeField] public float detectionRadius = 3f; // cone distance from enemy
        [SerializeField] public float innerDetectionRadius = 1.6f; // small circle around enemy ot know if player is behind 
        [SerializeField] float detectionCooldown = 1.5f; // ives the player a break between attacks 
        [SerializeField] public float attackRange = 2f; // Distance from enemy to player attack 

        
       

        public Transform Player { get; private set; }

        CountdownTimer detectionTimer;

        IDetectionStrategy detectionStrategy;

        public NurseCodeOffice enemy;

        void Awake()
        {
           
            Player = GameObject.FindGameObjectWithTag("Player").transform; 
       
       
        }

        void Start()
        {
            detectionTimer = new CountdownTimer(detectionCooldown);
            //detectionStrategy = new ConeDetectionStrategy(detectionAngle, detectionRadius, innerDetectionRadius);
        }



        private void Update() => detectionTimer.Tick(Time.deltaTime);

            
        public bool CanDetectPlayer()
        {
            if (enemy == null)
                return false;

          
            detectionStrategy = new ConeDetectionStrategy(detectionAngle, detectionRadius, innerDetectionRadius);

            

            return detectionTimer.IsRunning || detectionStrategy.Execute(Player, transform, detectionTimer);

   

        }

        public bool CanAttackPlayer()
        {
            var directionToPlayer = Player.position - transform.position;
            return directionToPlayer.magnitude <= attackRange;



        }


      

        public void SetDetectionStrategy(IDetectionStrategy detectionStrategy) => this.detectionStrategy = detectionStrategy;


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.DrawWireSphere(transform.position, innerDetectionRadius);

            Vector3 fowardConeDirection = Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward * detectionRadius;
            Vector3 backwardConeDirection = Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward * detectionRadius;

            //Drawlines represent the cone

            Gizmos.DrawLine(transform.position, transform.position + fowardConeDirection);
            Gizmos.DrawLine(transform.position, transform.position + backwardConeDirection);


        }
    }
}



