using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public RectTransform crosshair; // è∆èÄImage
    public Canvas canvas;           // CrosshairÇ™ì¸Ç¡ÇƒÇ¢ÇÈCanvas

    void Update()
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera,
            out localPos);

        crosshair.localPosition = localPos;
    }
}
