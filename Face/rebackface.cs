using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class rebackface : MonoBehaviour
{
    private void Update()
    {
        // 当按下ESC键时
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 加载名为"House"的场景
            SceneManager.LoadScene("House");
        }
    }
}
