using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Platformer
{
    public class FlashLightPickUp : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _prompt;
        public GameObject pickedUpFlashLight;
        public GameObject inHandFlashLight;
        public bool interactable;
        public string InteractionPrompt => _prompt;


        public bool Interact(Interactor interactor)
        {
            Debug.Log("FlashLight");

            if (interactable == false)
            {
                pickedUpFlashLight.gameObject.SetActive(true);
               // inHandFlashLight.gameObject.SetActive(false);
            }

            else if (interactable == true)
            {
                pickedUpFlashLight.gameObject.SetActive(false);
                //inHandFlashLight.gameObject.SetActive(true);
                //interactable = false;
            }

            return true;
        }
    }
}
