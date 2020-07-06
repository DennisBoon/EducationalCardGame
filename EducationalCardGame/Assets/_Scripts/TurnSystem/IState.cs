using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface which holds all the available methodes that can be used in every state.
public interface IState
{
    void Enter();
    void Execute();
    void Exit();
}
