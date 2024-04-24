using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataBase", menuName = "Cards_Menu/DataBase")]
public class CardDatabaseScriptableObject : ScriptableObject
{
    public Sprite CardBack;
    public List<CardScriptableObject> CardsData = new List<CardScriptableObject>();
    [Range(2, 10)]
    public int Rows ;
    [Range(2, 10)]
    public int Colums ;
    public CardScriptableObject GetCard()
    {
        int value = Random.Range(0, CardsData.Count - 1);
        return CardsData[value];
    }

    public Sprite GetBackFace()
    {
        return CardBack;
    }
}
