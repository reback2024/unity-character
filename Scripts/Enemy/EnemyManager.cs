using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //单例模式
    public static EnemyManager Instance { get; private set; }

    [Header("敌人刷新点")]
    public Transform[] spawnPoints;

    [Header("敌人巡逻点")]
    public Transform[] patrolPoints;

    [Header("该关卡的敌人")]
    public List<EnemyWave> enemyWaves;

    public int currentWaveIndex = 0;//当前波数的索引

    public int EnemyCount = 0;//敌人数量

    //是否为最后一波
    public bool GetLastWave() => currentWaveIndex == enemyWaves.Count;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (EnemyCount == 0)
        {
            StartCoroutine(nameof(startnextwaveCoroutine));
        }
    }

    IEnumerator startnextwaveCoroutine()
    {
        if (currentWaveIndex >= enemyWaves.Count) 
            yield break;//已经没有更多的波数

        List<EnemyData> enemies = enemyWaves[currentWaveIndex].enemies;
        foreach (EnemyData enemyData in enemies)
        {
            for(int i=0;i<enemyData.waveEnemyConut;i++)
            {
                GameObject enemy = Instantiate(enemyData.enemyPrefab, GetRandomSpawnPoint(), Quaternion.identity);

                if (patrolPoints != null)
                {
                    enemy.GetComponent<Enemy>().patrolPoints = patrolPoints;
                }

                yield return new WaitForSeconds(enemyData.spawnIterval);
            }
        }

        currentWaveIndex++;
    }

    //从怪物刷新点的位置列表中随机选择一个刷新点
    private Vector3 GetRandomSpawnPoint()
    {
        int randomIndex=Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomIndex].position;
    }

}

[System.Serializable]
public class EnemyData
{
    public GameObject enemyPrefab;//敌人预制体
    public float spawnIterval;//怪物生成间隔
    public float waveEnemyConut;//敌人数量
}

[System.Serializable]
public class EnemyWave
{
    public List<EnemyData> enemies;//每波敌人的列表
}
