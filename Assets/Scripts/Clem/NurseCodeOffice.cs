using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;


namespace Platformer
{
    [RequireComponent(typeof(NavMeshAgent))]

    public class NurseCodeOffice : MonoBehaviour///find out what this is meant to be called
    {

        [SerializeField, Self] NavMeshAgent agent;
        [SerializeField, Child] Animator animator;

        StateMachine stateMachine;

        void OnValidate() => this.ValidateRefs();

        void Start()
        {
            stateMachine = new StateMachine();

            var wanderState = new EnemyWanderState(this, animator, agent, 5f); //this is enemy, 5f is wander radius 

            Any(wanderState, new FuncPredicate (() => true)); // for testing PURPOSES setting it to always true!!!!!!!!!!!!!!!

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

