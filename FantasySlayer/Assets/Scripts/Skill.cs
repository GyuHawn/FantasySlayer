using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public int sdamage;

    private float damageTimer = 0f;

    void Update()
    {
        damageTimer += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HeroKnight player = other.gameObject.GetComponent<HeroKnight>();

            if (player != null)
            {
                if (gameObject.CompareTag("Skill1"))
                {
                    player.PlayerTakeDamage(sdamage);
                    Destroy(gameObject);
                }
                if (gameObject.CompareTag("Skill2"))
                {
                    player.PlayerTakeDamage(sdamage);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HeroKnight player = collision.gameObject.GetComponent<HeroKnight>();
            if (damageTimer >= 1f)
            {
                if (gameObject.CompareTag("Skill2"))
                {
                    player.PlayerTakeDamage(sdamage);
                    damageTimer = 0f;   
                }
            }
            if (damageTimer >= 0.5f)
            {
                if (gameObject.CompareTag("Skill3"))
                {
                    player.PlayerTakeDamage(sdamage);
                    damageTimer = 0f;
                }
            }
        }
    }
}
