using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject uiShowText;

    //单例模式
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

        DontDestroyOnLoad(gameObject);//加载新场景时,不销毁该对象
    }

    //增加金币
    public void AddCoins(int value)
    {
        coinCount += value;
        UICoinCountText.UpdateText(coinCount);//更新金币文本UI
    }

    //减少金币
    public void RemoveCoins(int value)
    {
        coinCount -= value;
        UICoinCountText.UpdateText(coinCount);//更新金币文本UI
    }

    //提示数值
    public void ShowText(string str, Vector2 pos, Color color)
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(pos);
        GameObject text = Instantiate(uiShowText, screenPosition, Quaternion.identity);
        text.transform.SetParent(GameObject.Find("HUD").transform);//UI 元素必须有Canvas才能显示
        text.GetComponent<UIShowText>().SetText(str, color);

    }
}
