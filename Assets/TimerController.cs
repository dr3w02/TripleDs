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
        public OrphanWaypointFollow orphan1;
        public OrphanWaypointFollow orphan2;
        public OrphanWaypointFollow orphan3;
        public OrphanWaypointFollow orphan4;
        
        
    

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
                orphan1.Sleeping();
                orphan2.Sleeping();
                orphan3.Sleeping();
                orphan4.Sleeping();

                orphan.MusicPlay = true;
                orphan1.MusicPlay = true;
                orphan2.MusicPlay = true;
                orphan3.MusicPlay = true;
                orphan4.MusicPlay = true;


            }

            else if (timeRemaining <= 0)
            {
                //this is where i add the children will attack becuse music box is out 
                orphan.Running();
                orphan1.Running();
                orphan2.Running();
                orphan3.Running();
                orphan4.Running();


                orphan.MusicPlay = false;
                orphan1.MusicPlay = false;
                orphan2.MusicPlay = false;
                orphan3.MusicPlay = false;
                orphan4.MusicPlay = false;

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


