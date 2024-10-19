using Cinemachine;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


namespace Platformer
{
    [RequireComponent(typeof (NavMeshAgent))]
    [RequireComponent(typeof (PlayerDetector))]

    public class NurseCodeOffice : Entity
    {
        [SerializeField, Self] NavMeshAgent agent;
        [SerializeField, Self] PlayerDetector playerDetector;
        [SerializeField, Child] Animator animator;
        public RBController characterMain;

        
       
        public FinalBossAnim ResetBossFight;

        [SerializeField] float wanderRadius = 10f; // changes how far enime is able to wander 

        [SerializeField] float timeBetweenAttacks = 1f;

        [SerializeField] public CanvasGroup myUIGroup;

        //[SerializeField] float speed;
        public GameObject clemDeathCam;

        public GameObject BlackBeackKillCam;
    

        public GameObject mCharacter;

        StateMachine stateMachine;

        CountdownTimer attackTimer;

        public Respawn respawn;

        public float speed;

        public bool smashed;
        
        /// <summary>
        /// //Main Scripts
        /// </summary>
        /// 



        void OnValidate() => this.ValidateRefs();



        void Start()
        {
            attackTimer = new CountdownTimer(timeBetweenAttacks);

            stateMachine = new StateMachine();

            var wanderState = new EnemyWanderState(this, animator, agent, wanderRadius);
            var chaseState = new EnemyChaseState(this, animator, agent, playerDetector.Player);
            var attackState = new EnemyAttackState(this, animator, agent, playerDetector.Player);


           
            At(wanderState, chaseState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
            At(chaseState, wanderState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
            At(chaseState, attackState, new FuncPredicate(() => playerDetector.CanAttackPlayer()));
            At(attackState, chaseState, new FuncPredicate(() => !playerDetector.CanAttackPlayer()));

            //Debug.Log("Wonder and chase");


            stateMachine.SetState(wanderState);

           



            clemDeathCam.SetActive(false);
          
            BlackBeackKillCam.SetActive(false);


        }


     


        void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);





        void Update()
        {

            stateMachine.Update();

            attackTimer.Tick(Time.deltaTime);

            //attackTimer = new CountdownTimer(timeBetweenAttacks);
            

            //Tried to make the animator speed fit with the speed of my gameobject
            //float speed = navMeshAgent.velocity.magnitude / navMeshAgent.speed;
            //animator.SetFloat("Speed", speed);

        }


  
       
            

        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
            

        }
        

        public void Attack()
        {
            if (attackTimer.IsRunning) return;

            attackTimer.Start();
            Debug.Log("Attacking");

        }


    }



}

