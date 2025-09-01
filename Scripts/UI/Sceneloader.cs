using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sceneloader : MonoBehaviour
{
    //单例模式，并且不销毁
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

        DontDestroyOnLoad(gameObject);//加载新场景的时不要销毁该游戏对象
    }

    //加载游戏场景
    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
