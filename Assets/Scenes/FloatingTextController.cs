using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingTextController : MonoBehaviour
{
    public float fadeSpeed = 1f;

    private TextMeshProUGUI text;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();

        text.fontSize = 60;
    }

    public void Show(string value, Color color)
    {
        text.text = value;
        text.color = color;

        // その場でフェードアウト
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}

