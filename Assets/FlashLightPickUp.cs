using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Platformer
{
    public class FlashLightPickUp : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _prompt;
        public GameObject InteractionImagePrompt => null;

        public GameObject pickedUpFlashLight;

        public GameObject inHandFlashLight;

        public bool hasLight;

        
        public string InteractionPrompt => _prompt;


        public bool Interact(Interactor interactor, GameObject player)
        {
            Debug.Log("FlashLight");

            hasLight = true;

            if (hasLight == true)
            {
               pickedUpFlashLight.gameObject.SetActive(false);
               inHandFlashLight.gameObject.SetActive(true);
            }

            else 
            {
                pickedUpFlashLight.gameObject.SetActive(true);
                inHandFlashLight.gameObject.SetActive(false);
                hasLight = false;
            }

            return true;
        }
    }
}
