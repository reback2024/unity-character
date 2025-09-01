using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
/// <summary>
/// UI浮动文字效果
/// </summary>
public class UIShowText : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        //DOTween动画效果
        transform.DOMoveY(transform.position.y + 20, 0.5f);
        transform.DOScale(transform.localScale * 2, 0.2f);
        Destroy(gameObject, 0.6f);
    }

    //设置文本内容
    public void SetText(string str,Color color)
    {
        text.text = str;
        text.color = color;
    }
}
