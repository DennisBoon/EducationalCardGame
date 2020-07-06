using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    // Inits IState interface
    IState currentState;

    // Method which is used to change the active state. Also manages when the Enter and Exit method of each state occurs.
    public void ChangeState(IState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter();
    }

    // Update method which is used for the Execute method of every state.
    public void Update()
    {
        if (currentState != null) currentState.Execute();
    }
}
