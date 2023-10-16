using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public int nextMove;

    private Rigidbody2D rigid;
    private Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        Invoke("Think", 2);
    }

    private void FixedUpdate()
    {
        // 이동
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // 플랫폼 확인
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2, LayerMask.GetMask("Floor"));

        if (rayHit.collider == null)
        {
            nextMove *= -1;
            CancelInvoke();
            Invoke("Think", 10);
        }

        // 움직임에 따른 애니메이션 상태 변경
        if (nextMove != 0)
        {
            anim.SetInteger("AnimState", 1); // 이동 중일 때 AnimState는 '1'
            if (nextMove == -1)
                transform.eulerAngles = new Vector3(0, 180, 0);
            else if (nextMove == 1)
                transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            anim.SetInteger("AnimState", 0); // 정지했을 때 AnimState는 '0'
        }
    }

    // 재귀함수
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
}
