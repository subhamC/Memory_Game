using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private Button _startBtn;
    [SerializeField]
    private Slider _rowSlider;
    [SerializeField]
    private Slider _colSlider;
    [SerializeField]
    private Text _rowText;
    [SerializeField]
    private Text _colText;
    private void OnEnable()
    {
        InitializeUI();
    }

    private void OnDisable()
    {
        RemoveListeneres();
    }

    private void RemoveListeneres()
    {

        _startBtn.onClick.RemoveAllListeners();
        _rowSlider.onValueChanged.RemoveAllListeners();
        _colSlider.onValueChanged.RemoveAllListeners(); ;
    }

    public void InitializeUI()
    {
        _startBtn.onClick.AddListener(() => StartGame());
        _rowSlider.onValueChanged.AddListener(delegate { UpdateRowText(); });
        _colSlider.onValueChanged.AddListener(delegate { UpdateColText(); });
        AudioPlayer.Instance.PlayThemeAudio();
    }

    private void UpdateRowText()
    {
        _rowText.text = _rowSlider.value.ToString();
    }

    private void UpdateColText()
    {
        _colText.text = _colSlider.value.ToString();
    }

    private void StartGame()
    {
        AudioPlayer.Instance.PlayAudio(AudioPlayer.ButtonAudio);
        StateController.Instance.ChangeState(GameState.GamePlay);
        Events.instance.Raise(new GridSizeGameEvent(_rowSlider.value, _colSlider.value));
    }
}
