using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostGameMenuController : MonoBehaviour
{
    [SerializeField] private Text _postGameStatus;
    [SerializeField] private Text _scroreText;
    [SerializeField] private Text _overallScoreText;
    [SerializeField] private Button NavToHome;

    private void OnEnable()
    {
        _postGameStatus.text = DataManager.Instance.GetReason();
        _scroreText.text = "Current Score: " +DataManager.Instance.GetLastPlayedScore().ToString();
        _overallScoreText.text = "OverAll Score: " +  DataManager.Instance.GetOverAllScore().ToString();
        NavToHome.onClick.AddListener(() => GotoHome());

    }

    private void OnDisable()
    {
        NavToHome.onClick.RemoveAllListeners();
    }
    private void GotoHome()
    {
        AudioPlayer.Instance.PlayAudio(AudioPlayer.ButtonAudio);
        StateController.Instance.ChangeState(GameState.StartGame);
    }
}
