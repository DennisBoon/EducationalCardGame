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
        /**
         * In this state there is room for placing objects in the scenery based on decisions
         * made by the players. For example if players decide to invest in robot technology
         * after the turn has ended these robots could be instantiated in the scene for the
         * players to see.
         */
        owner.BeginTurn();
    }

    // Method which activates when the Enter method has been run through.
    public void Execute()
    {

    }

    // Method which activates when the state is exited.
    public void Exit()
    {
        Debug.Log("EXITING NEXT TURN STATE");
    }
}
