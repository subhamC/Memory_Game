using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : GenericSingletonClass<StateController>
{
   
    public List<GameStateStep> StateDB = new List<GameStateStep>();
    private IState _currentState;
    public StartGameState startGame = new StartGameState();
    public GamePlayState gamePlayState = new GamePlayState();
    public PostGameState postGameState = new PostGameState();


    private void Start()
    {
        ChangeState(startGame);
    }

    void Update()
    {
        if (_currentState != null)
        {
            _currentState.UpdateState(this);
        }
    }

    public void ChangeState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GamePlay: ChangeState(gamePlayState); break;
            case GameState.PostGame: ChangeState(postGameState); break;
            case GameState.StartGame: ChangeState(startGame); break;
            default:break;
        }

    }

    

    private void ChangeState(IState newState)
    {
        if (_currentState != null)
        {
            _currentState.OnExit(this);
        }
        _currentState = newState;
        _currentState.OnEnter(this);

    }

    public GameObject EnableStateObject(GameState state)
    {
        foreach (var stateItem in StateDB)
        {
            if(stateItem.state == state)
            {
                return stateItem.StateObject;
            }

        }

        return null;
             
    }
}

