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
    [SerializeField] private GameObject infoPanel;

    // =========================
    // ■ ゲーム制御
    // =========================

    [Header("ゲーム制御")]
    [SerializeField] private TargetSpawner targetSpawner;
    [SerializeField] private TimerController timer;


    [Header("サウンド")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip countdownSE; // 3,2,1 の音
    [SerializeField] private AudioClip startSE;     // START!! の音
    // =========================
    // ■ スタートボタンから呼ばれる
    // =========================

    public void StartGame()
    {

        Debug.Log("▶ StartGameManager 実行中: " + gameObject.name);
        if (startPanel != null)
            startPanel.SetActive(false);

        StartCoroutine(CountdownRoutine());
    }

    public void OpenInfo()
    {
        if (infoPanel == null)
        {
            Debug.LogError("StartGameManager：infoPanel が Inspector で設定されていません！");
            return;
        }

        infoPanel.SetActive(true);
    }

    public void CloseInfo()
    {
        if (infoPanel == null)
        {
            Debug.LogError("StartGameManager：infoPanel が Inspector で設定されていません！");
            return;
        }

        infoPanel.SetActive(false);
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

            if (audioSource != null && countdownSE != null)
                audioSource.PlayOneShot(countdownSE);

            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "START!!";

        if (audioSource != null && startSE != null)
            audioSource.PlayOneShot(startSE);

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
