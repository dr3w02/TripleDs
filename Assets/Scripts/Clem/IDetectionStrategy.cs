using UnityEngine;


namespace Platformer
{
    public interface IDetectionStrategy
    {
        bool Execute (Transform player, Transform detector, CountdownTimer timer);

    }


    public class ConeDetectionStratagy : IDetectionStrategy
    {
        public ConeDetectionStratagy( float detectionAngle, float detectionRadius, float innerDetectionRadius)
        {
            this.detectionAngle = detectionAngle;
            this.detectionRadius = detectionRadius;
            this.innerDetectionRadius = innerDetectionRadius;



        }

        public bool Execute(Transform player, Transform detector, CountdownTimer timer)
        {

        }

        //13.23
    }



}

