using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateType
{
    Idle,Move,Dodge,MeleeAttack,Hurt,Death
}

public class Player : Character
{
    [Header("��ȡ�������")]
    public PlayerInput input;

    [Header("�ƶ�")]
    public Vector2 inputDirection;
    public float normalSpeed = 3f;//Ĭ���ٶ�
    public float attackSpeed = 1f;//����ʱ��ҵ��ٶ�
    private float currentSpeed;//��ǰ�ٶ�


    [Header("��ս����")]
    public bool isMeleeAttack;
    public float meleeAttackDamage;
    public Vector2 attackSize = new Vector2(1f, 1f);//������Χ
    public float offestX = 1f;//x��ƫ����
    public float offestY = 1f;//y��ƫ����
    public LayerMask enemyLayer;
    private Vector2 AttackAreaPos;

    [Header("����")]
    public bool isDodging = false;
    public float dodgeForce;//����
    public float dodgeDuration = 0f;
    public float dodgeCooldown = 1f;//������ȴʱ��
    public bool isDodegOnCooldown = false;//�����Ƿ�����ȴʱ����

    [Header("��������")]
    public bool isHurt;
    public bool isDead; 

    public SpriteRenderer sr;
    public Rigidbody2D rb;
    public Animator ani;

    private IState currentState;//��ǰ״̬
    // �ֵ�dictionary<����ֵ>��
    private Dictionary<PlayerStateType, IState> states = new Dictionary<PlayerStateType, IState>();

    private void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
        ani= GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        //ʵ����
        states.Add(PlayerStateType.Idle, new PlayerIdleState(this));
        states.Add(PlayerStateType.Move, new PlayerMoveState(this));
        states.Add(PlayerStateType.Dodge, new PlayerDodgeState(this));
        states.Add(PlayerStateType.MeleeAttack, new PlayerMeleeAttackState(this));
        states.Add(PlayerStateType.Hurt, new PlayerHurtState(this));
        states.Add(PlayerStateType.Death, new PlayerDeathState(this));

        //���ó�ʼ״̬
        TransitionState(PlayerStateType.Idle);

        input.EnableGameplayInput();//����������
    }

    protected override void OnEnable()
    {
        input.onMove += Move;
        input.onDodge += Dodge;
        input.onStopMove += StopMove;
        input.onMeleeAttack += MeleeAttack;
        base.OnEnable();
    }

    private void OnDisable()
    {
        input.onMove -= Move;
        input.onDodge -= Dodge;
        input.onStopMove -= StopMove;
        input.onMeleeAttack -= MeleeAttack;
    }

    //�����л�����״̬�ĺ���
    public void TransitionState(PlayerStateType type)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = states[type];
        currentState.OnEnter();
    }

    private void Update()
    {
        currentState.OnUpdate();
    }

    private void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    #region �ƶ�
   public void Move(Vector2 moveInput)
   {
       inputDirection = moveInput;
   }

   public void Move()
    {
        currentSpeed = isMeleeAttack ? attackSpeed : normalSpeed;

        rb.velocity = inputDirection * currentSpeed;
        if (inputDirection.x > 0) sr.flipX = false;
        if (inputDirection.x < 0) sr.flipX = true;
    }
    
    public void StopMove()
    {
        inputDirection=Vector2.zero;
    }
    #endregion

    #region ����
    public void Dodge()
    {
        if (!isDodging && !isDodegOnCooldown)
        {
            isDodging = true;
        }
    }

    public void DodgeOnCooldown()
    {
        StartCoroutine(nameof(DodgeOnCooldownCoroutine));
    }

    public IEnumerator DodgeOnCooldownCoroutine()
    {
        yield return new WaitForSeconds(dodgeCooldown);
        isDodegOnCooldown = false;
        Debug.Log("������ȴ����");
    }
    #endregion

    #region ��ս����
    public void MeleeAttack()
    {
        if(!isDodging)
        {
            ani.SetTrigger("meleeAttack");
            isMeleeAttack = true;
        }
    }
    void MeleeAttackAnimEvent(float isAttack)
    {
        AttackAreaPos = transform.position;

        offestX = sr.flipX ? -Mathf.Abs(offestX) : Mathf.Abs(offestX);

        AttackAreaPos.x += offestX;
        AttackAreaPos.y += offestY;

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(AttackAreaPos, attackSize, 0f, enemyLayer);

        foreach (Collider2D hitCollider in hitColliders)
        {
            hitCollider.GetComponent<Character>().TakeDamage(meleeAttackDamage * isAttack);
            //hitCollider.GetComponent<EnemyController>().Konckback(transform.position);
        }
    }
    #endregion

    #region ����
    public void PlayerHurt()
    {
        isHurt = true;
    }
    #endregion

    #region ����
    public void PlayerDead()
    {
        isDead = true;
        TransitionState(PlayerStateType.Death);
    }
    #endregion 

    //��ͼ���ڲ���
    private void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(AttackAreaPos,attackSize);
    }
}
