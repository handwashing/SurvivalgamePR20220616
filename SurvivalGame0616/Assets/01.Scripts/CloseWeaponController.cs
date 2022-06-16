using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponController : MonoBehaviour
{   //미완성 클래스 = 추상 클래스(abstract)


    //현재 장착된 Hand형 타입(무기)
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    //공격중?
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo; //Raycast에 닿은 정보를 hitInfo에 저장하는 변수

    protected void TryAttack()
    { //왼쪽 버튼을 누를 경우 코루틴이 실행
        if(Input.GetButton("Fire1")) //마우스를 누르고 있는 경우에도 효과 지속
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }
//마우스 좌클릭을 하는 순간 StartCoroutine(AttackCoroutine() 코루틴이 실행되고,
//바로 isAttack = true가 되면서 중복 실행이 막아진다!
//마지막에 isAttack = false를 줘서 실행시키기...!
    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger("Attack"); //Attack애니메이션 실행

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);//currentCloseWeapon.attackDelayA 만큼 대기시간 주기...
        isSwing = true; //공격 들어감 true가 된 순간 공격이 적중했는지 구분하는 함수(코루틴)

        //적중 여부를 판단할 수 있는 코루틴 반복 실행...
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB); //또 대기시간 주기
        isSwing = false; //일정 시간이 지나면 false 가 되어 HitCoroutine이 꺼짐

        //공격할 수 있게 대기...
        //딱 attackDelay 만큼 쉴 수 있게 그 전의 A,B값을 빼줌...
        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);
        isAttack = false; //false를 줘서 재공격할 수 있도록 만들었다...
    }

//공격 적중을 알아보는 코루틴
//Delay A,B사이에 계속 닿은것이 있는지를 체크...
    
    //abstract -> 미완성으로 남겨 자식 클래스가 완성시키도록 하는 추상코루틴
    protected abstract IEnumerator HitCoroutine();
 

    protected bool CheckObject()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range))
        {
            return true; //충돌한 게 있음
        }
        return false; //충돌한 게 없음
    }

    // public void CloseWeaponChange(CloseWeapon _closeWeapon)
    // {
    //     if (WeaponManager .currentWeapon != null)
    //         WeaponManager .currentWeapon.gameObject.SetActive(false);
        
    //     currentCloseWeapon = _closeWeapon;
    //     WeaponManager .currentWeapon = currentCloseWeapon.GetComponent<Transform>();
    //     WeaponManager .currentWeaponAnim = currentCloseWeapon.anim;

    //     currentCloseWeapon.transform.localPosition = Vector3.zero;
    //     currentCloseWeapon.gameObject.SetActive(true);
    //     isActivate = true;
    // }

}

