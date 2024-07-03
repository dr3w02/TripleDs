using Unity.VisualScripting;
using UnityEngine;

namespace Platformer
{

    public abstract partial class EnemyBaseState : IState
    {
        protected readonly NurseCodeOffice enemy;
        protected readonly Animator animator;

        //put all the animations here 
        protected static readonly int IdelHash = Animator.StringToHash(name: "");

        protected static readonly int RunHash = Animator.StringToHash(name: "");

        protected static readonly int WalkHash = Animator.StringToHash(name: "");

        protected static readonly int AttackHash = Animator.StringToHash(name: "");

        protected static readonly int DieHash = Animator.StringToHash(name: "");

        //finish animations here 


        protected const float crossFadeDuration = 0.1f;

        protected EnemyBaseState(NurseCodeOffice enemy, Animator animator)
        {
            this.enemy = enemy;
            this.animator = animator;
            
        }

            

        

        public virtual void OnEnter()
        {

        }
        public virtual void Update()
        {

        }
        public virtual void FixedUpdate()
        {

        }
        public virtual void OnExit()
        {

        }
    }


    //9.40SEC Easyunity enemy AI using a stateMachine
}

