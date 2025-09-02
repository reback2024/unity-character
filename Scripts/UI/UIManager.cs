using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {  get; private set; }

    [Header("UI组件")]
    public GameObject gameOverPanel;
    public GameObject gamePassPanel;

    public Button btnRestart;//重新开始
    public Button btnContinue;//继续
    public Slider healthSlider;//血量
    public Slider dodgecooldownSlider;//闪避冷却条 

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //给按钮添加事件监听
        btnRestart.onClick.AddListener(OnRestartButtonClick);
        btnContinue.onClick.AddListener(OnRestartButtonClick);//这里还没做下一关，先结束
    }

    //血量条UI
    public void UpdateHealthSlider(float maxHealth, float currentHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value= currentHealth;
    }

    //闪避冷却条UI
    public void DodegCooldownSlider(float cooldownTime)
    {
        StartCoroutine(UpdateCooldownCoroutine(cooldownTime));
    }
    //过渡效果
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

    //游戏结束
    public void showGameOvetPanel()
    {
        gameOverPanel.SetActive(true);
    }

    //重新开始 
    public void OnRestartButtonClick()
    {
        //重新加载场景
        //Sceneloader.Instance.LoadMainScene();
    }
}
