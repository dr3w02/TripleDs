using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlayerDetectorOrphan : MonoBehaviour
    {
        [SerializeField] float detectionAngle = 60f; //detection cone in front of ememy
        [SerializeField] float detectionRadius = 10f; // cone distance from enemy
        [SerializeField] float innerDetectionRadius = 5f; // small circle around enemy ot know if player is behind 
        [SerializeField] float detectionCooldown = 1f; // ives the player a break between attacks 
        [SerializeField] float attackRange = 2f; // Distance from enemy to player attack 


        public Transform Player { get; private set; }

        CountdownTimer detectionTimer;

        IDetectionStrategy detectionStrategy;

       

        void Awake()
        {

        


        }

        void Start()
        {
            detectionTimer = new CountdownTimer(detectionCooldown);
            detectionStrategy = new ConeDetectionStrategy(detectionAngle, detectionRadius, innerDetectionRadius);
        }

        private void Update() => detectionTimer.Tick(Time.deltaTime);


        public bool CanDetectPlayer()
        {

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
