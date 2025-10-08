using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [Header("�I��Prefab")]
    public GameObject targetPrefab;

    [Header("�I���o���͈�(UI��RectTransform)")]
    public RectTransform targetArea;

    [Header("�����Ԋu(�b)")]
    public float spawnInterval = 1.5f;

    [Header("�G���A�̗]���i���E�E�㉺�̒����j")]
    public float marginX = 100f;
    public float marginY = 80f;

    [Header("�I�̊Ԋu�i���h�~�j")]
    public float minDistance = 120f; // �I���m�̍ŏ�����(px)

    // ���݂̓I��ێ�
    private List<GameObject> spawnedTargets = new List<GameObject>();

    void Start()
    {
        InvokeRepeating(nameof(SpawnTarget), 1f, spawnInterval);
    }

    void SpawnTarget()
    {

        // �������I(null)�����X�g����폜
        spawnedTargets.RemoveAll(t => t == null);

        // ���S�ȃ����_���ʒu��T��
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
        for (int i = 0; i < 20; i++) // �ő�20�񎎂�
        {
            float x = Random.Range(-areaSize.x / 2 + marginX, areaSize.x / 2 - marginX);
            float y = Random.Range(-areaSize.y / 2 + marginY, areaSize.y / 2 - marginY);
            Vector2 localPos = new Vector2(x, y);
            Vector2 worldPos = targetArea.TransformPoint(localPos);

            // ���̓I�Ƌ����`�F�b�N
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

        // 20�񎎂��Ă����S�ȏꏊ���Ȃ��ꍇ�͏o���Ȃ�
        return Vector2.zero;
    }

    void Update()
    {
        spawnedTargets.RemoveAll(t => t == null);
    }
}



