using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

namespace Platformer
{
    public abstract class Timer 
    {

        protected float initialTime;

        protected float Time { get; set; }


        public bool IsRunning {get; protected set;}

        public float Progress => Time / initialTime;

        public Action OnTimerStart = delegate { };

        public Action OnTimerStop = delegate { };

        protected Timer(float value)
        {
            initialTime = value;
            IsRunning = false;
        }

        public void Start()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                OnTimerStart.Invoke();

            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                OnTimerStop.Invoke();
            }
        }


        public abstract void Tick(float deltaTime);

    }

    //countdown Cooldown Timer


    public class CountdownTimer: Timer
    {
        public CountdownTimer(float value) : base(value)
        {

        }
        public override void Tick(float deltaTime)
        {
            if(IsRunning && Time > 0)
            {
                Time -= deltaTime;
            }

            if (IsRunning && Time <= 0)
            {
                Stop();
            }
        }

        public bool IsFinished => Time <= 0;

        public void Reset() => Time = initialTime;
        
           
       

    }
}
