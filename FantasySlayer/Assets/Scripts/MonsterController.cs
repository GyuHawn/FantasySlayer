using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour
{
    private HeroKnight player;

    public int maxHealth;
    public int currentHealth;

    public int nextMove;
    public float attackDelay = 2f; // ���� ������ �ð�
    private float lastAttackTime; // ���������� ������ �ð�

    private bool isDead = false; // ��� ����

    private Rigidbody2D rigid;
    private Animator anim;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<HeroKnight>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        maxHealth = 100;
        currentHealth = 100;

        Invoke("Think", 0);
    }

    private void FixedUpdate()
    {
        // �̵�
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // �ٴ� Ȯ��
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2, LayerMask.GetMask("Floor"));
        if (rayHit.collider == null)
        {
            nextMove *= -1;
            CancelInvoke();
            Invoke("Think", 5);
        }

        // RaycastHit2D rayHitPlayer = Physics2D.Raycast(frontVec, transform.right * nextMove, 1f, LayerMask.GetMask("Player"));
        RaycastHit2D rayHitPlayer = Physics2D.Raycast(frontVec, Vector3.down, 2, LayerMask.GetMask("Player"));
        if (rayHitPlayer.collider != null && Time.time >= lastAttackTime + attackDelay)
        {
            Attack();
            lastAttackTime = Time.time;
        }

        if (nextMove != 0)
        {
            anim.SetInteger("AnimState", 1);
            if (nextMove == -1)
                transform.eulerAngles = new Vector3(0, 180, 0);
            else if (nextMove == 1)
                transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            anim.SetInteger("AnimState", 0);
        }

        if (currentHealth <= 0 && !isDead)
        {
            anim.SetTrigger("Death");
            isDead = true;
            StartCoroutine(DieDelayTime(0.5f));
        }

        // ����Լ�
        void Think()
        {
            nextMove = Random.Range(-1, 2);

            float nextThinkTime = Random.Range(2f, 5f);
            Invoke("Think", nextThinkTime);

            if (nextMove != 0)
            {
                anim.SetInteger("AnimState", 1);
                if (nextMove == -1)
                    transform.eulerAngles = new Vector3(0, 180, 0);
                else if (nextMove == 1)
                    transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                anim.SetInteger("AnimState", 0);
            }
        }

        void Attack()
        {
            int randomAttack = Random.Range(0, 2);

            switch (randomAttack)
            {
                case 0:
                    anim.SetTrigger("Attack1");
                    break;
                case 1:
                    anim.SetTrigger("Attack2");
                    break;
            }
        }

        void Die()
        {
            if (isDead)
            {
                Destroy(gameObject);
            }
        }

        IEnumerator DieDelayTime(float delay)
        {
            yield return new WaitForSeconds(delay);
            Die();
        }
    }
}