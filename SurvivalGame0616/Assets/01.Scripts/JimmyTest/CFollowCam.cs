using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CFollowCam : MonoBehaviour
{
    public Transform target;        // 따라다닐 타겟 오브젝트의 Transform
 
    private Transform tr;                // 카메라 자신의 Transform
 
    void Start()
    {
        tr = GetComponent<Transform>();
    }
 
    void LateUpdate()   // 타겟이 Update()에서 움직일 수 있기 때문에 모든 Update 함수가 호출된 다음에 실행되는 LateUpdate()를 사용함
    {
        tr.position = new Vector3(target.position.x , tr.position.y, target.position.z - 5.5f);
 
        tr.LookAt(target);
    }
}
