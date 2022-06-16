using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp; //바위의 체력(0이 되면 파괴)

    [SerializeField]
    private float destroyTime; //파괴 후 남은 파편 제거 시간

    [SerializeField]
    private SphereCollider col; //구체 콜라이더

    
    //필요한 게임 오브젝트
    [SerializeField]
    private GameObject go_rock; //일반 바위
    [SerializeField]
    private GameObject go_debris; //깨진 바위
    
    //채굴
    public void Mining()
    {
        hp--;
        if (hp <= 0) //hp가 0이하면 파괴
            Destruction();
    }

    private void Destruction()
    {//바위가 파괴 되었기에 비활성화하고 사라지게 하기 -> 잔해만 남도록
        col.enabled = false;
        Destroy(go_rock);

        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime); //일정 시간 후 debris도 삭제
    }

   
}
