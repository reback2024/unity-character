using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item",menuName ="Inventory/New Item")]
public class Item : ScriptableObject
{
    public string iteamName;
    public Sprite itemImage;
    public int iteamHeld;
    [TextArea]
    public string itemInfo;

    public bool equip;

}
