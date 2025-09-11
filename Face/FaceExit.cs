using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // 使用TextMeshPro显示提示，需添加此命名空间

/// <summary>
/// 场景退出点：玩家靠近后需按Y键触发场景切换
/// </summary>
public class FaceExit : MonoBehaviour
{
    [Tooltip("需要过渡的新场景的名称")]
    public string newSceneName;

    [Tooltip("交互提示文本（可在检查器中拖入UI文本组件）")]
    public TextMeshProUGUI interactPrompt; // 用于显示"按Y进入"的提示

    // 标记玩家是否在触发范围内
    private bool isPlayerNear = false;

    private void Start()
    {
        // 移除初始隐藏的代码，让物体一开始就存在
        // 初始隐藏交互提示
        if (interactPrompt != null)
            interactPrompt.gameObject.SetActive(false);
    }

    // 玩家进入触发器范围时
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true; // 标记玩家在附近
            // 显示交互提示
            if (interactPrompt != null)
                interactPrompt.gameObject.SetActive(true);
        }
    }

    // 玩家离开触发器范围时
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false; // 标记玩家离开
            // 隐藏交互提示
            if (interactPrompt != null)
                interactPrompt.gameObject.SetActive(false);
        }
    }

    // 每帧检测输入
    private void Update()
    {
        // 只有玩家在附近且按下Y键时，才触发场景切换
        if (isPlayerNear && Input.GetKeyDown(KeyCode.Y))
        {
            TransitionInternal();
        }
    }

    // 场景切换函数
    public void TransitionInternal()
    {
        Sceneloader.Instance.TransitionToString(newSceneName);
    }
}
