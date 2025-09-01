using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public PropPrefab[] dropPrefabs;//储存不同道具的预制体

    //开始生成掉落道具
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

[System.Serializable]//没有MonoBehaviour，想在编译器上显示就加,可序列化
public class PropPrefab
{
    public GameObject prefab;//掉落道具预制品

    [Range(0f, 100f)] public float dropPercentage;//掉落率0到100%


}
