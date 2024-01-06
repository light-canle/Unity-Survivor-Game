using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed = 3.0f;
    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private SpriteRenderer sprite;
    public Scanner scanner;

    public Hand[] hands;
    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    void OnMove(InputValue value)
    {
        if (!GameManager.instance.isLive)
            return;
        inputVec = value.Get<Vector2>();
    }
    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigidbody2D.MovePosition(rigidbody2D.position + nextVec);
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        animator.SetFloat("Speed", inputVec.magnitude);
        if (inputVec.x != 0)
        {
            sprite.flipX = (inputVec.x < 0);
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (!GameManager.instance.isLive)
            return;
        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health <= 0)
        {
            // area, shadow는 남겨야 하므로 2부터 시작(스포너와 양쪽 손 비활성화
            for (int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }
            
            // 애니메이션 작동
            animator.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
}
