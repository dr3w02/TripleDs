using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class StopClimb : MonoBehaviour
    {
      
        
        public ClimbingCharacter ladder;
        public Climbable climbable;
        public void OnTriggerExit(Collider other)
        {
            climbable.canClimb = false;
            
            ladder.DropLadder();


        }
    }
}
