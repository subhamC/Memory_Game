using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New_Card" , menuName = "Cards_Menu/Create")]
public class CardScriptableObject : ScriptableObject
{
    public CardType type;
    public Sprite artwork;
   
}

