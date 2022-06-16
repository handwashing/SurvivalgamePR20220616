using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField] private string animalName;     // 동물의 이름
    [SerializeField] private int hp;        // 동물의 체력

    [SerializeField] private float walkSpeed;       // 걷기 스피드
    [SerializeField] private float runSpeed;        // 뛰기 스피드

    private Vector3 direction;      // 방향 설정

    // 상태변수
    private bool isAction;      // 행동중인지 아닌지 판별
    private bool isWalking;      // 걷는지 안 걷는지 판별
    private bool isRunning;      // 뛰는지 판별

    [SerializeField] private float walkTime;    // 걷기 시간
    [SerializeField] private float waitTime;    // 대기 시간
    [SerializeField] private float runTime;     // 뛰기 시간
    private float currentTime;      // 여기에 대기 시간 넣고 1초에 1씩 감소시킬 것

    // 필요한 컴포넌트
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private BoxCollider boxCol;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = waitTime;     // 대기시간 넣어줌
        isAction = true;        // 대기하는 것도 액션이니 트루줌
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotation();
        ElapseTime();

    }

    private void Move()
    {
        if (isWalking)
        {
            rigid.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));
        }
    }

    private void Rotation()
    {
        if (isWalking)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, direction, 0.01f);
            rigid.MoveRotation(Quaternion.Euler(_rotation));    // Vector3를 Quaternion으로 바꿔줌
        }
    }

    private void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
                ReSet(); // 다음 랜덤 행동 개시
        }
    }

    private void ReSet()
    {
        isWalking = false; isAction = true;
        anim.SetBool("Walking", isWalking);
        direction.Set(0f, Random.Range(0f, 360f), 0f);      // 방향 랜덤하게
        RandomAction();
    }

    private void RandomAction()
    {
        int _random = Random.Range(0, 4);   // 대기, 풀뜯기, 두리번, 걷기 
                                            //(,_ 는 실행되지 않으므로 4개 실행하고 싶으면 3이 아닌 4 (0f, 4f 하면 4도 포함시킴))
        if (_random == 0)
            Wait();
        else if (_random == 1)
            Eat();
        else if (_random == 2)
            Peek();
        else if (_random == 3)
            TryWalk();
    }

    private void Wait()
    {
        currentTime = waitTime;
        Debug.Log("대기");
    }
    private void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
        Debug.Log("풀뜯기");
    }
    private void Peek()
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");
        Debug.Log("두리번");
    }
    private void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        Debug.Log("걷기");
    }

    private void Run(Vector3 _targetPos)
    {
        direction = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles;       // 플레이어와 반대방향으로 도망가게 할것

        currentTime = runTime;
    }
}
