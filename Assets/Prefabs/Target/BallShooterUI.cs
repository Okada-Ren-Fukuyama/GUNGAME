using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallShooterUI : MonoBehaviour
{
    public RectTransform canvasRect; // CanvasのRectTransform
    public GameObject ballPrefab;    // UI用の玉プレハブ
    public float speed = 500f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                Input.mousePosition,
                null, // カメラがScreen Space OverlayならnullでOK
                out mousePos
            );

            // 玉をCanvas内に生成
            GameObject ball = Instantiate(ballPrefab, canvasRect);
            RectTransform ballRect = ball.GetComponent<RectTransform>();
            ballRect.anchoredPosition = Vector2.zero; // 中心からスタート

            // 方向を計算
            Vector2 direction = (mousePos - ballRect.anchoredPosition).normalized;

            // 玉を動かす（UIなので物理じゃなくてスクリプトで動かす）
            ball.AddComponent<BallMover>().Init(direction * speed);
        }
    }
}
