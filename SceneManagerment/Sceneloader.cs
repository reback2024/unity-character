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

    //�л���Ϸ��������
    public void TransitionToString(string sceneName)
    {
        StartCoroutine(TransitionCoroutine(sceneName));
    }

    //�л�����Э��
    public IEnumerator TransitionCoroutine(string newSceneName)
    {

        //Debug.Log("Sceneloader ׼������ GameManager.Instance: " + GameManager.Instance); // �ؼ���־
        //if (GameManager.Instance == null)
        //{
        //    Debug.LogError("GameManager.Instance Ϊ null���޷����� SaveData()");
        //    yield break; // ��ʱ��ֹЭ�̣��������
        //}

        //�������г־û�����
        GameManager.Instance.SaveData();

        //������ǰ����
        yield return StartCoroutine(ScreenFader.instance.FadeSceneOut());

        //�첽�����³���
        yield return SceneManager.LoadSceneAsync(newSceneName);

        //�������г־û�����
        GameManager.Instance.LoadData();

        //��ȡĿ�곡�����ɵ�λ��
        SceneEntrane entrance = FindObjectOfType<SceneEntrane>();

        //���ý�����Ϸ�����λ��
        SetEnteringPosition(entrance);

        //�����³���
        yield return StartCoroutine(ScreenFader.instance.FadeSceneIn());
    }

    private void SetEnteringPosition(SceneEntrane entrance)
    {
        if (entrance == null)
            return;
        //��Ŀ�곡������λ�ø�ֵ�����
        Transform entanceTransform = entrance.transform;
        Player.Instance.transform.position = entanceTransform.position;
    }
}
