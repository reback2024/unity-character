using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [Range(0f,2f)]public float defauitTimeScale=1;//默认游戏时间速度

    [Header("子弹时间")]
    [SerializeField, Range(0f, 2f)] float bulletTimeScale;//子弹时间速度
    [SerializeField]private float timeRecoverDuration;//过渡回默认游戏时间的持续时间

    private GUIStyle labStyle;
    private void Awake()
    {
        Time.timeScale = defauitTimeScale;
    }
    private void Start()
    {
        labStyle = new GUIStyle();
        labStyle.fontSize = 120;
        labStyle.normal.textColor = Color.white;
    }//用于数据测试

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 80),"ScaleTime=" + Time.timeScale, labStyle);
    }

    public void BulletTime()
    {
        Time.timeScale = bulletTimeScale;
        StartCoroutine(nameof(TimeRecoveryCoroutine));
    }
    
    //恢复默认时间的协程
    IEnumerator TimeRecoveryCoroutine()
    {
        float ration = 0f;
        while(ration<1f)
        {
            ration += Time.unscaledDeltaTime / timeRecoverDuration;

            Time.timeScale = Mathf.Lerp(bulletTimeScale, defauitTimeScale, ration);

            yield return null;
        }
    }
}
