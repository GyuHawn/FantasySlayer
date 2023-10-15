using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private Animator animator;
    private bool isAttacking;
    private bool isWalking;

    private void Start()
    {
        animator = GetComponent<Animator>();
        isAttacking = false;
        isWalking = false;
    }

    private void Update()
    {
        // 공격 상태인 경우
        if (isAttacking)
        {
            // 공격2 애니메이션으로 전환
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
            {
                animator.SetTrigger("Attack2");
            }
            // 공격 상태가 끝났을 때 기본자세로 전환
            else if (!animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
            {
                animator.SetTrigger("Idle");
                isAttacking = false;
            }
        }

        // 피격 상태인 경우
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            // 피격 상태가 끝났을 때 기본자세로 전환
            if (!animator.IsInTransition(0))
            {
                animator.SetTrigger("Idle");
            }
        }

        // 걷기 상태인 경우, 걷기 애니메이션 재생 중일 때에도 계속해서 이동하도록 구현할 수 있습니다.
        else if (isWalking || animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            Move();
        }

        // 사망 상태인 경우 추가하고 싶은 로직을 작성할 수 있습니다.
        //animator.SetTrigger("Death");
    }

    public void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("Attack1");
        }
    }

    public void TakeDamage()
    {
        animator.SetTrigger("Hit");
    }

    public void Walk(bool shouldWalk)
    {
        isWalking = shouldWalk;
        animator.SetBool("Walk", shouldWalk);
    }

    private void Move()
    {
        // 몬스터 이동 로직 구현하기 
        // 예시: transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
