using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class TeleportToPointLibary : MonoBehaviour
    {
        public GameObject player;
        public GameObject PointAfterClimb;

        public void OnTriggerEnter(Collider other)
        {
            player.transform.position = PointAfterClimb.transform.position;
        }
    }
}
