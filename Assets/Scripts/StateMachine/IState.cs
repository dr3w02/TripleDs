using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public interface IState
    {
        void OnEnter();
        void Update();
        void FixedUpdate();
        void OnExit();
    }




}
