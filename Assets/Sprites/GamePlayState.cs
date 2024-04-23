using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayState :IState
{
    private GameState state = GameState.GamePlay;
    
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
