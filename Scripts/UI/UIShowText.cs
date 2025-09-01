using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
/// <summary>
/// UI��������Ч��
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
        //DOTween����Ч��
        transform.DOMoveY(transform.position.y + 20, 0.5f);
        transform.DOScale(transform.localScale * 2, 0.2f);
        Destroy(gameObject, 0.6f);
    }

    //�����ı�����
    public void SetText(string str,Color color)
    {
        text.text = str;
        text.color = color;
    }
}
