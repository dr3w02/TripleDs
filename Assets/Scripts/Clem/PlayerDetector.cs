using Unity.VisualScripting;
using UnityEngine;


namespace Platformer
{
    public class PlayerDetector : MonoBehaviour
    {
        [SerializeField] float detectionAngle = 60f; //detection cone in front of ememy
        [SerializeField] float detectionRadius = 10f; // cone distance from enemy
        [SerializeField] float innerDetectionRadius = 5f; // small circle around enemy ot know if player is behind 
        [SerializeField] float detectionCooldown = 1f; // ives the player a break between attacks 

        public Transform Player { get; private set; }

        CountdownTimer detectionTimer;

        IDetectionStrategy detectionStrategy;
        


        void Start()
        {
            detectionTimer = new CountdownTimer(detectionCooldown);
            Player = GameObject.FindGameObjectWithTag("Player").transform;  //make sure to TAG the player
            detectionStrategy = new ConeDetectionStrategy(detectionAngle, detectionRadius, innerDetectionRadius);
            Debug.Log("detector");
        }


        private void Update() => detectionTimer.Tick(Time.deltaTime);


        public bool CanDetectPlayer()
        {
            Debug.Log("DetectingPlayer");
            return detectionTimer.IsRunning || detectionStrategy.Execute(Player, transform, detectionTimer);
        }


        public void SetDetectionStrategy(IDetectionStrategy detectionStrategy) => this.detectionStrategy = detectionStrategy;

    }
}



