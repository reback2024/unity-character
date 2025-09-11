using UnityEngine;
using UnityEngine.InputSystem;

// ȷ����ɫ GameObject �Զ���ӱ�Ҫ���
[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class RoleMove : MonoBehaviour, InputActions.IGameplayActions
{
    [Header("�ƶ�����")]
    [Tooltip("��ɫ�ƶ��ٶȣ�����Inspector������")]
    public float moveSpeed = 3.5f;

    [Header("����������")]
    [Tooltip("�������п����ƶ�״̬��Float��������Ĭ����'speed'��")]
    public string speedParamName = "speed";

    // 1. �����������
    private Rigidbody2D rb;          // 2D���壨���������ƶ���
    private Animator animator;      // �����������ƶ����л���
    private SpriteRenderer sr;      // ������Ⱦ�������ƽ�ɫ���ҷ�ת��

    // 2. InputActionsʵ����������Զ������ࣩ
    private InputActions inputActions;
    // �����ƶ����루��InputActions��Move������ȡ��
    private Vector2 moveInput;

    private void Awake()
    {
        // ��ʼ��������ã��Զ���ȡ�������ֶ��ϸ�ֵ��
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        // ��ʼ��InputActions�������Զ����ɵ�InputActions��
        inputActions = new InputActions();
        // ����ǰ�ű�ע��ΪGameplay�����Ļص������ߣ��ؼ�������OnMove��������
        inputActions.Gameplay.SetCallbacks(this);

    }

    // 3. ����ص���ʵ��InputActions.IGameplayActions�ӿڵ�OnMove������
    // ��Move���������루WASD/�����/�ֱ���ҡ�ˣ�ʱ�Զ�����
    public void OnMove(InputAction.CallbackContext context)
    {
        // ��ȡMove����������ֵ��Vector2���ͣ�x=���ң�y=���£�
        moveInput = context.ReadValue<Vector2>();
    }

    // 4. �����ƶ�������FixedUpdate������������ͬ��������֡�ʲ���Ӱ���ƶ���
    private void FixedUpdate()
    {
        // ���������ƶ��ٶȣ����뷽�� �� �ƶ��ٶȣ�
        Vector2 finalMovement = moveInput * moveSpeed;
        // Ӧ���ƶ������壨���ƽ�ɫʵ��λ�ƣ�
        rb.velocity = finalMovement;

        // ���ƽ�ɫ���ҷ�ת������x�����뷽��
        if (moveInput.x != 0)  // ֻ������������ʱ�ŷ�ת
        {
            // x>0������ �� ����ת��x<0������ �� ��תX��
            sr.flipX = moveInput.x < 0;
        }
    }

    // 5. ���������£�����Update����֡ͬ�����������ɸ�������
    private void Update()
    {
        // ���㵱ǰ��ɫ��ʵ���ƶ��ٶȴ�С�����Է���ֻ������������
        float currentMoveSpeed = rb.velocity.magnitude;
        // ����������speed������ֵ�����ƶ������ţ�0=��ֹ��>0=�ƶ���
        animator.SetFloat(speedParamName, currentMoveSpeed);
    }

    // 6. InputActions�������ڹ��������ڴ�й©��
    private void OnEnable()
    {
        // ����Gameplay������ͼ�������������������ղ������룩
        inputActions.Gameplay.Enable();
    }

    private void OnDisable()
    {
        // ����Gameplay������ͼ���ر�����������ͷ���Դ��
        inputActions.Gameplay.Disable();
    }

    // ����ѡ��ʵ�ֽӿڵ��������������InputActions��MeleeAttack/Dodge����ʵ�ֱ��ⱨ��
    public void OnMeleeAttack(InputAction.CallbackContext context) { /* �����ӹ����߼���������д */ }
    public void OnDodge(InputAction.CallbackContext context) { /* �����������߼���������д */ }
}