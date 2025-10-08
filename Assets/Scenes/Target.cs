using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("寿命の最小秒数")]
    public float minLifeTime = 2f;

    [Header("寿命の最大秒数")]
    public float maxLifeTime = 5f;

    private float lifeTime;

    void Start()
    {
        // ランダムな寿命を決定（例：2〜5秒）
        lifeTime = Random.Range(minLifeTime, maxLifeTime);

        // 決定した時間後に自動で削除
        Destroy(gameObject, lifeTime);
    }
}
