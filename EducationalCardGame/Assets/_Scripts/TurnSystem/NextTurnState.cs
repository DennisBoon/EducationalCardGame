using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTurnState : IState
{
    // Inits TurnManager class.
    TurnManager owner;
    public NextTurnState(TurnManager owner) { this.owner = owner; }

    // Method which activates when the state is entered.
    public void Enter()
    {
        Debug.Log("ENTERING NEXT TURN STATE");
    }

    // Method which activates when the Enter method has been run through.
    public void Execute()
    {
        // Temporary way to return to CurrentTurnState
        if (Input.GetKeyDown(KeyCode.Space))
        {
           owner.BeginTurn();
        }
    }

    // Method which activates when the state is exited.
    public void Exit()
    {
        Debug.Log("EXITING NEXT TURN STATE");
    }
}
