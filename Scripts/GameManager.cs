using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject uiShowText;

    //����ģʽ
    public static GameManager Instance { get; private set; }

    private int coinCount;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);//�����³���ʱ,�����ٸö���
    }

    //���ӽ��
    public void AddCoins(int value)
    {
        coinCount += value;
        UICoinCountText.UpdateText(coinCount);//���½���ı�UI
    }

    //���ٽ��
    public void RemoveCoins(int value)
    {
        coinCount -= value;
        UICoinCountText.UpdateText(coinCount);//���½���ı�UI
    }

    //��ʾ��ֵ
    public void ShowText(string str, Vector2 pos, Color color)
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(pos);
        GameObject text = Instantiate(uiShowText, screenPosition, Quaternion.identity);
        text.transform.SetParent(GameObject.Find("HUD").transform);//UI Ԫ�ر�����Canvas������ʾ
        text.GetComponent<UIShowText>().SetText(str, color);

    }
}
