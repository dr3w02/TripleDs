using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

namespace Platformer
{
    public class MusicBox : MonoBehaviour
    {
        [SerializeField] Image radialImage;
        public OrphanWaypointFollow orphans;
        public InputManager inputManager;
        public TimerController timer;
        private float fillSpeed = 5.0f;
        public float maxTime = 20.0f; // change here u gotta change the other one on timer
        void Update()
        {
           
            if (InputManager.Instance.GetHold())
            {
                timer.MusicBoxWindUp();
                radialImage.fillAmount += Time.deltaTime * fillSpeed;

            }

            else
            {

                timer.MusicBoxWindDown();
            }

            // Clamp fillAmount between 0 and 1
            radialImage.fillAmount = Mathf.Clamp01(radialImage.fillAmount);
        }


    }
}
