using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTurnState : IState
{
    // Inits TurnManager class.
    TurnManager owner;
    public CurrentTurnState(TurnManager owner) { this.owner = owner; }

    // Method which activates when the state is entered.
    public void Enter()
    {
        Debug.Log("ENTERING CURRENT TURN STATE");
        owner.dilemmaManager.UpdateDilemma();
    }

    // Method which activates when the Enter method has been run through.
    public void Execute()
    {

    }

    // Method which activates when the state is exited.
    public void Exit()
    {
        Debug.Log("EXITING CURRENT TURN STATE");
    }
}