using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UIElements;
using System.Numerics;
using static UnityEngine.InputSystem.Controls.AxisControl;
using UnityEditor.ShaderGraph.Internal;

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
        public float speedofPause;
      
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
                StartCoroutine(pauseBeforeMoving());
            }

            else if (teleport == false)
            {
                player.transform.position = TeleportOut.transform.position;
                playerScript.Enabled();
                StartCoroutine(pauseBeforeMoving());
            }





        }
        public IEnumerator pauseBeforeMoving()
        {

            yield return new WaitForSeconds(speedofPause);
            playerScript.Enabled();
        }
    }
}
