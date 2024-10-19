using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class ForcedCrouch : MonoBehaviour
    {
        public RBController playerScript;
        public GameObject PlayerAnim;
        public GameObject Player;
        public void OnTriggerEnter(Collider other)
        {
            playerScript.StartCrouch();
        }
        public void OnTriggerStay(Collider other)
        {
            playerScript.StartCrouch();
        }

        public void OnTriggerExit(Collider other)
        {
            playerScript.StopCrouch();
            PlayerAnim.transform.position = Player.transform.position;

        }
    }
}
