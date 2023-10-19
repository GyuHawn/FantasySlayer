using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public int tDamage;
    private float damageTimer = 0f; // 타이머 추가

    private bool reconfirm = false;

    void Update()
    {
        damageTimer += Time.deltaTime; // 매 프레임마다 시간을 증가시킵니다.
    }


    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MoveMap"))
        {
            HeroKnight player = collision.gameObject.GetComponent<HeroKnight>();
            reconfirm = true;
        }
        /*if (collision.gameObject.CompareTag("EndMap"))
        {
            reconfirm = false;
        }*/

        if (collision.gameObject.CompareTag("Player"))
        {
            HeroKnight player = collision.gameObject.GetComponent<HeroKnight>();
            if (player != null && damageTimer >= 0.5f) // 0.5초가 지났는지 확인
            {
                player.PlayerTakeDamage(tDamage);
                damageTimer = 0f; // 타이머 초기화
            }
            if (gameObject.CompareTag("Arrow"))
            {
                Destroy(gameObject);
            }       
        }
        if (collision.gameObject.CompareTag("Delete"))
        {
            Destroy(gameObject);
        }
    }
}
