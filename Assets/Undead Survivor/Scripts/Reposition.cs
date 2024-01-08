using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using Random = UnityEngine.Random;

public class Reposition : MonoBehaviour
{
    private Collider2D collider;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Area"))
        {
            return;
        }

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        switch (transform.tag)
        {
            case "Ground":
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;
        
                float dirX = diffX < 0 ? -1.0f : 1.0f;
                float dirY = diffY < 0 ? -1.0f : 1.0f;

                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);
                
                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40.0f);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40.0f);
                }
                break;
            case "Enemy":
                if (collider.enabled)
                {
                    Vector3 dist = playerPos - myPos;
                    Vector3 rand = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(2 * dist + rand);
                }
                break;
        }
    }
}
