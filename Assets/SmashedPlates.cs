using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class SmashedPlates : MonoBehaviour
    {
        public AudioSource Smash;
        public GameObject Plates;
      
        [SerializeField] public Animator PlatesAnim;


   

        private void Start()
        {

            PlatesAnim.SetBool("Smash", false);

           
        }

        private void OnTriggerEnter(Collider other)
        {

            Debug.Log("in trigger");
            if (other.CompareTag("Player"))
            {

                PlatesAnim.SetBool("Smash", true);


                Smash.Play();



                Destroy(gameObject);

            }

        }
       
    }
}
