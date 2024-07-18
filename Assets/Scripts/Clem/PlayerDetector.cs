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
        // CountDownTimer ditectionTimer;


        IDetectionStrategy detectionStratery;

    }



}

