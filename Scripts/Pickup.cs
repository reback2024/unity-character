using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public AudioClip SFX;
    public GameObject VFX;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PropManager manager = collision.GetComponent<PropManager>();
        if (manager)
        {
            bool pickup = manager.PickupItem(gameObject);
            if (pickup)
            {
                RemoveItem();
            }
        }
    }

    private void RemoveItem()
    {
        AudioSource.PlayClipAtPoint(SFX, transform.position);
        Instantiate(VFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
