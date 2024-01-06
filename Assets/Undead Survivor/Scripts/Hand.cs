using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    // Note : 원래 강좌와 왼손/오른손이 다르다. 왼손이 원거리 무기, 오른손이 근접무기이다.
    public bool isLeft;
    public SpriteRenderer spriteRend;

    private SpriteRenderer player;

    private Vector3 leftPos = new Vector3(0.35f, -0.15f, 0);
    private Vector3 leftPosReverse = new Vector3(-0.15f, -0.15f, 0);
    private Quaternion rightRot = Quaternion.Euler(0, 0, -35f);
    private Quaternion rightRotReverse = Quaternion.Euler(0, 0, -135f);
    
    //private Vector3 rightPos = new Vector3();
    
    void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    private void LateUpdate()
    {
        bool isReverse = player.flipX;
        // 오른손(근접무기)
        if (!isLeft)
        {
            transform.localRotation = isReverse ? rightRotReverse : rightRot;
            spriteRend.flipY = isReverse;
            spriteRend.sortingOrder = isReverse ? 4 : 6;
        }
        // 왼손(원거리 무기)
        else
        {
            transform.localPosition = isReverse ? leftPosReverse : leftPos;
            spriteRend.flipX = isReverse;
            spriteRend.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
