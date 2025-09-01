using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// ʰȡ����
/// </summary>
public class Pickup : MonoBehaviour
{
    //����Pickup������
    public enum PickupType
    {
        Coin,
        HealingPotion
    }

    [Header("��������")]
    [SerializeField] private PickupType pickupType;          //����Pickup������
    [SerializeField] private int value;

    [Header("���׶���")]
    [SerializeField] private float throwHeight = 1f;
    [SerializeField] private float throwDuration = 1f;

    [Header("ʰȡ��Χ")]
    [SerializeField] private float pickUpDistance = 3f;    //�Զ�ʰȡ��Χ
    [SerializeField] private float moveSpeed = 5f;       //�Զ�ʰȡ�ٶ�
    private bool canPickUp = false;     //�Ƿ��ʰȡ

    private Player player;//��ҵ�����

    private void Awake()
    {
        player = FindObjectOfType<Player>();//���ҳ����е�Player���󲢸�ֵ��player����
    }
    private void Start()
    {
        ThrowItem();
    }
    private void Update()
    {
        if (canPickUp && Vector2.Distance(transform.position, player.transform.position) < pickUpDistance)
        {
            Vector2 dir = (player.transform.position - transform.position).normalized;
            transform.Translate(dir * moveSpeed * Time.deltaTime);
        }
    }

    //���׶���
    private void ThrowItem()
    {

        //ʹ��DOTween��������
        transform.DOJump(transform.position, throwHeight, 1, throwDuration)
        .OnComplete(() =>
        {
            canPickUp = true;
        });
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (canPickUp && collision.gameObject.GetComponent<Player>())
        {
            CollectPickup();
        }
    }

    //����������ִ�в�ͬ���߼�
    private void CollectPickup()
    {
        switch (pickupType)
        {
            case PickupType.Coin:
                //�������߼�
                HandleCoinPickup();
                break;
            case PickupType.HealingPotion:
                //�����Ѫ�����߼�
                HandleHealingPotionPickup();
                break;

        }
        //���ٵ���
        Destroy(gameObject);

    }

    //ʰȡ��Һ���߼�
    private void HandleCoinPickup()
    {
        //������ҵĽ������
        GameManager.Instance.AddCoins(value);

        //��ʾʰȡ�����ֵ
        GameManager.Instance.ShowText("+" + value, transform.position, Color.yellow);
    }
    //�����Ѫ���ߵ��߼�
    private void HandleHealingPotionPickup()
    {
        //������ҵ�����ֵ
        player.RestoreHealth(value);

        //��ʾ��Ѫ��ֵ
        GameManager.Instance.ShowText("+" + value, transform.position, Color.green);
    }
}
