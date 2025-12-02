using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSpawner : MonoBehaviour
{
    [Header("的のPrefab")]
    public GameObject targetPrefab;

    [Header("的を出す範囲(UIのRectTransform)")]
    public RectTransform targetArea;

    [Header("生成間隔の最小/最大(秒)")]
    public float minSpawnInterval = 1.0f;
    public float maxSpawnInterval = 2.5f;

    [Header("一度に出す的の最小数")]
    public int minSpawnCount = 3;

    [Header("一度に出す的の最大数")]
    public int maxSpawnCount = 4;

    [Header("エリアの余白（左右・上下の調整）")]
    public float marginX = 100f;
    public float marginY = 80f;

    [Header("的の間隔（被り防止）")]
    public float minDistance = 120f;

    [Header("レア的の出現確率(0〜1)")]
    [Range(0f, 1f)] public float rareChance = 0.1f;

    [Header("マイナス的の出現確率(0〜1)")]
    [Range(0f, 1f)] public float badChance = 0.2f;

    [Header("時間経過で変化する調整値")]
    public float difficultyIncreaseRate = 1.0f; // 難易度上昇スピード倍率（大きいほど早く難しくなる）

    [Header("難易度アップ演出")]
    public Text difficultyUpText;
    private bool[] levelTriggered = new bool[3]; // 3段階（30%, 60%, 90%）で発火

    private List<GameObject> spawnedTargets = new List<GameObject>();
    private TimerController timer;

    void Start()
    {
        timer = FindObjectOfType<TimerController>();
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (timer != null && timer.IsTimeUp())
            {
                yield break; // このコルーチンを終了（スポーン停止）
            }

            SpawnTarget();

            float wait = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(wait);
        }
    }

    void SpawnTarget()
    {
        spawnedTargets.RemoveAll(t => t == null);

        int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 randomPos = GetSafeRandomPosition();
            if (randomPos == Vector2.zero) continue;

            GameObject target = Instantiate(targetPrefab, targetArea);
            target.GetComponent<RectTransform>().localPosition = randomPos;
            spawnedTargets.Add(target);

            // --- タイプ判定 ---
            float roll = Random.value;
            Image img = target.GetComponent<Image>();
            Target tg = target.GetComponent<Target>();

            if (img != null && tg != null)
            {
                if (roll < badChance)
                {
                    // 減点的（青）
                    img.color = Color.blue;
                    tg.targetType = TargetType.Bad;
                }
                else if (roll < badChance + rareChance)
                {
                    // レア的（赤）
                    img.color = Color.red;
                    tg.targetType = TargetType.Rare;
                }
                else if (roll < badChance + rareChance + 0.1f)
                {
                    // 時間減少的（水色）
                    img.color = Color.cyan;
                    tg.targetType = TargetType.TimeMinus;
                }
                else if (roll < badChance + rareChance + 0.2f)
                {
                    // 時間増加的（緑）
                    img.color = Color.green;
                    tg.targetType = TargetType.TimePlus;
                }
                else
                {
                    img.color = Color.gray;   // 通常的
                    tg.targetType = TargetType.Normal;
                }
            }
        }
    }

    Vector2 GetSafeRandomPosition()
    {
        // RectTransform のサイズ取得
        Vector2 areaSize = targetArea.rect.size;

        for (int i = 0; i < 20; i++)
        {
            // ① ローカル座標でランダム生成
            float x = Random.Range(-areaSize.x / 2 + marginX, areaSize.x / 2 - marginX);
            float y = Random.Range(-areaSize.y / 2 + marginY, areaSize.y / 2 - marginY);
            Vector2 localPos = new Vector2(x, y);

            // ② 既存ターゲットとの距離チェック（ローカル空間で行う）
            bool tooClose = false;

            foreach (var t in spawnedTargets)
            {
                if (t == null) continue;

                Vector2 tLocal = t.GetComponent<RectTransform>().localPosition;
                if (Vector2.Distance(localPos, tLocal) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
                return localPos;
        }

        return Vector2.zero;
    }

    void Update()
    {
        spawnedTargets.RemoveAll(t => t == null);
        if (timer == null) return;

        // ------ 秒数による経過 ------
        float remaining = timer.GetRemainingTime();
        float elapsed = timer.timeLimit - remaining;          // 経過時間（秒）
        elapsed = Mathf.Max(0, elapsed);                      // マイナス防止

        // ------ 時間経過で難易度を変化させる ------
        float progress = elapsed / timer.timeLimit;
        progress = Mathf.Clamp01(progress);

        minSpawnInterval = Mathf.Lerp(1.2f, 0.6f, progress * difficultyIncreaseRate);
        maxSpawnInterval = Mathf.Lerp(2.5f, 1.0f, progress * difficultyIncreaseRate);

        minSpawnCount = Mathf.RoundToInt(Mathf.Lerp(2, 4, progress * difficultyIncreaseRate));
        maxSpawnCount = Mathf.RoundToInt(Mathf.Lerp(5, 8, progress * difficultyIncreaseRate));

        rareChance = Mathf.Lerp(0.1f, 0.3f, progress * difficultyIncreaseRate);
        badChance = Mathf.Lerp(0.2f, 0.4f, progress * difficultyIncreaseRate);

        // ------ 難易度UP演出（秒でトリガー） ------
        if (elapsed >= 20f && !levelTriggered[0])
        {
            StartCoroutine(ShowDifficultyUpText("難易度UP!"));
            levelTriggered[0] = true;
        }
        else if (elapsed >= 40f && !levelTriggered[1])
        {
            StartCoroutine(ShowDifficultyUpText("さらに難易度UP!"));
            levelTriggered[1] = true;
        }
        else if (elapsed >= 50f && !levelTriggered[2])
        {
            StartCoroutine(ShowDifficultyUpText("最終段階!!"));
            levelTriggered[2] = true;
        }
    }

    IEnumerator ShowDifficultyUpText(string message)
    {
        if (difficultyUpText == null) yield break;

        difficultyUpText.text = message;
        difficultyUpText.gameObject.SetActive(true);
        difficultyUpText.color = new Color(1, 1, 0, 1); // 黄・不透明

        float fadeTime = 1.5f;
        float t = 0;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeTime);
            difficultyUpText.color = new Color(1, 1, 0, alpha);
            yield return null;
        }

        difficultyUpText.gameObject.SetActive(false);
    }
}





