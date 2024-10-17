using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Platformer
{
    public class TeleportToPointLibary : MonoBehaviour
    {
        public GameObject player;
        public GameObject PointAfterClimb;
        public ClimbingCharacter ladder;
        public Climbable climbable;
        public void OnTriggerEnter(Collider other)
        {

            if (ladder.isPullPressed)
            {
                climbable.canClimb = false;
                player.transform.position = PointAfterClimb.transform.position;
                ladder.DropLadder();
            }
           


        }
        


    }
}
