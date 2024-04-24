using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // list of card
    private List<Card> cards = new List<Card>();
    private int gameSize = 5;
    // parent object of cards
    [SerializeField]
    private Transform _cardParent;
    [SerializeField]
    private GameObject _cardPanel;
    [SerializeField]
    private GameObject _cardPrefab;
    private int _scoreToBeAdded = 10;
    private int _currentScore =0;
    [SerializeField] private Text LifesTxt , ScoreTxt;
    // Initialize cards, size, and position based on size of game
    [SerializeField]
    private CardDatabaseScriptableObject _database;
    private CardType _selectedCardType = CardType.none;
    private int _selectedCardID = -1;
    private int _noOfRetries = 5;
    private void OnEnable()
    {
        GameInitialise();
    }

    private void OnDisable()
    {
        GameReset();
    }

    private void GameReset()
    {
        foreach (Transform child in _cardParent)
        {
            Destroy(child.gameObject);
        }
        cards.Clear();
       
    }

    private void GameInitialise()
    {
        Events.instance.AddListener<CardSelectionGameEvent>(SelectCard);
        gameSize = (_database.Rows * _database.Colums);
        SetPanel();
        SpriteAllocation();
        StartCoroutine(HideFace());
        UpdateLifes(_noOfRetries);
        UpdateScore(_currentScore);
      
    }


    private void SetPanel()
    {
        // if game is odd, we should have 1 card less
        int isOdd = gameSize % 2;
        // remove all gameobject from parent
        GameReset();
        DataManager.Instance.ResetCurrentScore();
        // calculate position between each card & start position of each card based on the Panel
        RectTransform panelsize = _cardPanel.transform.GetComponent<RectTransform>();
        float row_size = panelsize.sizeDelta.x;
        float col_size = panelsize.sizeDelta.y;
        float scaleX = 1.0f / _database.Rows;
        float scaleY = 1.0f / _database.Colums;
        float xInc = row_size / _database.Rows;
        float yInc = col_size / _database.Colums;
        float curX = -xInc * (float)(_database.Rows / 2);
        float curY = -yInc * (float)(_database.Colums / 2);
        
        if (isOdd == 0)
        {
            if(_database.Rows == _database.Colums)
            {
                curX += xInc / 2;
                curY += yInc / 2;
            }
           else if(_database.Rows%2 == 0 && _database.Colums%2 == 0) 
            {
                if (_database.Rows > _database.Colums)
                {
                    curX += xInc / 2;
                    curY += yInc / (_database.Rows - _database.Colums);
                }
                else
                {
                    curX += xInc / (_database.Colums - _database.Rows);
                    curY += yInc / 2;
                }
            }
            else
            {
                if (_database.Rows > _database.Colums)
                {
                    curX += xInc / 2;
                    //curY += yInc / (_database.Rows - _database.Colums);
                }
                else
                {
                   // curX += xInc / (_database.Colums - _database.Rows);
                    curY += yInc / 2;
                }
            }
            
            
        }

        float initialX = curX;
        // for each in y-axis
        for (int i = 0; i < _database.Colums; i++)
        {
            curX = initialX;
            // for each in x-axis
            for (int j = 0; j < _database.Rows; j++)
            {
                GameObject c;
                // if is the last card and game is odd, we instead move the middle card on the panel to last spot
                if (isOdd == 1 && i == (_database.Colums - 1) && j == (_database.Rows - 1))
                {
                    int index = _database.Colums / 2 * _database.Colums + _database.Rows / 2;
                    c = cards[index].gameObject;
                }
                else
                {
                    // create card prefab
                    c = Instantiate(_cardPrefab);
                    // assign parent
                    c.transform.SetParent(_cardParent);

                    //int index = i * gameSize + j;
                    Card currentCard = c.GetComponent<Card>();
                    cards.Add(currentCard);
                    currentCard.InitializeCardProperties(_database.GetBackFace());
                    // modify its size
                    c.transform.localScale = new Vector3(scaleX, scaleY);
                }
                // assign location
                c.transform.localPosition = new Vector3(curX, curY, 0);
                curX += xInc;
            }
            curY += yInc;
        }
        ResetCardID();
    }

    private void ResetCardID()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].CardID = i;
        }
    }

    // Allocate pairs of sprite to card instances
    private void SpriteAllocation()
    {
        List<Card> tempCards = cards.ToList();
       while(tempCards.Count > 0) 
        {
            CardScriptableObject cardData = _database.GetCard();
            int value = Random.Range(0, tempCards.Count);
            tempCards[value].SetCardScriptableObject(cardData);
            tempCards[value].InitializeCardProperties(null);
            tempCards.RemoveAt(value);

             value = Random.Range(0, tempCards.Count);
            tempCards[value].SetCardScriptableObject(cardData);
            tempCards[value].InitializeCardProperties(null);
            tempCards.RemoveAt(value);

        }
    }

    IEnumerator HideFace()
    {
        //display for a short moment before flipping
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < cards.Count; i++)
            cards[i].Flip(null);
        yield return new WaitForSeconds(0.5f);
    }

    private void SelectCard(CardSelectionGameEvent e)
    {
        // first card selected
        if (_selectedCardType == CardType.none)
        {
            _selectedCardType = e.type;
            _selectedCardID = e.cardID;
        }
        else
        { // second card selected
            if (_selectedCardType == e.type)
            {
                //correctly matched
                cards[_selectedCardID].DisableCard();
                cards[e.cardID].DisableCard();

                var card1 =cards.ElementAt(_selectedCardID);
                var card2 = cards.ElementAt(e.cardID);

                cards.Remove(card1);
                cards.Remove(card2);
                ResetCardID();
                DataManager.Instance.AddToOverAllScore(_scoreToBeAdded);
                _currentScore += _scoreToBeAdded;
                UpdateScore(_currentScore);
            }
            else
            {

                // incorrectly matched
                cards[_selectedCardID].DelayedFlipBack();
                cards[e.cardID].DelayedFlipBack();
                _noOfRetries--;
                UpdateLifes(_noOfRetries);
            }
            _selectedCardType = CardType.none;
            _selectedCardID = -1;
            CheckGame();
        }
    }

    private void CheckGame()
    {
        // win game
        if (cards.Count <= 0)
        {
            EndGame("You Win");   
        }

        if(_noOfRetries <= 0)
        {
            EndGame("You Lose");
        }


    }

    private void EndGame(string reason)
    {
       
        DataManager.Instance.SetReason(reason);
        DataManager.Instance.SetLastPlayedScore(_currentScore);
        StateController.Instance.ChangeState(GameState.PostGame);
        // show game status
    }

    private void UpdateLifes(int Lifes)
    {
        LifesTxt.text = "LIFES :" + _noOfRetries;
    }

    private void UpdateScore(int Score)
    {
        ScoreTxt.text = "SCORE:" + _currentScore;
    }
}
