using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    RectTransform crosshair;
    Canvas canvas;

    void Start()
    {
        crosshair = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera, // Å©Ç±Ç±Ç™èdóvÅI
            out mousePos
        );

        crosshair.localPosition = mousePos;
    }
}
