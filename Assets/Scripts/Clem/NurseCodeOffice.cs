using KBCore.Refs;
using System;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


namespace Platformer
{
    [RequireComponent(typeof(NavMeshAgent))]

    public class NurseCodeOffice : MonoBehaviour
    {

        [SerializeField, Self] NavMeshAgent agent;
        [SerializeField, Child] Animator animator;

        StateMachine stateMachine;

        void OnValidate() => this.ValidateRefs();

        void Start()
        {
            stateMachine = new StateMachine();
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

