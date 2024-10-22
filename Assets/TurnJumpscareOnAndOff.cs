using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
  
    public class TurnJumpscareOnAndOff : MonoBehaviour
    {
        public bool ClemsGone;
        public GameObject door;


        private void Update()
        {
            if (ClemsGone)
            {
               // door.SetActive(false);
            }
        }
        public void OnTriggerEnter(Collider other)
        {
            door.SetActive(false);  
        }

        public void OnTriggerExit(Collider other)
        {
            if (!ClemsGone)
            {
                door.SetActive(true);
            }
           
        }
    }
}
