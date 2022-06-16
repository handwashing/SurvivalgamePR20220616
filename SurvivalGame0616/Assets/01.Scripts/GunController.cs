using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    //현재 장착된 총
    [SerializeField]
    private Gun currentGun;

    //연사 속도 계산
    private float currentFireRate; 

    //상태 변수
    private bool isReload = false; 
    [HideInInspector]
    private bool isFineSightMode = false;

    //본래 포지션 값
    [SerializeField]
    private Vector3 originPos;

    //효과음 재생
    private AudioSource audioSource;

    //레이저 충돌 정보 받아옴
    private RaycastHit hitInfo;

    //필요한 컴퍼넌트
    [SerializeField]
    private Camera theCam; //*게임 화면이 카메라 시점인 경우임! 카메라 시점에서 정 가운데 총알 발사할 것!

    //피격 이펙트
    [SerializeField]
    private GameObject hit_effect_prefab;

    void Start()
    {
        originPos = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GunFireRateCalc();
        TryFire(); //발사 시도
        TryReload(); //재장전 시도
        TryFineSight(); //정조준 시도
    }

    //연사속도 재계산
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime; //currentFireRate가 0보다 클 경우, 1초에 1씩 감소 
                                               // 0이 되면 발사할 수 있는 상태가 됨
    }
    
    //발사 시도
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    //발사 전 계산
    private void Fire()
    {
        if ( !isReload)
        {
            if(currentGun.currentBulletCount > 0)
            {   
                Shoot(); //발사(발사 전)
            }
        
            else
            {//총알이 0발일때 발사하면 Reload가 이루어진다
                CancelFineSight();
                StartCoroutine(ReloadCoroutine());
            }
        }
    }

    //발사 후 계산
    private void Shoot() //(발사 후)
    {
        currentGun.currentBulletCount--; //총알 개수 -1
        currentFireRate = currentGun.fireRate; //발사 후 연사속도 재계산
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        Hit();
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
        //Debug.Log("총알 발사함");
    }

    private void Hit()
    {//현재 위치에서 발사 / 충돌한게 있다면 반환
        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward, out hitInfo, currentGun.range))
        {
            GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(clone, 2f); //2초 후에 생성된 클론 파괴
        }

    }

    //재장전 시도
    private void TryReload()
    {//R버튼을 누르면 재장전
        if(Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroutine());
        }

    }

    //재장전
    IEnumerator ReloadCoroutine()
    {
        if(currentGun.carryBulletCount > 0)
        {
            isReload = true;

            currentGun.anim.SetTrigger("Reload"); //Reload anim 실행


            currentGun.carryBulletCount += currentGun.currentBulletCount; //현재 소유한 탄알에 더해주기 
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if(currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }


            isReload = false;
        }

        else
        {
            Debug.Log("소유한 총알이 없습니다.");
        }


    }

    //정조준 시도
    private void TryFineSight()
        {
            if(Input.GetButtonDown("Fire2") && !isReload) //Reload중이 아닐때 정조준 가능
            {                                             //재장전을 할 경우 정조준 상태가 취소됨 -> false
                FineSight();
            }
        }

    //정조준 취소
    public void CancelFineSight()
    {//isFineSightMode가 true면 FineSight를 한 번 더 실행하기
        if(isFineSightMode)
            FineSight();
    }

    //정조준 로직 가동
    private void FineSight()
    {
        isFineSightMode = !isFineSightMode; //위의 FineSigh가 실행될 때마다 알아서 true,false로 바뀌게 함... / 처음에는 false이니 true로 바꿔짐...
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);
    
        //정조준인지 아닌지 구분
        if (isFineSightMode)
        {   
            StopAllCoroutines();
            StartCoroutine(FineSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeactivateCoroutine());
        }
    }

    //정조준 활성화(가동)
    IEnumerator FineSightActivateCoroutine()
    {//총의 현재 위치가 정조준 위치가 될 때까지 반복 / 화면 가운데로 총이 올떄까지...
        while(currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {   //0.2f 의 세기로 현재 위치에서 정조준 위치로...
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null; //1 frame 대기
        }

    }

    //정조준 비활성화
    IEnumerator FineSightDeactivateCoroutine()
    {//정조준을 취소하면 원래의 값으로 돌아갈 때까지 (Lerp돌리기...)반복
        while (currentGun.transform.localPosition != originPos)
        {   //0.2f 의 세기로 현재 위치에서 정조준 위치로...
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null; //1 frame 대기
        }

    }

    //반동 코루틴
    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z); //정조준 안 했을때 최대 반동
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z); //정조준 했을때 최대 반동

        if (!isFineSightMode)
        {
            currentGun.transform.localPosition = originPos; //currentGun의 위치를 원래 포지션 으로(반동의 중복을 멈추려고)

            //반동 시작
            while(currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f) //0.02만큼 여유를 줘 증가하다가 값이 일치해지면 끝내도록 하기
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null; //매 프레임마다 반복이 이루어도록 대기를 주기...
            }

            //원위치 

            while(currentGun.transform.localPosition != originPos)
            {
                //현재위치에서 originPos가 될 때까지 반복
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        else
        {
           currentGun.transform.localPosition = currentGun.fineSightOriginPos; //정조준 상태로 되돌리기

            //반동 시작
            while(currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f) 
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null; 
            }

           
            //정조준 상태의 원래값
            while(currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                //현재위치에서 fineSightOriginPos가 될 때까지 반복
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }

    //사운드 재생
    private void PlaySE(AudioClip _clip) 
        {
            audioSource.clip = _clip;
            audioSource.Play();
        }
}
