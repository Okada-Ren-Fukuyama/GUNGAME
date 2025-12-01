using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameManager : MonoBehaviour
{
    public GameObject startPanel;      // StartPanel を入れる
    public GameObject targetSpawner;   // TargetSpawner を入れる
    public TimerController timer;      // TimerController を入れる

    void Start()
    {
        // ゲーム開始前はスポーンを止めておく（重要！）
        targetSpawner.SetActive(false);

        // タイマーも止めておく（必要なら）
        if (timer != null)
            timer.isRunning = false;
    }

    public void StartGame()
    {
        // スタート画面を非表示
        startPanel.SetActive(false);

        // スポーン開始
        targetSpawner.SetActive(true);

        // タイマー開始
        if (timer != null)
            timer.StartTimer();
    }
}
