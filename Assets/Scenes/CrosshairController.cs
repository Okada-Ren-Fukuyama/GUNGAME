using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    RectTransform crosshair;

    void Start()
    {
        crosshair = GetComponent<RectTransform>();
        Cursor.visible = false; // マウスカーソルを消す（必要なら）
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        crosshair.position = mousePos;
    }
}
