using Unity.VisualScripting;
using UnityEngine;

namespace Platformer
{

    public abstract partial class EnemyBaseState : IState
    {
        protected readonly NurseCodeOffice enemy;
        protected readonly Animator animator;

        //put all the animations here 
        protected static readonly int IdelHash = Animator.StringToHash(name: "IdelNormal");

        protected static readonly int RunHash = Animator.StringToHash(name: "RunFWD");

        protected static readonly int WalkHash = Animator.StringToHash(name: "WalkFWD");

        protected static readonly int AttackHash = Animator.StringToHash(name: "Attack");

        protected static readonly int DieHash = Animator.StringToHash(name: "Die");

        //finish animations here 


        protected const float crossFadeDuration = 0.1f;

        protected EnemyBaseState(NurseCodeOffice enemy, Animator animator)
        {
            this.enemy = enemy;
            this.animator = animator;
            Debug.Log("Base");
            
        }

            

        

        public virtual void OnEnter()
        {
            //noop
        }
        public virtual void Update()
        {
            //noop
        }
        public virtual void FixedUpdate()
        {
            //noop
        }
        public virtual void OnExit()
        {
            //noop
        }

        public void OnStateEnter(Animator animator)
        {

        }
    }


    //9.40SEC Easyunity enemy AI using a stateMachine
}

