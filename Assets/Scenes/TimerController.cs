using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    // =========================
    // ■ 設定
    // =========================

    [Header("時間設定")]
    [SerializeField] private float timeLimit = 60.0f;

    // =========================
    // ■ UI
    // =========================

    [Header("UI")]
    [SerializeField] private Text timerText;
    [SerializeField] private GameObject timeUpPanel;
    [SerializeField] private Text finalScoreText;

    // =========================
    // ■ 内部状態
    // =========================

    private float currentTime;
    private bool isTimeUp = false;

    [Header("実行状態")]
    [SerializeField] private bool isRunning = false;

    // =========================
    // ■ 初期化
    // =========================

    void Start()
    {
        ResetTimer();

        if (timeUpPanel != null)
            timeUpPanel.SetActive(false);

        isRunning = false;
    }

    // =========================
    // ■ 更新処理
    // =========================

    void Update()
    {
        if (!isRunning || isTimeUp) return;

        currentTime -= Time.deltaTime;
        UpdateTimerDisplay();

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isTimeUp = true;
            isRunning = false;

            TimeUpAction();
        }
    }

    // =========================
    // ■ 表示更新
    // =========================

    void UpdateTimerDisplay()
    {
        if (timerText == null) return;

        int seconds = Mathf.FloorToInt(currentTime);
        timerText.text = "残り時間: " + seconds.ToString("D2");
    }

    // =========================
    // ■ タイムアップ処理
    // =========================

    void TimeUpAction()
    {
        Debug.Log("制限時間終了！ゲームオーバー");

        if (timeUpPanel != null)
            timeUpPanel.SetActive(true);

        ScoreManager sm = FindObjectOfType<ScoreManager>();
        if (finalScoreText != null && sm != null)
        {
            finalScoreText.text = "あなたのスコア：" + sm.GetScore();
        }
    }

    // =========================
    // ■ 制御系API（外部から呼ばれる）
    // =========================

    public void StartTimer()
    {
        ResetTimer();
        isRunning = true;
        isTimeUp = false;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        currentTime = timeLimit;
        UpdateTimerDisplay();
    }

    // =========================
    // ■ 時間操作
    // =========================

    public void AddTime(float amount)
    {
        currentTime += amount;
        Debug.Log($"時間加算 +{amount}秒");
    }

    public void ReduceTime(float amount)
    {
        currentTime -= amount;

        if (currentTime < 0f)
            currentTime = 0f;

        Debug.Log($"時間減少 -{amount}秒");
    }

    // =========================
    // ■ 状態取得
    // =========================

    public float GetRemainingTime()
    {
        return currentTime;
    }

    public bool IsTimeUp()
    {
        return isTimeUp;
    }

    // =========================
    // ■ UIボタン用
    // =========================

    public void Retry()
    {
        Debug.Log("再挑戦");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}