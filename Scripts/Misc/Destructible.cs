using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 可破坏物品
/// </summary>
public class Destructible : MonoBehaviour
{
    public GameObject destroyVFX;//被破坏后的特效

    private PickupSpawner PickupSpawner;

    private void Awake()
    {
        PickupSpawner = GetComponent<PickupSpawner>();
    }


    public void DestroyObject()
    {
        if( destroyVFX != null )
        {
            Instantiate(destroyVFX, transform.position, transform.rotation);
        }

        PickupSpawner.DropItems();//掉落道具

        //销毁
        Destroy(gameObject);
    }
}
