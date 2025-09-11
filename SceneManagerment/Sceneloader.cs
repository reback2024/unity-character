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

    //切换游戏场景函数
    public void TransitionToString(string sceneName)
    {
        StartCoroutine(TransitionCoroutine(sceneName));
    }

    //切换场景协程
    public IEnumerator TransitionCoroutine(string newSceneName)
    {

        //Debug.Log("Sceneloader 准备访问 GameManager.Instance: " + GameManager.Instance); // 关键日志
        //if (GameManager.Instance == null)
        //{
        //    Debug.LogError("GameManager.Instance 为 null！无法调用 SaveData()");
        //    yield break; // 临时终止协程，避免崩溃
        //}

        //保存所有持久化数据
        GameManager.Instance.SaveData();

        //淡出当前场景
        yield return StartCoroutine(ScreenFader.instance.FadeSceneOut());

        //异步加载新场景
        yield return SceneManager.LoadSceneAsync(newSceneName);

        //加载所有持久化数据
        GameManager.Instance.LoadData();

        //获取目标场景过渡的位置
        SceneEntrane entrance = FindObjectOfType<SceneEntrane>();

        //设置进入游戏对象的位置
        SetEnteringPosition(entrance);

        //淡入新场景
        yield return StartCoroutine(ScreenFader.instance.FadeSceneIn());
    }

    private void SetEnteringPosition(SceneEntrane entrance)
    {
        if (entrance == null)
            return;
        //把目标场景过渡位置赋值给玩家
        Transform entanceTransform = entrance.transform;
        Player.Instance.transform.position = entanceTransform.position;
    }
}
