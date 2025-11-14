using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMover : MonoBehaviour
{
    private Vector2 velocity;           // BallShooterUI Ç©ÇÁìnÇ∑ë¨ìx
    private RectTransform rectTransform;
    public float lifeTime = 2f;         // è¡Ç¶ÇÈÇ‹Ç≈ÇÃéûä‘

    public void Init(Vector2 vel)
    {
        velocity = vel;
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        rectTransform.anchoredPosition += velocity * Time.deltaTime;
    }
}
