using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private Button _startBtn;
    [SerializeField]
    private Button _LoadBtn;
    [SerializeField]
    private Button _quitBtn;
    [SerializeField]
    private Slider _rowSlider;
    [SerializeField]
    private Slider _colSlider;
    [SerializeField]
    private Text _rowText;
    [SerializeField]
    private Text _colText;
    private LevelData data;
    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        RemoveListeneres();
    }

    private void RemoveListeneres()
    {

        _startBtn.onClick.RemoveAllListeners();
        _quitBtn.onClick.RemoveAllListeners();
        _rowSlider.onValueChanged.RemoveAllListeners();
        _colSlider.onValueChanged.RemoveAllListeners(); ;
    }

    public void Initialize()
    {
        _startBtn.onClick.AddListener(() => StartGame(_rowSlider.value, _colSlider.value));
        _rowSlider.onValueChanged.AddListener(delegate { UpdateRowText(); });
        _colSlider.onValueChanged.AddListener(delegate { UpdateColText(); });
        AudioPlayer.Instance.PlayThemeAudio();
        _quitBtn.onClick.AddListener(() => QuitGame());
         data = DataManager.Instance.GetLevelData();
        if(data != null)
        {
            _LoadBtn.onClick.AddListener(() => StartGame(data.LevelRow,data.LevelCol,data));
            _LoadBtn.interactable = true;
        }
        else
        {
            _LoadBtn.interactable = false;
        }

    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void UpdateRowText()
    {
        _rowText.text = _rowSlider.value.ToString();
    }

    private void UpdateColText()
    {
        _colText.text = _colSlider.value.ToString();
    }

    private void StartGame(float row, float col ,LevelData data = null)
    {
        AudioPlayer.Instance.PlayAudio(AudioPlayer.ButtonAudio);
        StateController.Instance.ChangeState(GameState.GamePlay);
        if(data !=null)
        {
            Events.instance.Raise(new GridSizeGameEvent(row,col,data.CardKeyValues,data.RemainingLives,data.Score));

        }
        else
        {
            Events.instance.Raise(new GridSizeGameEvent(row, col, null));
        }
    }
}
