using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;


namespace Platformer
{
    [RequireComponent(typeof (NavMeshAgent))]
    [RequireComponent(typeof (PlayerDetector))]

    public partial class NurseCodeOffice : MonoBehaviour///find out what this is meant to be called
    {
        [SerializeField, Self] NavMeshAgent agent;
        [SerializeField, Self] PlayerDetector playerDetector;
        [SerializeField, Child] Animator animator;

        StateMachine stateMachine;
        [SerializeField] float wanderRadius = 10f; // changes how far enime is able to wander 

        void OnValidate() => this.ValidateRefs();

        void Start()
        {
            stateMachine = new StateMachine();

            var wanderState = new EnemyWanderState(this, animator, agent, wanderRadius);//this is enemy, 5f is wander radius
                                                                                        //
            var chaseState = new EnemyChaseState(this, animator, agent, playerDetector.Player);

            Any(wanderState, new FuncPredicate (() => true)); // for testing PURPOSES setting it to always true!!!!!!!!!!!!!!!


            At(wanderState, chaseState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
            At(chaseState, wanderState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
            Debug.Log("Wonder and chase");


            stateMachine.SetState(wanderState);



        }


        void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, IPredicate condition) => stateMachine.AddAnyTranstion(to, condition);



        void Update()
        {
            stateMachine.Update();
        }


        void FixedUpdate()
        {
            {
                stateMachine.FixedUpdate();
            }
        }

    }

}

