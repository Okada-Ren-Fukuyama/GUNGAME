using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMover : MonoBehaviour
{
    Vector2 velocity;
    RectTransform rect;

    public void Init(Vector2 vel)
    {
        velocity = vel;
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        rect.anchoredPosition += velocity * Time.deltaTime;
    }
}
