using UnityEngine;
using UnityEngine.UI; // Text (Legacy) の場合
// using TMPro; // Text Mesh Pro の場合

public class TimerController : MonoBehaviour
{
    // 制限時間をInspectorから設定 (例: 120.0f で 2分)
    public float timeLimit = 60.0f;

    // 現在の残り時間
    private float currentTime;

    // 画面に時間を表示するためのUIコンポーネント
    public Text timerText; // TextMeshProの場合は public TMPro.TextMeshProUGUI timerText;

    private bool isTimeUp = false; // 時間切れフラグ

    void Start()
    {
        currentTime = timeLimit;
        UpdateTimerDisplay();
    }

    void Update()
    {
        // 時間切れなら更新処理をスキップ
        if (isTimeUp)
        {
            return;
        }

        // **Time.deltaTime**（前フレームからの経過時間）を使って正確に時間を減らす
        currentTime -= Time.deltaTime;

        UpdateTimerDisplay();

        // 残り時間が0以下になったら
        if (currentTime <= 0)
        {
            currentTime = 0;
            isTimeUp = true;
            TimeUpAction(); // 時間切れ時の処理を実行
        }
    }

    // UIの表示を更新する処理
    void UpdateTimerDisplay()
    {
        // 残り時間を秒単位の整数に変換し、"残り時間: 59" の形式で表示
        int seconds = Mathf.FloorToInt(currentTime);
        timerText.text = "残り時間: " + seconds.ToString("D2"); // D2 は2桁表示（例: 05）
    }

    // 時間切れ時の処理（ゲームオーバー、シーン移動など）
    void TimeUpAction()
    {
        Debug.Log("制限時間終了！ゲームオーバーです。");
        // 例: SceneManager.LoadScene("GameOverScene");
    }

    // ✅ 時間を増やす
    public void AddTime(float amount)
    {
        currentTime += amount;
        Debug.Log($"時間加算 +{amount}秒");
    }

    // ✅ 時間を減らす
    public void ReduceTime(float amount)
    {
        currentTime -= amount;
        if (currentTime < 0) currentTime = 0;
        Debug.Log($"時間減少 -{amount}秒");
    }
}