using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer
{
    public class TimerController : MonoBehaviour
    {
        public UnityEngine.UI.Image timerRadial;

        float timeRemaining;
       

        public OrphanWaypointFollow orphan;

        public float maxTime = 20.0f;

    


        void Start()
        {
            timeRemaining = maxTime;

            MusicBoxWindDown();

        }

        public void Update()
        {

            if (timeRemaining > 0)
            {
                orphan.Sleeping();
                orphan.MusicPlay = true;

            }

            else if (timeRemaining <= 0)
            {
                //this is where i add the children will attack becuse music box is out 
                orphan.Running();
                orphan.MusicPlay = false;
                timeRemaining = 0;
            }

        }
        // Update is called once per frame
        public void MusicBoxWindDown()
        {
            
      
              timeRemaining -= Time.deltaTime;

             timerRadial.fillAmount = timeRemaining / maxTime;
            


        }

    
        public void MusicBoxWindUp()
        {
   
            timeRemaining += Time.deltaTime;

            if (timeRemaining > maxTime)
            {
               timeRemaining = maxTime;
            }


             timerRadial.fillAmount = timeRemaining / maxTime;
         
          
        }
    }
}


