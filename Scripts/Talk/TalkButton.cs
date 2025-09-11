using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkButton : MonoBehaviour
{
    public GameObject Button;
    public GameObject talkUI;
    
    private void Awake()
    {
        //Debug.Log("1");
        Button.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Button.SetActive(true); // 仅当碰撞Player时激活按钮 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Button.SetActive(false);
    }

    private void Update()
    {
        if (Button.activeSelf && Input.GetKeyDown(KeyCode.R))
        {
            Player.Instance.CanotMove();

            {
                talkUI.SetActive(true);
                Debug.Log("111111");
            }


            //// 递归检查所有父对象的激活状态
            //Transform current = talkUI.transform;
            //while (current.parent != null)
            //{
            //    current = current.parent;
            //    Debug.Log($"父对象 {current.name} 激活状态: {current.gameObject.activeSelf}");
            //}
        }
    }

}
