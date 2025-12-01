using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    public float timeLimit = 60.0f;   // 制限時間
    private float currentTime;

    public Text timerText;
    public GameObject timeUpPanel;
    public Text finalScoreText;

    private bool isTimeUp = false;

    // ★ 追加：ゲーム開始前かどうか
    public bool isRunning = false;

    void Start()
    {
        currentTime = timeLimit;
        UpdateTimerDisplay();

        if (timeUpPanel != null)
            timeUpPanel.SetActive(false);

        // ★ ゲーム開始前は動かさない
        isRunning = false;
    }

    void Update()
    {
        // ★ 動いていなければ時間減らさない
        if (!isRunning) return;

        // 時間切れなら止める
        if (isTimeUp)
            return;

        currentTime -= Time.deltaTime;

        UpdateTimerDisplay();

        if (currentTime <= 0)
        {
            currentTime = 0;
            isTimeUp = true;
            isRunning = false;   // ★ 追加：時間切れで停止
            TimeUpAction();
        }
    }

    void UpdateTimerDisplay()
    {
        int seconds = Mathf.FloorToInt(currentTime);
        timerText.text = "残り時間: " + seconds.ToString("D2");
    }

    void TimeUpAction()
    {
        Debug.Log("制限時間終了！ゲームオーバー");

        if (timeUpPanel != null)
        {
            timeUpPanel.SetActive(true);

            ScoreManager sm = FindObjectOfType<ScoreManager>();
            if (finalScoreText != null && sm != null)
            {
                finalScoreText.text = "あなたのスコア：" + sm.GetScore();
            }
        }
    }

    public void Retry()
    {
        Debug.Log("再挑戦");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddTime(float amount)
    {
        currentTime += amount;
        Debug.Log($"時間加算 +{amount}秒");
    }

    public void ReduceTime(float amount)
    {
        currentTime -= amount;
        if (currentTime < 0) currentTime = 0;
        Debug.Log($"時間減少 -{amount}秒");
    }

    public float GetRemainingTime()
    {
        return currentTime;
    }

    public bool IsTimeUp()
    {
        return isTimeUp;
    }

    // ★ 追加：ゲーム開始用
    public void StartTimer()
    {
        isRunning = true;
    }

    // ★ 追加：ゲーム停止用（任意）
    public void StopTimer()
    {
        isRunning = false;
    }
}