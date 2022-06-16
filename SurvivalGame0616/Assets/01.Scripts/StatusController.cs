using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    // 체력
    [SerializeField]
    private int hp;
    private int currentHp;

    // 스태미나
    [SerializeField]
    private int sp;
    private int currentSp;

    // 스태미나 증가량
    [SerializeField]
    private int spIncreaseSpeed;

    // 스태미나 재회복 딜레이
    [SerializeField]
    private int spRechargeTime;
    private int currentSpRechargeTime;

    // 스태미나 감소 여부
    private bool spUsed;

    // 방어력
    [SerializeField]
    private int dp;
    private int currentDp;

    // 배고픔
    [SerializeField]
    private int hungry;
    private int currentHungry;

    // 배고픔이 줄어드는 속도
    [SerializeField]
    private int hungryDecreaseTime;
    private int currentHungryDecreaseTime;

    // 목마름
    [SerializeField]
    private int thirsty;
    private int currentThirsty;

    // 목마름이 줄어드는 속도
    [SerializeField]
    private int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;

    // 만족도
    [SerializeField]
    private int satisfy;
    private int currentSatisfy;

    // 필요한 이미지
    [SerializeField]
    private Image[] images_Gauge;

    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;
    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;
        currentDp = dp;
        currentSp = sp;
        currentHungry = hungry;
        currentThirsty = thirsty;
        currentSatisfy = satisfy;
    }

    // Update is called once per frame
    void Update()
    {
        Hungry();
        Thirsty();
        SPRechargeTime();
        SPRecover();
        GaugeUpdate();
    }

    // SP 회복 딜레이
    private void SPRechargeTime()
    {
        if (spUsed)     // 스테미나가 감소중이라면
        {
            if (currentSpRechargeTime < spRechargeTime)     // 현재 SpRechargeTime이 SpRechargeTime보다 작으면
                currentSpRechargeTime++;    // 증가시켜줌
            else
                spUsed = false;
        }
    }
    // SP 회복
    private void SPRecover()
    {
        if (!spUsed && currentSp < sp)  // 스테미나를 사용하고있지 않고, 현재 Sp가 sp보다 작을때
        {
            currentSp += spIncreaseSpeed;
        }
    }

    private void Hungry()       // 배고픔 구현
    {
        if (currentHungry > 0)      // 현재 배고픔이 0보다 클 경우에만 깎음
        {
            if (currentHungryDecreaseTime <= hungryDecreaseTime)
                currentHungryDecreaseTime++;
            else
            {
                currentHungry--;
                currentHungryDecreaseTime = 0;
            }
        }
        else        // 0보다 작아졌을때
            Debug.Log("배고픔 수치가 0이 되었습니다");
    }

    private void Thirsty()      // 목마름 구현
    {
        if (currentThirsty > 0)
        {
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
                currentThirstyDecreaseTime++;
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;
            }
        }
        else
            Debug.Log("목마름 수치가 0이 되었습니다");
    }

    private void GaugeUpdate()      // 상태 수치 변화 시각화
    {
        images_Gauge[HP].fillAmount = (float)currentHp / hp;
        images_Gauge[SP].fillAmount = (float)currentSp / sp;
        images_Gauge[DP].fillAmount = (float)currentDp / dp;
        images_Gauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        images_Gauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        images_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;

    }

    // HP 회복 (아이템 사용시)
    public void IncreaseHP(int _count)
    {
        if (currentHp + _count < hp)        // currentHp와 회복될 수치를 더했을때 hp가 넘는가?
        {
            currentHp += _count;
        }
        else
            currentHp = hp;
    }

    // HP 감소
    public void DecreaseHP(int _count)
    {
        if (currentDp > 0)      // 체력을 깎기 전에 DP를 먼저 깎는다
        {
            DecreaseDP(_count);
        }
        currentHp -= _count;

        if (currentHp <= 0)     // currentHp가 0 이하가 되면 죽음
            Debug.Log("캐릭터의 hp가 0이 되었습니다!!");
    }

    // DP 회복 
    public void IncreaseDP(int _count)
    {
        if (currentDp + _count < hp)        // currentDp와 회복될 수치를 더했을때 hp가 넘는가?
        {
            currentDp += _count;
        }
        else
            currentDp = hp;
    }

    public void DecreaseDP(int _count)
    {
        currentDp -= _count;

        if (currentDp <= 0)
            Debug.Log("방어력이 0이 되었습니다!!");
    }

    // Hungry 증가
    public void IncreaseHungry(int _count)
    {
        if (currentHungry + _count < hungry)        
        {
            currentHungry += _count;
        }
        else
            currentHungry = hungry;
    }

    // Hungry 감소
    public void DecreaseHungry(int _count)
    {
        currentHungry -= _count;

    }

    public void DecreaseStamina(int _count)     // 특정한 행동으로 닳는것이기 때문에 함수 필요
    {
        spUsed = true;
        currentSpRechargeTime = 0;      // ++되면서 RechargeTime되면 sp 회복함

        if (currentSp - _count > 0)     // 받아온 _count 값만큼 깎아줌
            currentSp -= _count;
        else
            currentSp = 0;      // -가 되지않게 0으로 만들어줌
    }

    // SP가 0 이하가 되면 뛰거나 점프 금지
    public int GetCurrentSP()
    {
        return currentSp;
    }
}
