
using UnityEngine;
using UnityEngine.EventSystems;

public class xx : MonoBehaviour
{
    public void OnPointerClick()
    {

        Player.Instance._changeOpen();
        Debug.Log("UI ����ɹ���inOpen ������Ϊ 0");

    }
}