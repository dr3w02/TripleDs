using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class FlashLight : MonoBehaviour
    {

        [SerializeField] GameObject flashLightLight;
        [SerializeField] GameObject FlashLightObj;

        private bool FlashLightActive = false;

        // Start is called before the first frame update
        void Start()
        {
            flashLightLight.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.F)) 
            {
                if (FlashLightActive == false) 
                {
                    flashLightLight.gameObject.SetActive(true);
                    
                    FlashLightActive = true;
                
                }

                else
                {
                    flashLightLight.gameObject.SetActive(false);
                    FlashLightActive = false;
                }
            
            }
        }
    }
}
