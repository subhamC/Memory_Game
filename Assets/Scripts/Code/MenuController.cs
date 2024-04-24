using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private Button _startBtn;

    private void OnEnable()
    {
        InitializeUI();
    }

    public void InitializeUI()
    {
        _startBtn.onClick.AddListener(() => StartGame());
    }

    private void StartGame()
    {
        StateController.Instance.ChangeState(GameState.GamePlay);
    }
}
