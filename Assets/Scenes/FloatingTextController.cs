using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingTextController : MonoBehaviour
{
    public float moveUpSpeed = 50f;
    public float fadeSpeed = 2f;

    private TextMeshProUGUI text;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show(string value, Color color)
    {
        text.text = value;
        text.color = color;
    }

    void Update()
    {
        transform.Translate(Vector3.up * moveUpSpeed * Time.deltaTime);
        canvasGroup.alpha -= fadeSpeed * Time.deltaTime;

        if (canvasGroup.alpha <= 0)
        {
            Destroy(gameObject);
        }
    }
}

