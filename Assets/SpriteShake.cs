using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class SpriteShake : MonoBehaviour
    {
        [SerializeField] public GameObject InteractableSprite;
        [SerializeField] private Animator InteractAnim;
        private Collider showGlassesCollider; 

        void Start()
        {
            showGlassesCollider = gameObject.GetComponent<Collider>();
            InteractableSprite.SetActive(false);
            InteractAnim.SetBool("Glasses", false);
        }

        private void OnTriggerEnter(Collider other) 
        {
            Debug.Log("OnTriggerEnter called with: " + other.name);

            if (other.CompareTag("Player"))
            {
                InteractableSprite.SetActive(true);
                InteractAnim.SetBool("Glasses", true);
            }
        }

        private void OnTriggerStay(Collider other) 
        {
            if (other.CompareTag("Player"))
            {
                InteractableSprite.SetActive(true);
                InteractAnim.SetBool("Glasses", true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                InteractableSprite.SetActive(false);
                InteractAnim.SetBool("Glasses", false);
            }
        }
    }
}