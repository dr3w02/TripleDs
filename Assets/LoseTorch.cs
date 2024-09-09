using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class LoseTorch : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject torch;

        private void OnTriggerEnter(Collider other)
        {
            torch.SetActive(false);
        }

        private void OnTriggerStay(Collider other)
        {
            torch.SetActive(false);
        }


        private void OnTriggerExit(Collider other)
        {
            torch.SetActive(false);
        }
    }
}
