using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader instance {  get; private set; }

    public CanvasGroup faderCanvasGroup;

    public float fadeeDuration = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    //���볡��
    public IEnumerator FadeSceneIn()
    {
        yield return StartCoroutine(Fade(0f,faderCanvasGroup));
        faderCanvasGroup.gameObject.SetActive(false);
    }

    //��������
    public IEnumerator FadeSceneOut()
    {
        faderCanvasGroup.gameObject.SetActive(true);
        yield return StartCoroutine(Fade(1f, faderCanvasGroup));
    }

    //���뵭��ʵ��
    public IEnumerator Fade(float finalAlpja,CanvasGroup canvasGroup)
    {
        yield return canvasGroup.DOFade(finalAlpja,fadeeDuration).WaitForCompletion();
    }
}
