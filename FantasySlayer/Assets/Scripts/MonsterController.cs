using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour
{
    private HeroKnight player;
    private MonsterController m;

    public int maxHealth;
    public int currentHealth;

    public int damage;

    public int nextMove;
    public float attackDelay = 2f; // ���� ������ �ð�
    private float lastAttackTime; // ���������� ������ �ð�

    private bool isDead = false; // ��� ����

    public Transform pos;
    public Vector2 boxSize;

    private Rigidbody2D rigid;
    private Animator anim;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<HeroKnight>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        maxHealth = 100;
        currentHealth = 100;

        Invoke("Think", 2);
    }

    private void Update()
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

        // ���� ���� ���� �÷��̾ �ִ��� Ȯ��
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {
                // ���� ������
                if (Time.time >= lastAttackTime + attackDelay)
                {
                    StartAttack();
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    IEnumerator DelayedDamage(float delay)
    {
        yield return new WaitForSeconds(delay);

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {
                collider.GetComponent<HeroKnight>().PlayerTakeDamage(damage);
            }
        }
    }

    public void Think()
    {
        nextMove = Random.Range(-1, 2);

        float nextThinkTime = Random.Range(.5f, 4.5f);
        Invoke("Think", nextThinkTime);

        if (nextMove != 0)
        {
            anim.SetInteger("AnimState", 1);
            if (nextMove == -1)
                transform.eulerAngles = new Vector3(180, transform.eulerAngles.y + 180, transform.eulerAngles.z + 180);
            else
                transform.eulerAngles = Vector3.zero;

        }
        else
            anim.SetInteger("AnimState", 0);

    }

    void StartAttack()
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

        // ���� �ִϸ��̼� �Ŀ� ������ ó���� �����մϴ�.
        StartCoroutine(DelayedDamage(0.8f));
    }

    public void MonsterTakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
        anim.SetTrigger("Hit");
    }

    void Die()
    {
        if (isDead)
            Destroy(gameObject);
    }

    IEnumerator DieDelayTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        Die();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
