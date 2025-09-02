using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject uiShowText;

    //单例模式
    public static GameManager Instance { get; private set; }

    public int coinCount {  get; private set; }

    public float PlayerCurrentHealth {  get; private set; }

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

    public void ChangeCoins(int amount)
    {
        coinCount += amount;
        if (coinCount < 0) coinCount = 0;
        UICoinCountText.UpdateText(coinCount);
    }

    //提示数值
    public void ShowText(string str, Vector2 pos, Color color)
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(pos);
        GameObject text = Instantiate(uiShowText, screenPosition, Quaternion.identity);
        text.transform.SetParent(GameObject.Find("HUD").transform);//UI 元素必须有Canvas才能显示
        text.GetComponent<UIShowText>().SetText(str, color);

    }

    //保存数据
    public void SaveData()
    {
        PlayerCurrentHealth = Player.Instance.curHealth;
    }

    //加载数据
    public void LoadData()
    {
        Player.Instance.curHealth = PlayerCurrentHealth;
        UICoinCountText.UpdateText(coinCount);
    }
}
