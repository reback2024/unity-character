using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sceneloader : MonoBehaviour
{
    //����ģʽ�����Ҳ�����
    public static Sceneloader Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);//�����³�����ʱ��Ҫ���ٸ���Ϸ����
    }

    //������Ϸ����
    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
