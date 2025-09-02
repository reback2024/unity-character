using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �����˳�
/// </summary>
public class SceneExit : MonoBehaviour
{
    [Tooltip("��Ҫ���ɵ��³���������")]
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

    //�����л�����
    public void TransitionInternal()
    {
        Sceneloader.Instance.TransitionToString(newSceneName);
    }
}

