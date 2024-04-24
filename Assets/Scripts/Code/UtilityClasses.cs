using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameStateStep
{
    public GameState state;
    public GameObject StateObject;

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