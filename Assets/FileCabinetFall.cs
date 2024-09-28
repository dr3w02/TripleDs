using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class FileCabinetFall : MonoBehaviour
    {

        public AudioSource Boom;

        public AudioSource Giggle;

        [SerializeField] public Animator cabinetAnim;


        private void Start()
        {

            cabinetAnim.SetBool("Falling", false);


        }

        private void OnTriggerEnter(Collider other)
        {

            Debug.Log("in trigger");
            if (other.CompareTag("Player"))
            {

                cabinetAnim.SetBool("Falling", true);


                Boom.Play();


                Giggle.Play();

                Destroy(gameObject);
            }

        }
    }
}
