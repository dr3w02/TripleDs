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

        public float timeRemaining;
       

        public OrphanWaypointFollow orphan;
        public OrphanWaypointFollow orphan1;
        public OrphanWaypointFollow orphan2;
        public OrphanWaypointFollow orphan3;


        public MusicBox musicbox;
    

        public float maxTime;


        public MusicBoxMusic music;

     

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
              

                orphan.MusicPlay = true;
                orphan1.MusicPlay = true;
                orphan2.MusicPlay = true;
                orphan3.MusicPlay = true;
            


            }

            else if (timeRemaining <= 0)
            {
                //this is where i add the children will attack becuse music box is out 
                orphan.Running();
                orphan1.Running();
                orphan2.Running();
                orphan3.Running();
              


                orphan.MusicPlay = false;
                orphan1.MusicPlay = false;
                orphan2.MusicPlay = false;
                orphan3.MusicPlay = false;
               

                timeRemaining = 0;

               
            }

        }
        // Update is called once per frame
        public void MusicBoxWindDown()
        {
            music.audioDownPlay = false;
            timeRemaining -= Time.deltaTime;
            timerRadial.fillAmount = timeRemaining / maxTime;
        }

    
        public void MusicBoxWindUp()
        {
            music.audioDownPlay = true;

            timeRemaining += Time.deltaTime;

           if (timeRemaining > maxTime)
           {
               timeRemaining = maxTime;
           }


             timerRadial.fillAmount = timeRemaining / maxTime;
         
          
        }
    }
}


