using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class FlickeringLight : MonoBehaviour
    {
        [SerializeField] GameObject flashLightLight;
        public bool isFlickering = false;
        public float timeDelay;

        public void Start()
        {
            if (isFlickering == false)
            {
                StartFlicker();
            }
        }
        void StartFlicker()
        {
         
             StartCoroutine(LightFlicker());

          

            

        }

        IEnumerator LightFlicker()
        {
          
            isFlickering = true;

            flashLightLight.gameObject.SetActive(false);

            timeDelay = Random.Range(0.2f, 1f);

            yield return new WaitForSeconds(timeDelay);

            flashLightLight.gameObject.SetActive(true);

            timeDelay = Random.Range(0.2f, 1f);

            yield return new WaitForSeconds(timeDelay);

            isFlickering = false;
        }
    }
}
