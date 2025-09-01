using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ���ƻ���Ʒ
/// </summary>
public class Destructible : MonoBehaviour
{
    public GameObject destroyVFX;//���ƻ������Ч

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

        PickupSpawner.DropItems();//�������

        //����
        Destroy(gameObject);
    }
}
