using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {
        // 기본 세팅
        name = "Gear" + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;
        
        // 프로퍼티 세팅
        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }
    
    // 장갑 전용 함수 - 장착한 무기의 속도 또는 발사 간격을 빠르게 한다.
    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                // 근접 무기
                case 0:
                    weapon.speed = 150 + 150 * rate;
                    break;
                // 원거리 무기
                default:
                    weapon.speed = 0.5f * (1f - rate);
                    break;
            }
        }
    }
    
    // 신발 전용 함수 - 플레이어의 이동 속도를 향상시킨다.
    void SpeedUp()
    {
        float speed = 3f;
        GameManager.instance.player.speed = speed + speed * rate;
    }
}
