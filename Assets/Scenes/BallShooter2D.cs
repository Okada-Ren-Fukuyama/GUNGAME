using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShooter2D : MonoBehaviour
{
    public GameObject ballPrefab;
    public float shootForce = 500f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootBall();
        }
    }

    void ShootBall()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 spawnPos = Camera.main.transform.position;
        spawnPos.z = 0f;

        GameObject ball = Instantiate(ballPrefab, spawnPos, Quaternion.identity);

        Vector2 direction = (mousePos - spawnPos).normalized;
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * shootForce);
    }
}
