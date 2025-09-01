using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI���")]
    public GameObject gameOverPanel;
    public GameObject gamePassPanel;

    public Button btnRestart;//���¿�ʼ
    public Button btnContinue;//����
    public Slider healthSlider;//Ѫ��
    public Slider dodgecooldownSlider;//������ȴ�� 
    // Start is called before the first frame update
    void Start()
    {
        //����ť����¼�����
        btnRestart.onClick.AddListener(OnRestartButtonClick);
        btnContinue.onClick.AddListener(OnRestartButtonClick);//���ﻹû����һ�أ��Ƚ���
    }

    // Update is called once per frame
    void Update()
    {
        //ͨ������
        if (EnemyManager.Instance.GetLastWave() && EnemyManager.Instance.EnemyCount == 0)
        {
            gamePassPanel.SetActive(true);
        }
    }

    //Ѫ����UI
    public void UpdateHealthSlider(float maxHealth, float currentHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value= currentHealth;
    }

    //������ȴ��UI
    public void DodegCooldownSlider(float cooldownTime)
    {
        StartCoroutine(UpdateCooldownCoroutine(cooldownTime));
    }
    //����Ч��
    public IEnumerator UpdateCooldownCoroutine(float cooldownTime)
    {
        dodgecooldownSlider.maxValue = cooldownTime;
        dodgecooldownSlider.value = cooldownTime;

        float elapsegTime = 0f;

        while(elapsegTime<cooldownTime)
        {
            dodgecooldownSlider.value = elapsegTime;

            elapsegTime += Time.deltaTime;
            yield return null;
        }
    }

    //��Ϸ����
    public void showGameOvetPanel()
    {
        gameOverPanel.SetActive(true);
    }

    //���¿�ʼ 
    public void OnRestartButtonClick()
    {
        //���¼��س���
        Sceneloader.Instance.LoadMainScene();
    }
}
