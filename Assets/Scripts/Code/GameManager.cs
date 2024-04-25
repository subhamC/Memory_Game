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
    private int gridTotalSize = 5;
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
    private int _selectedRowsConfig=0 , _selectedColConfig=0;
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
        Events.instance.RemoveListener<CardSelectionGameEvent>(ListenerRemoved);
        Events.instance.RemoveListener<GridSizeGameEvent>(ListenerRemoved);
    }

    private void ListenerRemoved(GameEvent e)
    {
        Debug.Log(e.ToString() + "listener removed");
    }

    private void GameInitialise()
    {
        Events.instance.AddListener<CardSelectionGameEvent>(SelectCard);
        Events.instance.AddListener<GridSizeGameEvent>(SetGridConfig);

        StartCoroutine(DelayedGameSetup());
    }

    private void SetGridConfig(GridSizeGameEvent e)
    {
        _selectedRowsConfig = (int)e.rows;
        _selectedColConfig = (int)e.cols;
    }
    private IEnumerator DelayedGameSetup()
    {
        yield return new WaitUntil(() => _selectedRowsConfig != 0);
        gridTotalSize = (_selectedRowsConfig * _selectedColConfig);
        GamePanelSetUp();
        AllocationOfSpritesToCards();
        StartCoroutine(FlipCard());
        UpdateLifes(_noOfRetries);
        UpdateScore(_currentScore);
    }
    private void GamePanelSetUp()
    {
        // if total grid size is odd, we should have 1 card less
        int _hasBlankCard = gridTotalSize % 2;
        // remove all gameobject from parent
        GameReset();
        DataManager.Instance.ResetCurrentScore();
        // calculate position between each card & start position of each card based on the Panel
        RectTransform _panelSize = _cardPanel.transform.GetComponent<RectTransform>();
        float _panelRowSize = _panelSize.sizeDelta.x;
        float _panelColSize = _panelSize.sizeDelta.y;
        float _cardScaleX = 1.0f / _selectedRowsConfig;
        float _cardScaleY = 1.0f / _selectedColConfig;
        float _cardWidth = _panelRowSize / _selectedRowsConfig;
        float _cardHeight = _panelColSize / _selectedColConfig;
        float _cardPositionX = -_cardWidth * (float)(_selectedRowsConfig / 2);
        float _cardPositionY = -_cardHeight * (float)(_selectedColConfig / 2);

        if(_selectedRowsConfig % 2 == 0)
        {
         _cardPositionX += _cardWidth / 2;
        }

        if(_selectedColConfig % 2 == 0)
        {
             _cardPositionY += _cardHeight / 2;
        }

        float initialX = _cardPositionX;
        // for each in y-axis
        for (int i = 0; i < _selectedColConfig; i++)
        {
            _cardPositionX = initialX;
            // for each in x-axis
            for (int j = 0; j < _selectedRowsConfig; j++)
            {
                GameObject _dummyCard;
                // if is the last card and game is odd, we instead move the middle card on the panel to last spot
                if (_hasBlankCard == 1 && i == (_selectedColConfig - 1) && j == (_selectedRowsConfig - 1))
                {
                    int index = _selectedColConfig / 2 * _selectedColConfig + _selectedRowsConfig / 2;
                    _dummyCard = cards[index].gameObject;
                }
                else
                {
                    // create card prefab
                    _dummyCard = Instantiate(_cardPrefab);
                    // assign parent
                    _dummyCard.transform.SetParent(_cardParent);
                    Card currentCard = _dummyCard.GetComponent<Card>();
                    cards.Add(currentCard);
                    currentCard.InitializeCardProperties(_database.GetBackFace());
                    // modify its size
                    _dummyCard.transform.localScale = new Vector3(_cardScaleX, _cardScaleY);
                }
                // assign location
                _dummyCard.transform.localPosition = new Vector3(_cardPositionX, _cardPositionY, 0);
                _cardPositionX += _cardWidth;
            }
            _cardPositionY += _cardHeight;
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
    private void AllocationOfSpritesToCards()
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

    IEnumerator FlipCard()
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
            CheckGameWinCondition();
        }
    }

    private void CheckGameWinCondition()
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
        AudioPlayer.Instance.PlayAudio(AudioPlayer.EndGame);
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
