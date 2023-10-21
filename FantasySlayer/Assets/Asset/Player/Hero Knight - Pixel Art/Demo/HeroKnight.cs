using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine.UIElements;

public class HeroKnight : MonoBehaviour
{
    private Trap trap;
    public PlayerUI playerUI;

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] int m_jumpCount = 2;
    [SerializeField] float m_jumpForce = 6.0f;
    [SerializeField] float m_rollForce = 6.0f;
    [SerializeField] bool m_noBlood = false;
    [SerializeField] GameObject m_slideDust;

    private DynamicJoystick joy;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_HeroKnight m_groundSensor;
    private Sensor_HeroKnight m_wallSensorR1;
    private Sensor_HeroKnight m_wallSensorR2;
    private Sensor_HeroKnight m_wallSensorL1;
    private Sensor_HeroKnight m_wallSensorL2;
    private bool m_isWallSliding = false;
    private int m_currentJumpCount = 0;
    private bool m_grounded = false;
    private bool m_rolling = false;
    private int m_facingDirection = 1;
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private float m_delayToIdle = 0.0f;
    private float m_rollDuration = 8.0f / 14.0f;
    private float m_rollCurrentTime;
    public float climbSpeed = 5f;
    private bool isClimbing = false;
    private bool isDead = false; // 사망 여부

    public int maxHealth;
    public int currentHealth;
    public TMP_Text hpText;
    private int damage = 30;

    public Transform pos;
    public Vector2 boxSize;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();

        GameObject hpTextObject = GameObject.Find("HPText");
        if (hpTextObject != null)
        {
            hpText = hpTextObject.GetComponent<TMP_Text>();
        }

        GameObject joystickObject = GameObject.Find("Dynamic Joystick");
        if (joystickObject != null)
        {
            joy = joystickObject.GetComponent<DynamicJoystick>();
        }

        if (playerUI == null)
        {
            playerUI = FindObjectOfType<PlayerUI>();
        }


        maxHealth = 100;
        currentHealth = 100;
    }

    void Update()
    {
        // 조이스틱 입력
        float inputjX = joy.Horizontal;
       // float inputjY = joy.Vertical;

        hpText.text = "HP : " + currentHealth.ToString() + " / " + maxHealth.ToString();

        // 공격 콤보를 제어하는 ​​타이머 증가
        m_timeSinceAttack += Time.deltaTime;

        // 롤 지속 시간을 확인하는 타이머 증가
        if (m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // 타이머가 지속 시간을 연장하면 롤링을 비활성화합니다.
        if (m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        // 캐릭터가 방금 땅에 착지한 경우, 점프 카운트 초기화
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
            m_currentJumpCount = 0;
        }

        // 캐릭터가 막 떨어지기 시작했는지 확인
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        float inputX = Input.GetAxis("Horizontal");

        // 걷는 방향에 따라 스프라이트 방향을 바꿉니다.
        if (inputX > 0 || inputjX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }

        else if (inputX < 0 || inputjX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        // 키보드 이동
        /*if (!m_rolling)
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);*/

        // 조이스틱 이동
        if (!m_rolling)
            m_body2d.velocity = new Vector2(inputjX * m_speed, m_body2d.velocity.y);

        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // 공격
        /* if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
         {
             Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
             foreach (Collider2D collider in collider2Ds)
             {
                 if (collider.tag == "Monster" || collider.tag == "Boss")
                 {
                     collider.GetComponent<MonsterController>().MonsterTakeDamage(damage);
                 }
             }

             m_currentAttack++;

             // 세 번째 공격 이후 1로 다시 루프합니다.
             if (m_currentAttack > 3)
                 m_currentAttack = 1;

             // 마지막 공격 이후 시간이 너무 길면 공격 콤보를 재설정합니다.
             if (m_timeSinceAttack > 1.0f)
                 m_currentAttack = 1;

             // 세 가지 공격 애니메이션 중 하나를 호출합니다.(Attack 1,2,3)
             m_animator.SetTrigger("Attack" + m_currentAttack);

             // 타이머 재설정
             m_timeSinceAttack = 0.0f;
         }*/

        // 막기
        if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1))
            m_animator.SetBool("IdleBlock", false);

        // 구르기
        else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        }

        // 점프
       /* else if (Input.GetKeyDown("space") && !m_rolling && (m_grounded || m_currentJumpCount < m_jumpCount))
        {
            float jumpForce = m_jumpForce;

            if (!m_grounded)
            {
                ++m_currentJumpCount;
            }

            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, jumpForce);
            m_groundSensor.Disable(0.2f);
        }*/

        // 달리기
        else if (Mathf.Abs(inputX) > Mathf.Epsilon || Mathf.Abs(inputjX) > Mathf.Epsilon)
        {
            // 타이머 재설정
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        // 기본자세
        else
        {
            // 깜박임이 유휴 상태로 전환되는 것을 방지합니다.
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }

        // 사다리
        /*if (isClimbing)
        {
            float verticalInput = Input.GetAxis("Vertical");
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, verticalInput * climbSpeed);
            m_animator.SetInteger("AnimState", 1); // 삭제 확인
        }*/
        

        // 사망
        if (currentHealth <= 0 && !isDead)
        {
            m_animator.SetTrigger("Death");
            isDead = true;
            UIManager.gameOver();
            StartCoroutine(DieDelayTime(1f));
        }
    }
    IEnumerator DieDelayTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        Die();
    }

    IEnumerator DieAinDelayTime(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    void Die()
    {
        if (isDead)
        {     
            // 사망시 UI 생성
            StartCoroutine(DieAinDelayTime(100f));
            m_animator.enabled = false;
        }
    }

    // 슬라이드 애니메이션에서 호출됩니다.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;
 
        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // 올바른 화살표 생성 위치를 설정합니다.
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // 화살표를 올바른 방향으로 돌립니다.
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }

    public void PlayerTakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
        m_animator.SetTrigger("Hit");
        UpdateHealthBar(currentHealth);
    }

    void UpdateHealthBar(float currentHealth)
    {
        float ratio = currentHealth / maxHealth;
        playerUI.healthBar.fillAmount = ratio;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ladder")
        {
            isClimbing = true;
            m_body2d.gravityScale = 0;  // 중력 영향 제거
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ladder")
        {
            isClimbing = false;
            m_body2d.gravityScale = 1;  // 중력 영향 복구
            m_animator.SetInteger("AnimState", 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            currentHealth = currentHealth - trap.tDamage;
        }
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }*/

    // 모바일용 기능버튼 코드
    public void Jump()
    {
            if (!m_rolling && (m_grounded || m_currentJumpCount < m_jumpCount))
            {
                float jumpForce = m_jumpForce;

                if (!m_grounded)
                {
                    ++m_currentJumpCount;
                }

                m_animator.SetTrigger("Jump");
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, jumpForce);
                m_groundSensor.Disable(0.2f);
            }
    }

    public void Attack()
    {
        if (m_timeSinceAttack > 0.25f && !m_rolling)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Monster" || collider.tag == "Boss")
                {
                    collider.GetComponent<MonsterController>().MonsterTakeDamage(damage);
                }
            }

            m_currentAttack++;

            // 세 번째 공격 이후 1로 다시 루프합니다.
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // 마지막 공격 이후 시간이 너무 길면 공격 콤보를 재설정합니다.
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // 세 가지 공격 애니메이션 중 하나를 호출합니다.(Attack 1,2,3)
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // 타이머 재설정
            m_timeSinceAttack = 0.0f;
        }
    }

    public void Climb(bool isPressed)
    {
        if (isClimbing)
        {
            float verticalInput = isPressed ? 1f : -1f;
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, verticalInput * climbSpeed);
            m_animator.SetInteger("AnimState", 1);
        }
    }
}   