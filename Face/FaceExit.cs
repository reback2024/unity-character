using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // ʹ��TextMeshPro��ʾ��ʾ������Ӵ������ռ�

/// <summary>
/// �����˳��㣺��ҿ������谴Y�����������л�
/// </summary>
public class FaceExit : MonoBehaviour
{
    [Tooltip("��Ҫ���ɵ��³���������")]
    public string newSceneName;

    [Tooltip("������ʾ�ı������ڼ����������UI�ı������")]
    public TextMeshProUGUI interactPrompt; // ������ʾ"��Y����"����ʾ

    // �������Ƿ��ڴ�����Χ��
    private bool isPlayerNear = false;

    private void Start()
    {
        // �Ƴ���ʼ���صĴ��룬������һ��ʼ�ʹ���
        // ��ʼ���ؽ�����ʾ
        if (interactPrompt != null)
            interactPrompt.gameObject.SetActive(false);
    }

    // ��ҽ��봥������Χʱ
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true; // �������ڸ���
            // ��ʾ������ʾ
            if (interactPrompt != null)
                interactPrompt.gameObject.SetActive(true);
        }
    }

    // ����뿪��������Χʱ
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false; // �������뿪
            // ���ؽ�����ʾ
            if (interactPrompt != null)
                interactPrompt.gameObject.SetActive(false);
        }
    }

    // ÿ֡�������
    private void Update()
    {
        // ֻ������ڸ����Ұ���Y��ʱ���Ŵ��������л�
        if (isPlayerNear && Input.GetKeyDown(KeyCode.Y))
        {
            TransitionInternal();
        }
    }

    // �����л�����
    public void TransitionInternal()
    {
        Sceneloader.Instance.TransitionToString(newSceneName);
    }
}
