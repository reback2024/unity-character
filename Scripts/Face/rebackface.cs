using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class rebackface : MonoBehaviour
{
    private void Update()
    {
        // ������ESC��ʱ
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ������Ϊ"House"�ĳ���
            SceneManager.LoadScene("House");
        }
    }
}
