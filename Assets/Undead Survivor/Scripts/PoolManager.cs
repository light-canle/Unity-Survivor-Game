using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리팹 보관
    public GameObject[] prefabs;
    // 프리팹 리스트
    private List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
        
    }

    /// <summary>
    /// 해당 프리팹에 해당하는 게임 오브젝트를 찾아서 반환한다.
    /// </summary>
    /// <param name="index">찾으려는 프리팹의 인덱스 번호</param>
    /// <returns>그 프리팹 종류가 들어있는 List에서 비활성화 된 개체를 찾아 반환하고 없으면 새로 생성한다.</returns>
    public GameObject Get(int index)
    {
        GameObject select = null;

        // 선택한 풀에 속한 게임오브젝트 중 비활성화 된 것에 접근
        
        foreach (GameObject item in pools[index])
        {
            // 발견하면 select 변수에 할당
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // 비활성화된 오브젝트가 없을 시 새롭게 생성
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }
        return select;
    }
}
