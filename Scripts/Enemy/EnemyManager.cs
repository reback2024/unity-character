using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // ����ģʽ
    public static EnemyManager Instance { get; private set; }

    [Header("����ˢ�µ�")]
    public Transform[] spawnPoints;

    [Header("����Ѳ�ߵ�")]
    public Transform[] patrolPoints;

    [Header("�ùؿ��ĵ���")]
    public List<EnemyWave> enemyWaves;

    [Header("���ͳ���")]
    public SceneExit exit;
    public bool isExitActive; //�жϴ������Ƿ񼤻�

    public int currentWaveIndex = 0; //��ǰ����������
    public int EnemyCount = 0; //��������

    private MiniMapController miniMapController;

    // �Ƿ�Ϊ���һ��
    public bool GetLastWave() => currentWaveIndex == enemyWaves.Count;

    private void Awake()
    {
        Instance = this;
        exit = FindObjectOfType<SceneExit>();

        // ��ȡHUD�ϵ�MiniMapController
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
            // ����������
            if (exit != null)
            {
                exit.gameObject.SetActive(true);
                isExitActive = true;

                // С��ͼ����ʾ������
                if (miniMapController != null)
                {
                    miniMapController.portal = exit.transform;
                }
            }
        }
    }

    // ����������С��ͼͬ��
    IEnumerator startnextwaveCoroutine()
    {
        if (currentWaveIndex >= enemyWaves.Count)
            yield break; //�Ѿ�û�и���Ĳ���

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

                // С��ͼ��ӵ���
                if (miniMapController != null)
                {
                    miniMapController.AddEnemy(enemy.transform);
                }

                EnemyCount++; // ���ӵ��˼���

                // ��������ʱ֪ͨС��ͼ
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    // ����һ��Enemy���͵Ĳ�������������ΪdeadEnemy����ƥ��Action<Enemy>ί��
                    enemyComponent.OnDeath += (deadEnemy) =>
                    {
                        EnemyCount--;
                        if (miniMapController != null)
                        {
                            miniMapController.RemoveEnemy(enemy.transform);
                            // �������Ҫʹ�������ĵ���ʵ�����������ֱ����deadEnemy
                            // ���磺deadEnemy.DoSomething();
                        }
                    };
                }

                yield return new WaitForSeconds(enemyData.spawnIterval);
            }
        }

        currentWaveIndex++;
    }

    // �ӹ���ˢ�µ��λ���б������ѡ��һ��ˢ�µ�
    private Vector3 GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomIndex].position;
    }
}

[System.Serializable]
public class EnemyData
{
    public GameObject enemyPrefab; //����Ԥ����
    public float spawnIterval; //�������ɼ��
    public float waveEnemyConut; //��������
}

[System.Serializable]
public class EnemyWave
{
    public List<EnemyData> enemies; //ÿ�����˵��б�
}