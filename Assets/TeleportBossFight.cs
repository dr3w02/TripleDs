using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UIElements;
using System.Numerics;

namespace Platformer
{
    public class TeleportBossFight : MonoBehaviour
    {

        public GameObject TeleportIn;
        public GameObject TeleportOut;

        public bool teleport;

        public GameObject player;
        public RBController playerScript;
        // Start is called before the first frame update
        void Start()
        {
            TeleportIn.SetActive(true);
            TeleportOut.SetActive(false);
        }

        public void OnTriggerEnter(Collider other)
        {
            teleport = !teleport;

            playerScript.TurnOffMovement();

            if (teleport == true)
            {
                player.transform.position = TeleportIn.transform.position;
                playerScript.Enabled();
            }

            else if (teleport == false)
            {
                player.transform.position = TeleportOut.transform.position;
                playerScript.Enabled();
            }
          
    
            


        }


    }
}
