using UnityEngine;
using Cinemachine;
/// <summary>
/// 摄像机一运行就自动跟随玩家
/// </summary>
public class AutoSetupCamera : MonoBehaviour
{
    Player player;

    private void Awake()
    {
        CinemachineVirtualCamera camera = GetComponent<CinemachineVirtualCamera>();

        player = FindObjectOfType<Player>();//在场景中查找Player 类型的对象实例

        if (player != null)
        {
            camera.Follow = player.transform;
        }
    }
}
