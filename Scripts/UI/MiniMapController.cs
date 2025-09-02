using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MiniMapController : MonoBehaviour
{
    [Header("MiniMap UI")]
    public RectTransform miniMapRect;        // 小地图底图
    public RectTransform playerIcon;         // 玩家图标（中心固定）
    public RectTransform portalIcon;         // 传送门图标
    public RectTransform enemyIconPrefab;    // 敌人图标预制体

    [Header("位置调整")]
    public Vector2 offsetFromCorner = new Vector2(-10, -10); // 离右上角的偏移

    [Header("World References")]
    public Transform player;                     // 玩家对象
    [HideInInspector] // 强制隐藏，禁止在Inspector中赋值，只能通过代码设置
    public Transform portal;                     // 传送门对象（初始为null）
    [HideInInspector]
    public List<Transform> enemies = new List<Transform>();

    [Header("视野与距离设置")]
    public float viewRange = 100f;
    public float maxIndicatorRange = 500f;
    public float iconBorder = 5f;

    [Header("显示设置")]
    public bool debugMode = true;               // 开启调试日志
    public bool useYAxisAsDepth = true;
    [Range(0.1f, 0.5f)]
    public float mapScreenRatio = 0.2f;

    private List<RectTransform> enemyIcons = new List<RectTransform>();
    private float mapHalfWidth;
    private float mapHalfHeight;
    private CanvasScaler canvasScaler;

    // 初始状态强制隐藏图标（比Start更早执行）
    void Awake()
    {
        if (portalIcon != null)
        {
            portalIcon.gameObject.SetActive(false);
            if (debugMode) Debug.Log("Awake: 强制隐藏传送门图标（初始状态）");
        }
        // 确保传送门初始为null（防止意外赋值）
        portal = null;
    }

    void Start()
    {
        canvasScaler = GetComponentInParent<CanvasScaler>();
        UpdateMapSizeAndPosition();

        if (playerIcon != null)
        {
            playerIcon.anchoredPosition = Vector2.zero;
            playerIcon.pivot = new Vector2(0.5f, 0.5f);
        }

        // 再次确认隐藏（双重保险）
        if (portalIcon != null)
        {
            portalIcon.gameObject.SetActive(false);
            if (debugMode) Debug.Log("Start: 再次确认隐藏传送门图标");
        }
    }

    void Update()
    {
        if (player == null) return;

        if (canvasScaler != null && (Screen.width != (int)canvasScaler.referenceResolution.x ||
            Screen.height != (int)canvasScaler.referenceResolution.y))
        {
            UpdateMapSizeAndPosition();
        }

        UpdatePortalIconLogic(); // 严格控制图标的显示/隐藏
        SyncEnemyIcons();
        UpdateAllEnemyIcons();
    }

    // 严格的传送门图标控制逻辑
    void UpdatePortalIconLogic()
    {
        // 图标未赋值 → 直接返回（避免空引用）
        if (portalIcon == null)
        {
            if (debugMode) Debug.LogWarning("portalIcon未在Inspector中赋值！");
            return;
        }

        // 核心判断：只有传送门存在且激活时，才显示图标
        bool shouldShow = portal != null && portal.gameObject.activeInHierarchy;

        // 状态变化时才执行操作（减少冗余）
        if (portalIcon.gameObject.activeSelf != shouldShow)
        {
            portalIcon.gameObject.SetActive(shouldShow);
            if (debugMode)
            {
                if (shouldShow)
                    Debug.Log($"显示传送门图标（传送门已生成，位置: {portal.position}）");
                else
                    Debug.Log("隐藏传送门图标（传送门未生成或已销毁）");
            }
        }

        // 仅在显示状态时更新位置
        if (shouldShow)
        {
            UpdateIconPosition(portalIcon, portal.position);
        }
    }

    void UpdateIconPosition(RectTransform icon, Vector3 worldPos)
    {
        if (icon == null) return;

        icon.pivot = new Vector2(0.5f, 0.5f);

        float offsetX = worldPos.x - player.position.x;
        float offsetY = useYAxisAsDepth ? worldPos.y - player.position.y : worldPos.z - player.position.z;
        Vector2 offsetFromPlayer = new Vector2(offsetX, offsetY);
        float distance = offsetFromPlayer.magnitude;

        if (distance > maxIndicatorRange)
        {
            icon.gameObject.SetActive(false);
            return;
        }

        Vector2 mapPos;
        if (distance <= viewRange)
        {
            mapPos = new Vector2(
                (offsetFromPlayer.x / viewRange) * mapHalfWidth,
                (offsetFromPlayer.y / viewRange) * mapHalfHeight
            );
        }
        else
        {
            Vector2 direction = offsetFromPlayer.normalized;
            mapPos = direction * (mapHalfWidth - iconBorder);
            mapPos.x = Mathf.Clamp(mapPos.x, -mapHalfWidth + iconBorder, mapHalfWidth - iconBorder);
            mapPos.y = Mathf.Clamp(mapPos.y, -mapHalfHeight + iconBorder, mapHalfHeight - iconBorder);
        }

        icon.anchoredPosition = mapPos;
    }

    void UpdateMapSizeAndPosition()
    {
        if (miniMapRect == null) return;

        float baseSize = Screen.width * mapScreenRatio;
        mapHalfWidth = baseSize / 2;
        mapHalfHeight = baseSize / 2;
        miniMapRect.sizeDelta = new Vector2(baseSize, baseSize);

        miniMapRect.anchorMin = new Vector2(1, 1);
        miniMapRect.anchorMax = new Vector2(1, 1);
        miniMapRect.pivot = new Vector2(1, 1);
        miniMapRect.anchoredPosition = offsetFromCorner;
    }

    void UpdateAllEnemyIcons()
    {
        for (int i = 0; i < enemyIcons.Count; i++)
        {
            if (i < enemies.Count && enemies[i] != null)
            {
                enemyIcons[i].gameObject.SetActive(true);
                UpdateIconPosition(enemyIcons[i], enemies[i].position);
            }
            else if (enemyIcons[i] != null)
            {
                enemyIcons[i].gameObject.SetActive(false);
            }
        }
    }

    void SyncEnemyIcons()
    {
        if (enemyIconPrefab == null) return;

        while (enemyIcons.Count < enemies.Count)
        {
            RectTransform newIcon = Instantiate(enemyIconPrefab, miniMapRect);
            enemyIcons.Add(newIcon);
        }

        while (enemyIcons.Count > enemies.Count)
        {
            Destroy(enemyIcons[enemyIcons.Count - 1].gameObject);
            enemyIcons.RemoveAt(enemyIcons.Count - 1);
        }
    }

    // 外部调用：传送门生成时调用此方法
    public void SetPortal(Transform newPortal)
    {
        // 只有当传送门有效时才赋值
        if (newPortal != null && newPortal.gameObject.activeInHierarchy)
        {
            portal = newPortal;
            if (debugMode) Debug.Log($"SetPortal: 传送门已生成（对象名: {newPortal.name}）");
        }
        else
        {
            portal = null;
            if (debugMode) Debug.Log("SetPortal: 传入的传送门无效，不显示图标");
        }
    }

    // 外部调用：传送门销毁时调用
    public void RemovePortal()
    {
        portal = null;
        if (portalIcon != null && portalIcon.gameObject.activeSelf)
        {
            portalIcon.gameObject.SetActive(false);
            if (debugMode) Debug.Log("RemovePortal: 传送门已销毁，隐藏图标");
        }
    }

    public void AddEnemy(Transform enemy)
    {
        if (enemy != null && !enemies.Contains(enemy))
            enemies.Add(enemy);
    }

    public void RemoveEnemy(Transform enemy)
    {
        if (enemies.Contains(enemy))
            enemies.Remove(enemy);
    }

    void OnDrawGizmosSelected()
    {
        if (debugMode && player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.position, viewRange);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(player.position, maxIndicatorRange);
        }
    }
}
