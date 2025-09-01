using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding;
using UnityEditorInternal;
using System.Xml.Serialization;
//敌人状态枚举
public enum EnemyStateType
{
    Idle,Patrol,Chase,Attack,Hurt,Death
}

public class Enemy : Character
{
    [Header("目标")]
    public Transform player;

    [Header("待机巡逻")]
    public float IdleDuration;//待机时间
    public Transform[] patrolPoints;//巡逻点
    public int targetPointIndex = 0;//目标巡逻点


    [Header("移动追击")]
    [SerializeField] public float currentSpeed = 0;
    public Vector2 MovementInput { get; set; }

    public float chaseDistance = 3f;//追击距离
    public float attackDistance = 0.8f;//攻击距离

    private Seeker seeker;
    [HideInInspector] public List<Vector3> pathPointlist;//路径点列表
    [HideInInspector]public int currentIndex;//路径点索引
    private float pathGenerateInterval = 0.5f;//每0.5s生成一次路径
    private float pathGenerateTimer = 0f;//计时器

    [Header("攻击")]
    public float meleeAttackDamage;//近战攻击伤害
    public LayerMask playerLayer;//表示玩家图层
    public float AttackCooldownDuration = 0.5f;//攻击冷却时间
    [HideInInspector] public float distance;
    public bool isAttack = true;

    [Header("受伤击退")]
    public bool isHurt;
    public bool isKnokback = true;
    public float knokbackForce = 10f;
    public float knokbackForecDuration = 0.1f;

    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Collider2D enemyColler;

    private IState currentState;//当前状态
    // 字典dictionary<键，值>对
    private Dictionary<EnemyStateType, IState> states = new Dictionary<EnemyStateType, IState>();

    private PickupSpawner PickupSpawner;//掉落物品脚本
    
    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        enemyColler = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        PickupSpawner=GetComponent<PickupSpawner>();

        //实例化敌人状态
        states.Add(EnemyStateType.Idle, new EnemyIdleState(this));
        states.Add(EnemyStateType.Chase, new EnemyChaseState(this));
        states.Add(EnemyStateType.Attack, new EnemyAttackState(this));
        states.Add(EnemyStateType.Hurt, new EnemyHurtState(this));
        states.Add(EnemyStateType.Death, new EnemyDeathState(this));
        states.Add(EnemyStateType.Patrol, new EnemyPatrolState(this));

        //默认状态
        TransitionState(EnemyStateType.Idle);
    }

    //用于切换敌人状态的函数
    public void TransitionState(EnemyStateType type)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState=states[type];
        currentState.OnEnter();
    }

    private void Start()
    {
        EnemyManager.Instance.EnemyCount++;
    }

    private void OnDestroy()
    {
        EnemyManager.Instance.EnemyCount--;
    }

    private void Update()
    {
        currentState.OnUpdate();
    }

    private void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    //判断是否在追击范围内
    public void GetPlayerTransform()
    {
        Collider2D[] chaseColliders=Physics2D.OverlapCircleAll(transform.position,chaseDistance,playerLayer);

        if(chaseColliders.Length > 0 )//玩家在追击范围内
        {
            player = chaseColliders[0].transform;
            distance=Vector2.Distance(player.position,transform.position);
        }
        else
        {
            player = null;
        }
    }

    #region 移动
    //移动函数
    public void Move()
    {
        if (MovementInput.magnitude > 0.1f && currentSpeed >= 0)
        {
            rb.velocity = MovementInput * currentSpeed;
            //玩家左右翻转
            if (MovementInput.x < 0)
            {
                sr.flipX = false;
            }
            if (MovementInput.x > 0)
            {
                sr.flipX = true;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    #endregion

    #region 自动寻路
    //自动寻路
    public void Autopath()
    {
        if(player == null) return;
        pathGenerateTimer += Time.deltaTime;
        if (pathGenerateTimer >= pathGenerateInterval)
        {
            GeneratePath(player.position);
            pathGenerateTimer = 0f;
        }

        if (pathPointlist == null || pathPointlist.Count <= 0 || pathPointlist.Count <= currentIndex)
        {
            GeneratePath(player.position);
        }
        else if (currentIndex < pathPointlist.Count) 
        {
            if(Vector2.Distance(transform.position, pathPointlist[currentIndex]) <= 0.1f)
            {
                currentIndex++;
                if (currentIndex >= pathPointlist.Count)
                    GeneratePath(player.position);
            }
        }
    }
    //获取路径点
    public void GeneratePath(Vector3 target)
    {
        currentIndex = 0;
        seeker.StartPath(transform.position, target, Path => 
        {
            pathPointlist = Path.vectorPath;
            //这里找到的是到目标点的路径的每个点的坐标
        });
    }
    #endregion

    #region 敌人近战攻击帧事件
    IEnumerator AttackCooldownCoroutine()
    {
        yield return new WaitForSeconds(AttackCooldownDuration);
        isAttack = true;
    }

    public void AttackColdown()
    {
        StartCoroutine(nameof(AttackCooldownCoroutine));
    }

    private void MeleeAttcakAnimEvent()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackDistance, playerLayer);
    
        foreach(Collider2D hitCollider in hitColliders)
        {
            hitCollider.GetComponent<Character>().TakeDamage(meleeAttackDamage);
        }
    }
    #endregion

    #region 受伤
    //受伤事件触发的回调函数
    public void EnemyHurt()
    {
        isHurt = true;
    }
    #endregion

    #region 死亡
    public void EnemyDie()
    {
        TransitionState(EnemyStateType.Death);
    }
    public void DestoryEnemy()
    {
        PickupSpawner.DropItems();
        Destroy(this.gameObject);
    }
    #endregion

    //显示追击范围
    private void OnDrawGizmosSelected()
    {
        //显示攻击范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        //显示追击范围
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
