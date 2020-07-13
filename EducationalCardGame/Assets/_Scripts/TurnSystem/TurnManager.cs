using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    // State Machine.
    StateMachine stateMachine = new StateMachine();

    // Dilemma Manager.
    public DilemmaManager dilemmaManager;

    // Sets the state to enter at the start of the game.
    private void Awake()
    {
        stateMachine.ChangeState(new CurrentTurnState(this));
    }

    // Updates the state that's currently active. 
    private void Update()
    {
        stateMachine.Update();
    }

    // Method to go to the NextTurnState.
    public void NextTurn()
    {
        stateMachine.ChangeState(new NextTurnState(this));
    }

    // Method to go to the CurrentTurnState.
    public void BeginTurn()
    {
        stateMachine.ChangeState(new CurrentTurnState(this));
    }
}
