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
    public float attackDelay = 2f; // 공격 딜레이 시간
    private float lastAttackTime; // 마지막으로 공격한 시간

    private bool isDead = false; // 사망 여부

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
        // 이동
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // 바닥 확인
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

        // 공격 범위 내에 플레이어가 있는지 확인
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {
                // 공격 딜레이
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

        // 공격 애니메이션 후에 데미지 처리를 시작합니다.
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
