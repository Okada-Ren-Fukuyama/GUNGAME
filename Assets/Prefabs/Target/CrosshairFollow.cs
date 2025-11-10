using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairFollow : MonoBehaviour
{
    RectTransform rectTransform;

    void Start()
    {
        // 自分自身のRectTransformを覚えておく
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // マウスの位置を画面上の座標で取得
        Vector2 mousePos = Input.mousePosition;

        // 自分の位置をマウスの位置に合わせる
        rectTransform.position = mousePos;
    }
}
