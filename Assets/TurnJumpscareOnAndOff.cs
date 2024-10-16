using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class TurnJumpscareOnAndOff : MonoBehaviour
    {
        public GameObject door;
        public void OnTriggerEnter(Collider other)
        {
            door.SetActive(false);  
        }

        public void OnTriggerExit(Collider other)
        {
            door.SetActive(true);
        }
    }
}
