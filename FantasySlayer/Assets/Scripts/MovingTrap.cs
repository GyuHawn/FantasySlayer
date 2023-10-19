using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrap : MonoBehaviour
{
    public GameObject Arrow;
    public Transform shotPos;
    public int direction; // 1 : 왼쪽 / 2 : 오른쪽

    public float shootInterval = 1f; // 발사 속도
    public float arrowSpd = 10f; // 화살의 속도

    private float shootTimer = 0f;

    private void Start()
    {
        direction = 1;
    }

    void Update()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            ShootArrow();
            shootTimer = 0f;
        }
    }

    void ShootArrow()
    {
        Quaternion rotation = Quaternion.identity;
        float directionMultiplier = 0;

        if (direction == 1)
        {
            rotation = Quaternion.Euler(0, 180, 0);
            directionMultiplier = -1;
        }
        else if (direction == 2)
        {
            rotation = Quaternion.Euler(0, 0, 0);
            directionMultiplier = +1;
        }

        GameObject arrow = Instantiate(Arrow, shotPos.position, rotation);

        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

        if (rb != null)
            rb.velocity = new Vector2(directionMultiplier * arrowSpd, rb.velocity.y);
    }
}