using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// 拾取道具
/// </summary>
public class Pickup : MonoBehaviour
{
    //定义Pickup的类型
    public enum PickupType
    {
        Coin,
        HealingPotion
    }

    [Header("道具类型")]
    [SerializeField] private PickupType pickupType;          //设置Pickup的类型
    [SerializeField] private int value;

    [Header("上抛动画")]
    [SerializeField] private float throwHeight = 1f;
    [SerializeField] private float throwDuration = 1f;

    [Header("拾取范围")]
    [SerializeField] private float pickUpDistance = 3f;    //自动拾取范围
    [SerializeField] private float moveSpeed = 5f;       //自动拾取速度
    private bool canPickUp = false;     //是否可拾取

    private Player player;//玩家的引用

    private void Awake()
    {
        player = FindObjectOfType<Player>();//查找场景中的Player对象并赋值给player变量
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

    //上抛动画
    private void ThrowItem()
    {

        //使用DOTween创建动画
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

    //根据类型来执行不同的逻辑
    private void CollectPickup()
    {
        switch (pickupType)
        {
            case PickupType.Coin:
                //处理金币逻辑
                HandleCoinPickup();
                break;
            case PickupType.HealingPotion:
                //处理回血道具逻辑
                HandleHealingPotionPickup();
                break;

        }
        //销毁道具
        Destroy(gameObject);

    }

    //拾取金币后的逻辑
    private void HandleCoinPickup()
    {
        //增加玩家的金币数量
        GameManager.Instance.AddCoins(value);

        //显示拾取金币数值
        GameManager.Instance.ShowText("+" + value, transform.position, Color.yellow);
    }
    //处理回血道具的逻辑
    private void HandleHealingPotionPickup()
    {
        //增加玩家的生命值
        player.RestoreHealth(value);

        //显示回血数值
        GameManager.Instance.ShowText("+" + value, transform.position, Color.green);
    }
}
