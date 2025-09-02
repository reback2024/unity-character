using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject uiShowText;

    //����ģʽ
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

        DontDestroyOnLoad(gameObject);//�����³���ʱ,�����ٸö���
    }

    public void ChangeCoins(int amount)
    {
        coinCount += amount;
        if (coinCount < 0) coinCount = 0;
        UICoinCountText.UpdateText(coinCount);
    }

    //��ʾ��ֵ
    public void ShowText(string str, Vector2 pos, Color color)
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(pos);
        GameObject text = Instantiate(uiShowText, screenPosition, Quaternion.identity);
        text.transform.SetParent(GameObject.Find("HUD").transform);//UI Ԫ�ر�����Canvas������ʾ
        text.GetComponent<UIShowText>().SetText(str, color);

    }

    //��������
    public void SaveData()
    {
        PlayerCurrentHealth = Player.Instance.curHealth;
    }

    //��������
    public void LoadData()
    {
        Player.Instance.curHealth = PlayerCurrentHealth;
        UICoinCountText.UpdateText(coinCount);
    }
}
