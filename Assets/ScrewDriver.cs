using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Platformer
{
    public class ScrewDriver : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _prompt;

        public GameObject pickedUpScrew;
        public GameObject holdingScrew;

        public bool hasScrew;

        public string InteractionPrompt => _prompt;

        public bool Interact(Interactor interactor)
        {
            Debug.Log("ScrewDriver");

            hasScrew = true;

            if (hasScrew == true)
            {
                pickedUpScrew.gameObject.SetActive(false);
                holdingScrew.gameObject.SetActive(true);
            }

            else
            {
                pickedUpScrew.gameObject.SetActive(true);
                holdingScrew.gameObject.SetActive(false);
                hasScrew = false;
            }


            return true;
        }
     
    }
}
