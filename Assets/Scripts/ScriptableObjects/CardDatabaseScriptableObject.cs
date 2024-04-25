using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataBase", menuName = "Cards_Menu/DataBase")]
public class CardDatabaseScriptableObject : ScriptableObject
{
    public Sprite CardBack;
    public List<CardScriptableObject> CardsData = new List<CardScriptableObject>();
    
    public CardScriptableObject GetCard()
    {
        int value = Random.Range(0, CardsData.Count - 1);
        return CardsData[value];
    }
    public CardScriptableObject GetCard(CardType type)
    {
        for (int i = 0; i < CardsData.Count; i++)
        {
            if(CardsData[i].type == type)
                return CardsData[i];
        }
        return null;
    }

    public Sprite GetBackFace()
    {
        return CardBack;
    }
}
