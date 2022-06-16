using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    public string closeWeaponName; //근접 무기 이름

    //Weapon Type
    public bool isHand; //맨손인지
    public bool isAxe; //도끼인지
    public bool isPickaxe; //곡괭이인지...

    public float range; //공격 범위(팔을 뻗으면 어디까지 닿을지 결정...)
    public int damage; //공격력
    public float workSpeed; //작업 속도
    public float attackDelay; //attck Delay
    public float attackDelayA; //공격 활성화 시점(딜레이)
    public float attackDelayB; //공격 비활성화 시점
    
    public Animator anim; //animation

}
