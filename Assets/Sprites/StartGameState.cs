using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameState : IState
{
    private GameState state = GameState.StartGame;

    public void OnEnter(StateController controller)
    {
        controller.EnableStateObject(state).SetActive(true);
    }

    public void OnExit(StateController controller)
    {
        controller.EnableStateObject(state).SetActive(false);
    }

    public void UpdateState(StateController controller)
    {
      
    }
    
}
