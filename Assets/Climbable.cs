using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Climbable : MonoBehaviour
    {
        public bool canClimb;
        public Transform attachPoint;

        ClimbingCharacter character;

        private void FixedUpdate()
        {
            if (canClimb)
            {
                character.CheckForLadder(this);
            }

            //else 
            //{
              //  character.DropLadder();    
           // }
         
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                character = other.GetComponent<ClimbingCharacter>();
                canClimb = true;

            }

            if(other == null)
            {
                character.DropLadder();
            }
        }

        private void OnTriggerExit(Collider other)
        {

            character = other.GetComponent<ClimbingCharacter>();
            character.DropLadder();
            canClimb = false;
            
        }

        //exit
        //can not climb
    }
}
