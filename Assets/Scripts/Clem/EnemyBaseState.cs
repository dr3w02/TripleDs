using Unity.VisualScripting;
using UnityEngine;

namespace Platformer
{

    public abstract class EnemyBaseState : IState
    {
        protected readonly characterMovement player;
        protected readonly Animator animator;



        protected static readonly int LocomotionHash = Animator.StringToHash(name:"Locomotion");
        protected static readonly int JumpHash = Animator.StringToHash(name: "Jump");


        protected const float crossFadeDuration = 0.1f;

        protected EnemyBaseState(characterMovement player, Animator animator)
        {
            this.player = player;
            this.animator = animator;
            
        }

            

        protected readonly EnemyBaseState enemy;

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


    
}

