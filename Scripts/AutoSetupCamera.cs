using UnityEngine;
using Cinemachine;
/// <summary>
/// �����һ���о��Զ��������
/// </summary>
public class AutoSetupCamera : MonoBehaviour
{
    Player player;

    private void Awake()
    {
        CinemachineVirtualCamera camera = GetComponent<CinemachineVirtualCamera>();

        player = FindObjectOfType<Player>();//�ڳ����в���Player ���͵Ķ���ʵ��

        if (player != null)
        {
            camera.Follow = player.transform;
        }
    }
}
