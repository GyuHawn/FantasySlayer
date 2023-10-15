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
        // ���� ������ ���
        if (isAttacking)
        {
            // ����2 �ִϸ��̼����� ��ȯ
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
            {
                animator.SetTrigger("Attack2");
            }
            // ���� ���°� ������ �� �⺻�ڼ��� ��ȯ
            else if (!animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
            {
                animator.SetTrigger("Idle");
                isAttacking = false;
            }
        }

        // �ǰ� ������ ���
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            // �ǰ� ���°� ������ �� �⺻�ڼ��� ��ȯ
            if (!animator.IsInTransition(0))
            {
                animator.SetTrigger("Idle");
            }
        }

        // �ȱ� ������ ���, �ȱ� �ִϸ��̼� ��� ���� ������ ����ؼ� �̵��ϵ��� ������ �� �ֽ��ϴ�.
        else if (isWalking || animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            Move();
        }

        // ��� ������ ��� �߰��ϰ� ���� ������ �ۼ��� �� �ֽ��ϴ�.
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
        // ���� �̵� ���� �����ϱ� 
        // ����: transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
