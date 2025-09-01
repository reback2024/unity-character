using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// �������Text
/// </summary>
public class UICoinCountText : MonoBehaviour
{

    private static TextMeshProUGUI coinCountText;

    private void Awake()
    {
        coinCountText = GetComponent<TextMeshProUGUI>();
    }

    //ˢ���ı�����
    public static void UpdateText(int amount)
    {
        coinCountText.text = amount.ToString();
    }
}
