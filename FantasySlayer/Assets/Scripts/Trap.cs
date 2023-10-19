using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public int tDamage;
    private float damageTimer = 0f; // Ÿ�̸� �߰�

    private bool reconfirm = false;

    void Update()
    {
        damageTimer += Time.deltaTime; // �� �����Ӹ��� �ð��� ������ŵ�ϴ�.
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
            if (player != null && damageTimer >= 0.5f) // 0.5�ʰ� �������� Ȯ��
            {
                player.PlayerTakeDamage(tDamage);
                damageTimer = 0f; // Ÿ�̸� �ʱ�ȭ
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
