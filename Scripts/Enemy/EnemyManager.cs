using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //����ģʽ
    public static EnemyManager Instance { get; private set; }

    [Header("����ˢ�µ�")]
    public Transform[] spawnPoints;

    [Header("����Ѳ�ߵ�")]
    public Transform[] patrolPoints;

    [Header("�ùؿ��ĵ���")]
    public List<EnemyWave> enemyWaves;

    public int currentWaveIndex = 0;//��ǰ����������

    public int EnemyCount = 0;//��������

    //�Ƿ�Ϊ���һ��
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
            yield break;//�Ѿ�û�и���Ĳ���

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

    //�ӹ���ˢ�µ��λ���б������ѡ��һ��ˢ�µ�
    private Vector3 GetRandomSpawnPoint()
    {
        int randomIndex=Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomIndex].position;
    }

}

[System.Serializable]
public class EnemyData
{
    public GameObject enemyPrefab;//����Ԥ����
    public float spawnIterval;//�������ɼ��
    public float waveEnemyConut;//��������
}

[System.Serializable]
public class EnemyWave
{
    public List<EnemyData> enemies;//ÿ�����˵��б�
}
