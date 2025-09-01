using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding;
using UnityEditorInternal;
using System.Xml.Serialization;
//����״̬ö��
public enum EnemyStateType
{
    Idle,Patrol,Chase,Attack,Hurt,Death
}

public class Enemy : Character
{
    [Header("Ŀ��")]
    public Transform player;

    [Header("����Ѳ��")]
    public float IdleDuration;//����ʱ��
    public Transform[] patrolPoints;//Ѳ�ߵ�
    public int targetPointIndex = 0;//Ŀ��Ѳ�ߵ�


    [Header("�ƶ�׷��")]
    [SerializeField] public float currentSpeed = 0;
    public Vector2 MovementInput { get; set; }

    public float chaseDistance = 3f;//׷������
    public float attackDistance = 0.8f;//��������

    private Seeker seeker;
    [HideInInspector] public List<Vector3> pathPointlist;//·�����б�
    [HideInInspector]public int currentIndex;//·��������
    private float pathGenerateInterval = 0.5f;//ÿ0.5s����һ��·��
    private float pathGenerateTimer = 0f;//��ʱ��

    [Header("����")]
    public float meleeAttackDamage;//��ս�����˺�
    public LayerMask playerLayer;//��ʾ���ͼ��
    public float AttackCooldownDuration = 0.5f;//������ȴʱ��
    [HideInInspector] public float distance;
    public bool isAttack = true;

    [Header("���˻���")]
    public bool isHurt;
    public bool isKnokback = true;
    public float knokbackForce = 10f;
    public float knokbackForecDuration = 0.1f;

    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Collider2D enemyColler;

    private IState currentState;//��ǰ״̬
    // �ֵ�dictionary<����ֵ>��
    private Dictionary<EnemyStateType, IState> states = new Dictionary<EnemyStateType, IState>();

    private PickupSpawner PickupSpawner;//������Ʒ�ű�
    
    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        enemyColler = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        PickupSpawner=GetComponent<PickupSpawner>();

        //ʵ��������״̬
        states.Add(EnemyStateType.Idle, new EnemyIdleState(this));
        states.Add(EnemyStateType.Chase, new EnemyChaseState(this));
        states.Add(EnemyStateType.Attack, new EnemyAttackState(this));
        states.Add(EnemyStateType.Hurt, new EnemyHurtState(this));
        states.Add(EnemyStateType.Death, new EnemyDeathState(this));
        states.Add(EnemyStateType.Patrol, new EnemyPatrolState(this));

        //Ĭ��״̬
        TransitionState(EnemyStateType.Idle);
    }

    //�����л�����״̬�ĺ���
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

    //�ж��Ƿ���׷����Χ��
    public void GetPlayerTransform()
    {
        Collider2D[] chaseColliders=Physics2D.OverlapCircleAll(transform.position,chaseDistance,playerLayer);

        if(chaseColliders.Length > 0 )//�����׷����Χ��
        {
            player = chaseColliders[0].transform;
            distance=Vector2.Distance(player.position,transform.position);
        }
        else
        {
            player = null;
        }
    }

    #region �ƶ�
    //�ƶ�����
    public void Move()
    {
        if (MovementInput.magnitude > 0.1f && currentSpeed >= 0)
        {
            rb.velocity = MovementInput * currentSpeed;
            //������ҷ�ת
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

    #region �Զ�Ѱ·
    //�Զ�Ѱ·
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
    //��ȡ·����
    public void GeneratePath(Vector3 target)
    {
        currentIndex = 0;
        seeker.StartPath(transform.position, target, Path => 
        {
            pathPointlist = Path.vectorPath;
            //�����ҵ����ǵ�Ŀ����·����ÿ���������
        });
    }
    #endregion

    #region ���˽�ս����֡�¼�
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

    #region ����
    //�����¼������Ļص�����
    public void EnemyHurt()
    {
        isHurt = true;
    }
    #endregion

    #region ����
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

    //��ʾ׷����Χ
    private void OnDrawGizmosSelected()
    {
        //��ʾ������Χ
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        //��ʾ׷����Χ
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
