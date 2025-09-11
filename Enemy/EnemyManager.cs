using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // 单例模式
    public static EnemyManager Instance { get; private set; }

    [Header("敌人刷新点")]
    public Transform[] spawnPoints;

    [Header("敌人巡逻点")]
    public Transform[] patrolPoints;

    [Header("该关卡的敌人")]
    public List<EnemyWave> enemyWaves;

    [Header("传送出口")]
    public SceneExit exit;
    public bool isExitActive; //判断传送门是否激活

    public int currentWaveIndex = 0; //当前波数的索引
    public int EnemyCount = 0; //敌人数量

    private MiniMapController miniMapController;

    // 是否为最后一波
    public bool GetLastWave() => currentWaveIndex == enemyWaves.Count;

    private void Awake()
    {
        Instance = this;
        exit = FindObjectOfType<SceneExit>();

        // 获取HUD上的MiniMapController
        miniMapController = FindObjectOfType<MiniMapController>();
    }

    private void Update()
    {
        if (EnemyCount == 0 && !GetLastWave())
        {
            StartCoroutine(nameof(startnextwaveCoroutine));
        }
        else if (EnemyCount == 0 && GetLastWave() && !isExitActive)
        {
            // 开启传送门
            if (exit != null)
            {
                exit.gameObject.SetActive(true);
                isExitActive = true;

                // 小地图上显示传送门
                if (miniMapController != null)
                {
                    miniMapController.portal = exit.transform;
                }
            }
        }
    }

    // 敌人生成与小地图同步
    IEnumerator startnextwaveCoroutine()
    {
        if (currentWaveIndex >= enemyWaves.Count)
            yield break; //已经没有更多的波数

        List<EnemyData> enemies = enemyWaves[currentWaveIndex].enemies;
        foreach (EnemyData enemyData in enemies)
        {
            for (int i = 0; i < enemyData.waveEnemyConut; i++)
            {
                GameObject enemy = Instantiate(enemyData.enemyPrefab, GetRandomSpawnPoint(), Quaternion.identity);

                if (patrolPoints != null)
                {
                    enemy.GetComponent<Enemy>().patrolPoints = patrolPoints;
                }

                // 小地图添加敌人
                if (miniMapController != null)
                {
                    miniMapController.AddEnemy(enemy.transform);
                }

                EnemyCount++; // 增加敌人计数

                // 敌人死亡时通知小地图
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    // 增加一个Enemy类型的参数（例如命名为deadEnemy），匹配Action<Enemy>委托
                    enemyComponent.OnDeath += (deadEnemy) =>
                    {
                        EnemyCount--;
                        if (miniMapController != null)
                        {
                            miniMapController.RemoveEnemy(enemy.transform);
                            // 如果你需要使用死亡的敌人实例，这里可以直接用deadEnemy
                            // 例如：deadEnemy.DoSomething();
                        }
                    };
                }

                yield return new WaitForSeconds(enemyData.spawnIterval);
            }
        }

        currentWaveIndex++;
    }

    // 从怪物刷新点的位置列表中随机选择一个刷新点
    private Vector3 GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomIndex].position;
    }
}

[System.Serializable]
public class EnemyData
{
    public GameObject enemyPrefab; //敌人预制体
    public float spawnIterval; //怪物生成间隔
    public float waveEnemyConut; //敌人数量
}

[System.Serializable]
public class EnemyWave
{
    public List<EnemyData> enemies; //每波敌人的列表
}