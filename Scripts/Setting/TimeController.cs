using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [Range(0f,2f)]public float defauitTimeScale=1;//Ĭ����Ϸʱ���ٶ�

    [Header("�ӵ�ʱ��")]
    [SerializeField, Range(0f, 2f)] float bulletTimeScale;//�ӵ�ʱ���ٶ�
    [SerializeField]private float timeRecoverDuration;//���ɻ�Ĭ����Ϸʱ��ĳ���ʱ��

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
    }//�������ݲ���

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 80),"ScaleTime=" + Time.timeScale, labStyle);
    }

    public void BulletTime()
    {
        Time.timeScale = bulletTimeScale;
        StartCoroutine(nameof(TimeRecoveryCoroutine));
    }
    
    //�ָ�Ĭ��ʱ���Э��
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
