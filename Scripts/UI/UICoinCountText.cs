using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// 金币数量Text
/// </summary>
public class UICoinCountText : MonoBehaviour
{

    private static TextMeshProUGUI coinCountText;

    private void Awake()
    {
        coinCountText = GetComponent<TextMeshProUGUI>();
    }

    //刷新文本内容
    public static void UpdateText(int amount)
    {
        coinCountText.text = amount.ToString();
    }
}
