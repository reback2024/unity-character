
using UnityEngine;
using UnityEngine.EventSystems;

public class xx : MonoBehaviour
{
    public void OnPointerClick()
    {

        Player.Instance._changeOpen();
        Debug.Log("UI 点击成功，inOpen 已设置为 0");

    }
}