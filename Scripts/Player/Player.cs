using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum PlayerStateType
{
    Idle,Move,Dodge,MeleeAttack,Hurt,Death
}

public class Player : Character
{
    [Header("获取玩家输入")]
    public PlayerInput input;

    [Header("移动")]
    public Vector2 inputDirection;
    public float normalSpeed = 3f;//默认速度
    public float attackSpeed = 1f;//攻击时玩家的速度
    private float currentSpeed;//当前速度


    [Header("近战攻击")]
    public bool isMeleeAttack;
    public float meleeAttackDamage;
    public Vector2 attackSize = new Vector2(1f, 1f);//攻击范围
    public float offestX = 1f;//x轴偏移量
    public float offestY = 1f;//y轴偏移量
    public LayerMask enemyLayer;//敌人图层
    public LayerMask destructibleLayer;//可破环物体图层
    private Vector2 AttackAreaPos;

    [Header("闪避")]
    public bool isDodging = false;
    public float dodgeForce;//推力
    public float dodgeDuration = 0f;
    public float dodgeCooldown = 1f;//闪避冷却时间
    public bool isDodegOnCooldown = false;//闪避是否在冷却时间中
    public UnityEvent<float> OnDodgeUpdate;

    [Header("受伤死亡")]
    public bool isHurt;
    public bool isDead;

    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator ani;

    private IState currentState;//当前状态
    // 字典dictionary<键，值>对
    private Dictionary<PlayerStateType, IState> states = new Dictionary<PlayerStateType, IState>();

    private void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
        ani= GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        //实例化
        states.Add(PlayerStateType.Idle, new PlayerIdleState(this));
        states.Add(PlayerStateType.Move, new PlayerMoveState(this));
        states.Add(PlayerStateType.Dodge, new PlayerDodgeState(this));
        states.Add(PlayerStateType.MeleeAttack, new PlayerMeleeAttackState(this));
        states.Add(PlayerStateType.Hurt, new PlayerHurtState(this));
        states.Add(PlayerStateType.Death, new PlayerDeathState(this));

        //设置初始状态
        TransitionState(PlayerStateType.Idle);

        input.EnableGameplayInput();//启动动作表
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

    //用于切换敌人状态的函数
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

    #region 移动
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

    #region 翻滚
    public void Dodge()
    {
        if (!isDodging && !isDodegOnCooldown)
        {
            isDodging = true;
        }
    }

    public void DodgeOnCooldown()
    {
        OnDodgeUpdate?.Invoke(dodgeCooldown);//更新冷却条，传入冷却时间
        StartCoroutine(nameof(DodgeOnCooldownCoroutine));
    }

    public IEnumerator DodgeOnCooldownCoroutine()
    {
        yield return new WaitForSeconds(dodgeCooldown);
        isDodegOnCooldown = false;
        Debug.Log("翻滚冷却结束");
    }
    #endregion

    #region 近战攻击
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
        Collider2D[] destructiveColliders = Physics2D.OverlapBoxAll(AttackAreaPos, attackSize, 0f, destructibleLayer);

        //判断是否为敌人
        foreach (Collider2D hitCollider in hitColliders)
        {
            hitCollider.GetComponent<Character>().TakeDamage(meleeAttackDamage * isAttack);
            //hitCollider.GetComponent<EnemyController>().Konckback(transform.position);

        }

        //判断是否为可破坏物体
        foreach(Collider2D hitCollider in destructiveColliders)
        {
            hitCollider.GetComponent<Destructible>().DestroyObject();
        }

    }
    #endregion

    #region 受伤
    public void PlayerHurt()
    {
        isHurt = true;
    }
    #endregion

    #region 死亡
    public void PlayerDead()
    {
        isDead = true;
        input.DisableAllInputs();
        TransitionState(PlayerStateType.Death);
    }
    #endregion 

    //绘图用于测试
    private void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(AttackAreaPos,attackSize);
    }
}
