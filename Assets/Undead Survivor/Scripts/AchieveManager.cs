using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{

    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    // 업적 달성 알림을 위한 오브젝트
    public GameObject uiNotice;
    
    // 업적 종류를 담는 enum
    enum Achieve { UnlockPotato, UnlockBean }
    // 업적의 목록
    private Achieve[] achieves;
    private WaitForSecondsRealtime wait;
    void Awake()
    {
        achieves = (Achieve[])Enum.GetValues(typeof(Achieve));
        wait = new WaitForSecondsRealtime(3);
        
        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    void Init()
    {
        // 메모리 데이터 저장
        PlayerPrefs.SetInt("MyData", 1);

        // 업적 달성 여부 초기화
        foreach (Achieve achieve in achieves)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 0);
        }

    }
    
    // Start is called before the first frame update
    void Start()
    {
        UnlockCharacter();
    }

    void UnlockCharacter()
    {
        // 업적 달성 여부에 따른 캐릭터 해금
        for (int index = 0; index < lockCharacter.Length; index++)
        {
            string achieveName = achieves[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achieveName) == 1;
            lockCharacter[index].SetActive(!isUnlock);
            unlockCharacter[index].SetActive(isUnlock);
        }
    }
    
    void LateUpdate()
    {
        foreach (Achieve achieve in achieves)
        {
            CheckAchieve(achieve);
        }
    }

    // 업적 달성을 했는지 여부 체크
    void CheckAchieve(Achieve achieve)
    {
        bool isAchieve = false;
        switch (achieve)
        {
            case Achieve.UnlockPotato:
                isAchieve = GameManager.instance.kill >= 100;
                break;
            case Achieve.UnlockBean:
                isAchieve = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }

        // 업적 달성 - 값을 1로 수정
        if (isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);

            for (int index = 0; index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (int)achieve;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }
            
            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        
        yield return wait;
        
        uiNotice.SetActive(false);
    }
}
