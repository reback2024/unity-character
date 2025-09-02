using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MiniMapController : MonoBehaviour
{
    [Header("MiniMap UI")]
    public RectTransform miniMapRect;        // С��ͼ��ͼ
    public RectTransform playerIcon;         // ���ͼ�꣨���Ĺ̶���
    public RectTransform portalIcon;         // ������ͼ��
    public RectTransform enemyIconPrefab;    // ����ͼ��Ԥ����

    [Header("λ�õ���")]
    public Vector2 offsetFromCorner = new Vector2(-10, -10); // �����Ͻǵ�ƫ��

    [Header("World References")]
    public Transform player;                     // ��Ҷ���
    [HideInInspector] // ǿ�����أ���ֹ��Inspector�и�ֵ��ֻ��ͨ����������
    public Transform portal;                     // �����Ŷ��󣨳�ʼΪnull��
    [HideInInspector]
    public List<Transform> enemies = new List<Transform>();

    [Header("��Ұ���������")]
    public float viewRange = 100f;
    public float maxIndicatorRange = 500f;
    public float iconBorder = 5f;

    [Header("��ʾ����")]
    public bool debugMode = true;               // ����������־
    public bool useYAxisAsDepth = true;
    [Range(0.1f, 0.5f)]
    public float mapScreenRatio = 0.2f;

    private List<RectTransform> enemyIcons = new List<RectTransform>();
    private float mapHalfWidth;
    private float mapHalfHeight;
    private CanvasScaler canvasScaler;

    // ��ʼ״̬ǿ������ͼ�꣨��Start����ִ�У�
    void Awake()
    {
        if (portalIcon != null)
        {
            portalIcon.gameObject.SetActive(false);
            if (debugMode) Debug.Log("Awake: ǿ�����ش�����ͼ�꣨��ʼ״̬��");
        }
        // ȷ�������ų�ʼΪnull����ֹ���⸳ֵ��
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

        // �ٴ�ȷ�����أ�˫�ر��գ�
        if (portalIcon != null)
        {
            portalIcon.gameObject.SetActive(false);
            if (debugMode) Debug.Log("Start: �ٴ�ȷ�����ش�����ͼ��");
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

        UpdatePortalIconLogic(); // �ϸ����ͼ�����ʾ/����
        SyncEnemyIcons();
        UpdateAllEnemyIcons();
    }

    // �ϸ�Ĵ�����ͼ������߼�
    void UpdatePortalIconLogic()
    {
        // ͼ��δ��ֵ �� ֱ�ӷ��أ���������ã�
        if (portalIcon == null)
        {
            if (debugMode) Debug.LogWarning("portalIconδ��Inspector�и�ֵ��");
            return;
        }

        // �����жϣ�ֻ�д����Ŵ����Ҽ���ʱ������ʾͼ��
        bool shouldShow = portal != null && portal.gameObject.activeInHierarchy;

        // ״̬�仯ʱ��ִ�в������������ࣩ
        if (portalIcon.gameObject.activeSelf != shouldShow)
        {
            portalIcon.gameObject.SetActive(shouldShow);
            if (debugMode)
            {
                if (shouldShow)
                    Debug.Log($"��ʾ������ͼ�꣨�����������ɣ�λ��: {portal.position}��");
                else
                    Debug.Log("���ش�����ͼ�꣨������δ���ɻ������٣�");
            }
        }

        // ������ʾ״̬ʱ����λ��
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

    // �ⲿ���ã�����������ʱ���ô˷���
    public void SetPortal(Transform newPortal)
    {
        // ֻ�е���������Чʱ�Ÿ�ֵ
        if (newPortal != null && newPortal.gameObject.activeInHierarchy)
        {
            portal = newPortal;
            if (debugMode) Debug.Log($"SetPortal: �����������ɣ�������: {newPortal.name}��");
        }
        else
        {
            portal = null;
            if (debugMode) Debug.Log("SetPortal: ����Ĵ�������Ч������ʾͼ��");
        }
    }

    // �ⲿ���ã�����������ʱ����
    public void RemovePortal()
    {
        portal = null;
        if (portalIcon != null && portalIcon.gameObject.activeSelf)
        {
            portalIcon.gameObject.SetActive(false);
            if (debugMode) Debug.Log("RemovePortal: �����������٣�����ͼ��");
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
