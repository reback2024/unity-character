using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding;
using UnityEditorInternal;
using System.Xml.Serialization;
//����״̬ö��
public enum EnemySateType
{
    Idle,Chase,Attack,Hurt,Death
}

public class Enemy : Character
{
    [Header("Ŀ��")]
    public Transform player;
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

    private IState currentState;
    // �ֵ�dictionary<����ֵ>��
    private Dictionary<EnemySateType, IState> states = new Dictionary<EnemySateType, IState>();

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        enemyColler = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        //ʵ��������״̬
        states.Add(EnemySateType.Idle, new EnemyIdleState(this));
        states.Add(EnemySateType.Chase, new EnemyChaseState(this));
        states.Add(EnemySateType.Attack, new EnemyAttackState(this));
        states.Add(EnemySateType.Hurt, new EnemyHurtState(this));
        states.Add(EnemySateType.Death, new EnemyDeathState(this));

        //Ĭ��״̬
        TransitionState(EnemySateType.Idle);
    }

    //�����л�����״̬�ĺ���
    public void TransitionState(EnemySateType type)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState=states[type];
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
    private void GeneratePath(Vector3 target)
    {
        currentIndex = 0;
        seeker.StartPath(transform.position, target, Path => 
        {
            pathPointlist = Path.vectorPath;
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
        TransitionState(EnemySateType.Death);
    }
    public void DestoryEnemy()
    {
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
