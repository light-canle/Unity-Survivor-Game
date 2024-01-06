using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    private bool isLive;
    
    private Rigidbody2D rigid;
    private Collider2D collider;
    private Animator anim;
    private SpriteRenderer sprite;
    private WaitForFixedUpdate wait;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
    }

    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        // 콜라이더 활성화 - 처음 상태로 돌려놓음
        collider.enabled = true;
        rigid.simulated = true;
        sprite.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        
        // Hit상태(총알에 맞아 넉백 필요)인 경우 FixedUpdate를 하지 않음
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            return;
        }
        
        Vector2 dirVec = (target.position - rigid.position).normalized;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        
        if (!isLive)
        {
            return;
        }
        sprite.flipX = target.position.x < rigid.position.x;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Bullet") || !isLive)
        {
           return; 
        }

        health -= col.GetComponent<Bullet>().damage;
        // 넉백
        StartCoroutine(KnockBack());
        if (health > 0)
        {
            // 피격 표현
            anim.SetTrigger("Hit");
        }
        else
        {
            isLive = false;
            // 콜라이더 비활성화
            collider.enabled = false;
            rigid.simulated = false;
            sprite.sortingOrder = 1;
            // dead 애니메이션, 비활성화
            anim.SetBool("Dead", true);
            // 경험치 부여
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait; // 다음 fixedupdate까지 딜레이
        
        // 플레이어 반대 방향으로 넉백
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
