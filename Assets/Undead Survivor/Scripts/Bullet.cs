using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per; // 관통력

    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        // 원거리 무기용 속도
        if (per >= 0)
        {
            rigid.velocity = dir * 10f;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Enemy") || per == -100)
            return;

        per--;
        if (per < 0)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    // 화면 밖으로 나간 총알 삭제
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Area") || per == -100)
        {
            return;
        }
        
        rigid.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }
}
