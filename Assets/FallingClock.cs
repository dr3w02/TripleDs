using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class FallingClock : MonoBehaviour
    {
        public class FileCabinetFall : MonoBehaviour
        {

            public AudioSource FallingClock;
            public AudioSource GlassSmash;


            [SerializeField] public Animator ClockAnim;


            private void Start()
            {

                ClockAnim.SetBool("FallingClock", false);


            }

            private void OnTriggerExit(Collider other)
            {

                Debug.Log("in trigger");
                if (other.CompareTag("Player"))
                {

                    ClockAnim.SetBool("FallingClock", true);


                    FallingClock.Play();


                    GlassSmash.Play();


                    Destroy(gameObject);
                }

            }
        }
    }
}
