using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartGameManager : MonoBehaviour
{
    // =========================
    // ■ UI
    // =========================

    [Header("UI")]
    [SerializeField] private GameObject startPanel;   // スタート画面
    [SerializeField] private Text countdownText;     // カウントダウン表示

    // =========================
    // ■ ゲーム制御
    // =========================

    [Header("ゲーム制御")]
    [SerializeField] private TargetSpawner targetSpawner;
    [SerializeField] private TimerController timer;

    // =========================
    // ■ スタートボタンから呼ばれる
    // =========================

    public void StartGame()
    {
        if (startPanel != null)
            startPanel.SetActive(false);

        StartCoroutine(CountdownRoutine());
    }

    // =========================
    // ■ カウントダウン処理
    // =========================

    IEnumerator CountdownRoutine()
    {
        if (countdownText == null)
        {
            Debug.LogError("StartGameManager：countdownText が未設定です！");
            yield break;
        }

        countdownText.gameObject.SetActive(true);

        for (int count = 3; count > 0; count--)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "START!!";
        yield return new WaitForSeconds(0.8f);

        countdownText.gameObject.SetActive(false);

        Debug.Log("★ カウントダウン終了：ゲーム開始");

        StartGameCore();
    }

    // =========================
    // ■ 本当のゲーム開始処理
    // =========================

    void StartGameCore()
    {
        if (targetSpawner != null)
        {
            targetSpawner.StartGame();   // ✅ GetComponent 不要
        }
        else
        {
            Debug.LogError("StartGameManager：TargetSpawner が未設定です！");
        }

        if (timer != null)
        {
            timer.StartTimer();
        }
        else
        {
            Debug.LogError("StartGameManager：TimerController が未設定です！");
        }
    }
}
