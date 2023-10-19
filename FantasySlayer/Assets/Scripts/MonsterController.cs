using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour
{
    private HeroKnight player;
    private MonsterController m;

    public float spd = 5f;

    public int maxHealth;
    public int currentHealth;

    public int damage;

    public int nextMove;
    public float attackDelay = 2f; // 공격 딜레이 시간
    private float lastAttackTime; // 마지막으로 공격한 시간

    private bool isDead = false; // 사망 여부

    // 공격 반응 관련
    public Transform apos;
    public Vector2 aBoxSize;

    // 이동 반응 관련
    public Transform mpos;
    public Vector2 mBoxSize;
    private float nextMoveTime; // 다음 이동 시간    

    private Rigidbody2D rigid;
    private Animator anim;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<HeroKnight>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        maxHealth = 100;
        currentHealth = 100;

        nextMoveTime = Time.time + Random.Range(.5f, 4.5f); // 초기 이동 시간 설정
    }

    private void Update()
    {
        if (Time.time >= nextMoveTime) // 현재시각이 다음 이동시각보다 클 경우
        {
            nextMove = Random.Range(-1, 2); // 랜덤한 방향 결정

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
                rigid.velocity = Vector2.zero;
            }

            nextMoveTime += Random.Range(.5f, 4.5f); // 다음번에 바꿔줄 시각 결정
        }

        // 이동
        bool isPlayerInRange = false;
        Vector3 playerDirection = Vector3.zero;

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(mpos.position, mBoxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {
                isPlayerInRange = true;
                playerDirection = (collider.transform.position - transform.position).normalized;
                break;
            }
        }

        if (isPlayerInRange)
        {
            // 플레이어가 있으면 플레이어의 방향으로 이동  
            rigid.velocity = new Vector2(playerDirection.x * spd, rigid.velocity.y);

            // 애니메이션 상태 변경  
            anim.SetInteger("AnimState", 1);

            // 몬스터가 플레이어를 바라보도록 회전
            if (playerDirection.x > 0)
                transform.eulerAngles = new Vector3(0, 0, 0);
            else if (playerDirection.x < 0)
                transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            // 바닥 확인
            Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2, LayerMask.GetMask("Floor"));

            if (rayHit.collider == null)
                nextMove *= -1;

            if (nextMove != 0)
            {
                anim.SetInteger("AnimState", 1);
                if (nextMove == -1)
                    transform.eulerAngles = new Vector3(0, 180);
                else if (nextMove == 1)
                    transform.eulerAngles = new Vector3(0, 0, 0);

                rigid.velocity = new Vector2(nextMove * spd, rigid.velocity.y);
            }
            else
            {
                anim.SetInteger("AnimState", 0);
                rigid.velocity = Vector2.zero;
            }
        }

        if (currentHealth <= 0 && !isDead)
        {
            anim.SetTrigger("Death");
            isDead = true;

            StartCoroutine(DieDelayTime(0.5f));
        }

        // 공격 범위 내에 플레이어가 있는지 확인
        Collider2D[] collider2DsAt = Physics2D.OverlapBoxAll(apos.position, aBoxSize, 0);
        foreach (Collider2D collider in collider2DsAt)
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

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(apos.position, aBoxSize, 0);

        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {
                collider.GetComponent<HeroKnight>().PlayerTakeDamage(damage);
            }
        }
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
        Gizmos.DrawWireCube(apos.position, aBoxSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(mpos.position, mBoxSize);
    }
}
