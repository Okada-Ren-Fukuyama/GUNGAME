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
    public int minSpawnCount = 1;

    [Header("一度に出す的の最大数")]
    public int maxSpawnCount = 3;

    [Header("エリアの余白（左右・上下の調整）")]
    public float marginX = 100f;
    public float marginY = 80f;

    [Header("的の間隔（被り防止）")]
    public float minDistance = 120f;

    [Header("レア的の出現確率(0〜1)")]
    [Range(0f, 1f)] public float rareChance = 0.2f;

    private List<GameObject> spawnedTargets = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnTarget();

            // 次の生成までランダムで待つ
            float wait = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(wait);
        }
    }

    void SpawnTarget()
    {
        spawnedTargets.RemoveAll(t => t == null);

        int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);
        bool rareSpawned = false;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 randomPos = GetSafeRandomPosition();
            if (randomPos == Vector2.zero) continue;

            GameObject target = Instantiate(targetPrefab, targetArea);
            target.transform.position = randomPos;
            spawnedTargets.Add(target);

            // --- ここでレア判定をSpawnerが決める ---
            bool makeRare = !rareSpawned && Random.value < rareChance;

            Image img = target.GetComponent<Image>();
            if (img != null)
            {
                if (makeRare)
                {
                    rareSpawned = true;
                    img.color = Color.red;

                    // Targetスクリプトがあるなら isRare = true を設定
                    Target t = target.GetComponent<Target>();
                    if (t != null)
                        t.SetRare(true);
                }
                else
                {
                    img.color = Color.gray;
                    Target t = target.GetComponent<Target>();
                    if (t != null)
                        t.SetRare(false);
                }
            }
        }
    }




    Vector2 GetSafeRandomPosition()
    {
        Vector2 areaSize = targetArea.rect.size;
        for (int i = 0; i < 20; i++)
        {
            float x = Random.Range(-areaSize.x / 2 + marginX, areaSize.x / 2 - marginX);
            float y = Random.Range(-areaSize.y / 2 + marginY, areaSize.y / 2 - marginY);
            Vector2 localPos = new Vector2(x, y);
            Vector2 worldPos = targetArea.TransformPoint(localPos);

            bool tooClose = false;
            foreach (var t in spawnedTargets)
            {
                if (t == null) continue;
                float dist = Vector2.Distance(worldPos, t.transform.position);
                if (dist < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
                return worldPos;
        }

        return Vector2.zero;
    }

    void Update()
    {
        spawnedTargets.RemoveAll(t => t == null);
    }
}




