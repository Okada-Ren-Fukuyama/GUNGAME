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



    IEnumerator ShowDifficultyUpText(string message)
    {
        Debug.Log("★★★ ShowDifficultyUpText 実行");

        if (difficultyUpText == null)
        {
            Debug.LogError("difficultyUpText が null！");
            yield break;
        }

        // 表示ON
        difficultyUpText.gameObject.SetActive(true);

        difficultyUpText.text = message;
        difficultyUpText.color = Color.white;

        Debug.Log("ActiveSelf: " + difficultyUpText.gameObject.activeSelf);
        Debug.Log("ActiveInHierarchy: " + difficultyUpText.gameObject.activeInHierarchy);

        // 5秒間表示
        yield return new WaitForSeconds(5f);

        difficultyUpText.gameObject.SetActive(false);
    }

    public void SetDifficultyByLevel(int level)
    {
        Debug.Log("★ SetDifficultyByLevel 呼ばれた！ level = " + level);

        if (level == 1)
        {
            minSpawnInterval = 1.2f;
            maxSpawnInterval = 2.5f;

            minSpawnCount = 2;
            maxSpawnCount = 4;

            rareChance = 0.1f;
            badChance = 0.2f;

        }
        else if (level == 2)
        {
            minSpawnInterval = 0.9f;
            maxSpawnInterval = 1.8f;

            minSpawnCount = 3;
            maxSpawnCount = 5;

            rareChance = 0.15f;
            badChance = 0.25f;

            StartCoroutine(ShowDifficultyUpText("LEVEL UP!"));
        }
        else if (level == 3)
        {
            minSpawnInterval = 0.6f;
            maxSpawnInterval = 1.2f;

            minSpawnCount = 4;
            maxSpawnCount = 7;

            rareChance = 0.2f;
            badChance = 0.3f;

            StartCoroutine(ShowDifficultyUpText("LEVEL UP!"));
        }
        else if (level == 4)
        {
            minSpawnInterval = 0.4f;
            maxSpawnInterval = 0.9f;

            minSpawnCount = 5;
            maxSpawnCount = 9;

            rareChance = 0.3f;
            badChance = 0.4f;

            StartCoroutine(ShowDifficultyUpText("LEVEL UP!!!"));
        }
    }

    void Awake()
    {
        if (difficultyUpText != null)
            difficultyUpText.gameObject.SetActive(false);
    }
}





