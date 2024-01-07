using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count; // 근접 무기의 개수
    public float speed; // 공전 속도

    private float timer; // 원거리 무기 발사 간격
    private Player player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isLive)
            return;
        
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }
        
        // 테스트용 코드
        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);
        }
    }

    public void Init(ItemData data)
    {
        // 기본 세팅
        name = "Weapon" + data.itemId;
        transform.parent = player.transform; // player의 자식 오브젝트로 설정
        transform.localPosition = Vector3.zero;

        // 프로퍼티 세팅
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 150 * Character.WeaponSpeed; // 회전 속도
                Batch();
                break;
            default:
                speed = 0.3f * Character.WeaponRate; // 발사 간격
                break;
        }
        
        // Hand 설정
        Hand hand = player.hands[(int)data.itemType];
        hand.spriteRend.sprite = data.hand;
        hand.gameObject.SetActive(true);
        
        // 자식 오브젝트에게 ApplyGear 함수 실행을 명령
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage * Character.Damage;
        this.count += count;

        // 근접 무기용 추가 배치 함수
        if (id == 0)
        {
            Batch();
        }
        
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }
    
    void Batch()
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;
            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360.0f * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // 근접 무기의 관통력은 무한하다(-1)
        }
    }

    void Fire()
    {
        // 적이 있는지 확인
        if (!player.scanner.nearestTarget)
        {
            return;
        }

        // 총알이 가려는 방향 계산
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;
        // 총알의 속성 결정
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
