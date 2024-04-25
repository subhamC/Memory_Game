using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameStateStep
{
    public GameState State;
    public GameObject StateObject;

}

[System.Serializable]
public class LevelData
{
    public int LevelRow,LevelCol;
    public int RemainingLives, Score;
    public Dictionary<int, CardType> CardKeyValues = new Dictionary<int, CardType>();
}

public class CardSelectionGameEvent : GameEvent
{
    public CardType type;
    public int cardID;

    public CardSelectionGameEvent(CardType card, int ID)
    {
        type = card;
        cardID = ID;
    }

}



public class GridSizeGameEvent : GameEvent
{
    public float cols;
    public float rows;
    public Dictionary<int, CardType> keyValues = new Dictionary<int, CardType>();
    public int RemainingLifes, Score;
  
    public GridSizeGameEvent(float row, float col, Dictionary<int, CardType> keyValues = null , int lifes = -1 , int score = -1)
    {
        cols = col;
        rows = row;
        this.keyValues = keyValues;
        RemainingLifes = lifes;
        Score = score;
    }

}

public enum CardType
{
    none,
    bull,
    camel,
    cat,
    cow,
    croc,
    dog,
    donkey,
    hippo,
    lion,
    monkey,
    mouse,
    pig,
    rabbit,
    rhino,
    sheep,
    turtle

}

public enum GameState
{
    StartGame,
    GamePlay,
    PostGame
}