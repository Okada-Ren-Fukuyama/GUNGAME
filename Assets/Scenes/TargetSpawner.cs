using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [Header("的のPrefab")]
    public GameObject targetPrefab;

    [Header("的を出す範囲(UIのRectTransform)")]
    public RectTransform targetArea;

    [Header("生成間隔(秒)")]
    public float spawnInterval = 1.5f;

    [Header("エリアの余白（左右・上下の調整）")]
    public float marginX = 100f;
    public float marginY = 80f;

    [Header("的の間隔（被り防止）")]
    public float minDistance = 120f; // 的同士の最小距離(px)

    // 現在の的を保持
    private List<GameObject> spawnedTargets = new List<GameObject>();

    void Start()
    {
        InvokeRepeating(nameof(SpawnTarget), 1f, spawnInterval);
    }

    void SpawnTarget()
    {

        // 消えた的(null)をリストから削除
        spawnedTargets.RemoveAll(t => t == null);

        // 安全なランダム位置を探す
        Vector2 randomPos = GetRandomPositionInArea();


        if (randomPos != Vector2.zero)
        {
            GameObject target = Instantiate(targetPrefab, targetArea);
            target.transform.position = randomPos;
            spawnedTargets.Add(target);
        }
    }

    Vector2 GetRandomPositionInArea()
    {
        Vector2 areaSize = targetArea.rect.size;
        for (int i = 0; i < 20; i++) // 最大20回試す
        {
            float x = Random.Range(-areaSize.x / 2 + marginX, areaSize.x / 2 - marginX);
            float y = Random.Range(-areaSize.y / 2 + marginY, areaSize.y / 2 - marginY);
            Vector2 localPos = new Vector2(x, y);
            Vector2 worldPos = targetArea.TransformPoint(localPos);

            // 他の的と距離チェック
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

        // 20回試しても安全な場所がない場合は出さない
        return Vector2.zero;
    }

    void Update()
    {
        spawnedTargets.RemoveAll(t => t == null);
    }
}



