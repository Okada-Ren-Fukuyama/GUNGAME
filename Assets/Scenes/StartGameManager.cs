using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameManager : MonoBehaviour
{
    public GameObject startPanel;       // StartPanel
    public GameObject targetSpawner;    // TargetSpawner
    public TimerController timer;       // TimerController
    public Text countdownText;

    public void StartGame()
    {
        startPanel.SetActive(false);     // スタート画面を消す
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        countdownText.gameObject.SetActive(true);

        int count = 3;

        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }

        // 最後に "START!!"
        countdownText.text = "START!!";
        yield return new WaitForSeconds(0.8f);

        countdownText.gameObject.SetActive(false);

        // ---- カウントダウン後にゲーム開始 ----
        targetSpawner.SetActive(true);  // 的スポーン開始
        timer.StartTimer();             // タイマー開始
    }
}
