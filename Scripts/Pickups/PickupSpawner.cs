using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public PropPrefab[] dropPrefabs;//���治ͬ���ߵ�Ԥ����

    //��ʼ���ɵ������
    public void DropItems()
    {
        foreach(var propPrefab in dropPrefabs)
        {
            if (Random.Range(0f, 100f) <= propPrefab.dropPercentage)
            {
                Instantiate(propPrefab.prefab, transform.position, Quaternion.identity);
            }
        }
    }
}

[System.Serializable]//û��MonoBehaviour�����ڱ���������ʾ�ͼ�,�����л�
public class PropPrefab
{
    public GameObject prefab;//�������Ԥ��Ʒ

    [Range(0f, 100f)] public float dropPercentage;//������0��100%


}
