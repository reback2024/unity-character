using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 场景退出
/// </summary>
public class SceneExit : MonoBehaviour
{
    [Tooltip("需要过渡的新场景的名称")]
    public string newSceneName;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            TransitionInternal();
        }
    }

    //场景切换函数
    public void TransitionInternal()
    {
        Sceneloader.Instance.TransitionToString(newSceneName);
    }
}

