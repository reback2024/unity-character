using UnityEngine;
using UnityEngine.InputSystem;

// 确保角色 GameObject 自动添加必要组件
[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class RoleMove : MonoBehaviour, InputActions.IGameplayActions
{
    [Header("移动配置")]
    [Tooltip("角色移动速度（可在Inspector调整）")]
    public float moveSpeed = 3.5f;

    [Header("动画器配置")]
    [Tooltip("动画器中控制移动状态的Float参数名（默认填'speed'）")]
    public string speedParamName = "speed";

    // 1. 核心组件引用
    private Rigidbody2D rb;          // 2D刚体（控制物理移动）
    private Animator animator;      // 动画器（控制动画切换）
    private SpriteRenderer sr;      // 精灵渲染器（控制角色左右翻转）

    // 2. InputActions实例（绑定你的自动生成类）
    private InputActions inputActions;
    // 缓存移动输入（从InputActions的Move动作读取）
    private Vector2 moveInput;

    private void Awake()
    {
        // 初始化组件引用（自动获取，无需手动拖赋值）
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        // 初始化InputActions：绑定你自动生成的InputActions类
        inputActions = new InputActions();
        // 将当前脚本注册为Gameplay动作的回调接收者（关键！否则OnMove不触发）
        inputActions.Gameplay.SetCallbacks(this);

    }

    // 3. 输入回调（实现InputActions.IGameplayActions接口的OnMove方法）
    // 当Move动作有输入（WASD/方向键/手柄左摇杆）时自动触发
    public void OnMove(InputAction.CallbackContext context)
    {
        // 读取Move动作的输入值（Vector2类型，x=左右，y=上下）
        moveInput = context.ReadValue<Vector2>();
    }

    // 4. 物理移动（放在FixedUpdate，与物理引擎同步，避免帧率波动影响移动）
    private void FixedUpdate()
    {
        // 计算最终移动速度（输入方向 × 移动速度）
        Vector2 finalMovement = moveInput * moveSpeed;
        // 应用移动到刚体（控制角色实际位移）
        rb.velocity = finalMovement;

        // 控制角色左右翻转（根据x轴输入方向）
        if (moveInput.x != 0)  // 只有有左右输入时才翻转
        {
            // x>0：向右 → 不翻转；x<0：向左 → 翻转X轴
            sr.flipX = moveInput.x < 0;
        }
    }

    // 5. 动画器更新（放在Update，与帧同步，动画过渡更流畅）
    private void Update()
    {
        // 计算当前角色的实际移动速度大小（忽略方向，只看“快慢”）
        float currentMoveSpeed = rb.velocity.magnitude;
        // 给动画器的speed参数赋值（控制动画播放：0=静止，>0=移动）
        animator.SetFloat(speedParamName, currentMoveSpeed);
    }

    // 6. InputActions生命周期管理（避免内存泄漏）
    private void OnEnable()
    {
        // 启用Gameplay动作地图（打开输入监听，否则接收不到输入）
        inputActions.Gameplay.Enable();
    }

    private void OnDisable()
    {
        // 禁用Gameplay动作地图（关闭输入监听，释放资源）
        inputActions.Gameplay.Disable();
    }

    // （可选）实现接口的其他方法（你的InputActions有MeleeAttack/Dodge，空实现避免报错）
    public void OnMeleeAttack(InputAction.CallbackContext context) { /* 后续加攻击逻辑可在这里写 */ }
    public void OnDodge(InputAction.CallbackContext context) { /* 后续加闪避逻辑可在这里写 */ }
}