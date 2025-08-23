using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    public int coinCount;
    public int equipmentCount;
    public int keyCount;
    public int potionCount;
    
    public bool PickupItem(GameObject obj)
    {
        switch(obj.tag)
        {
            case Constants.coin:
                PickUpCoin();
                return true;
            case Constants.equipment:
                PickUpEquipment();
                return true;
            case Constants.key:
                PickUpKey();
                return true;
            case Constants.potion:
                PickUpPotion();
                return true;
            default:
                Debug.Log("≤ªø… ∞»°");
                return false;
        }
    }
    private void OnGUI()
    {
        GUI.skin.label.fontSize = 50;
        GUI.Label(new Rect(20, 20, 500, 500), "Coin Num" + coinCount);
        GUI.Label(new Rect(20, 100, 500, 500), "Equipment Num" + equipmentCount);
        GUI.Label(new Rect(20, 180, 500, 500), "Key Num" + keyCount);
        GUI.Label(new Rect(20, 260, 500, 500), "Potion Num" + potionCount);
    }
    private void PickUpCoin()
    {
        coinCount++;
    }
    private void PickUpEquipment()
    {
        equipmentCount++;
    }
    private void PickUpKey()
    {
        keyCount++;
    }
    private void PickUpPotion()
    {
        potionCount++;
    }

}
