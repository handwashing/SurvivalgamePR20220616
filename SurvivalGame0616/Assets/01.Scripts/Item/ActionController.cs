using UnityEngine;
using UnityEngine.UI;

// 아이템 강의 26분에 Rock 스크립트에 내용 추가 필요 + 프리팹 할당 필요
public class ActionController : MonoBehaviour
{

    // 습득가능한 최대 거리 == ray 사정거리
    [SerializeField]
    private float range; 

    // 습득 가능하다면 true, default == false
    private bool pickupActivated = false;

    // 충돌체 정보 저장
    private RaycastHit hitInfo;

    // 아이템이 아이템 레이어에만 반응하도록 레이어 마스크를 생성
    [SerializeField]
    private LayerMask layerMask;

    // 아이템을 획득 가능 할 때 표기할 텍스트
    [SerializeField]
    private Text actionText;

    [SerializeField]
    private Inventory theInventory;
    // 매 프레임마다 키가 눌리고 있는지 확인
    private void Update() 
    {
        // 매프레임마다 아이템이 있는지 확인
        CheckItem();
        TryAction();
    }

    // 아이템 먹을 때 E 키가 눌리는지 확인할 메서드
    private void TryAction()
    {
        // E키가 눌렸을 떄 아이템이 있는지 없는지 확인하는 메서드
        if(Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }

    // pickupActivated가 true라면 아이템을 주어라
    private void CanPickUp()
    {
        if(pickupActivated){
            if(hitInfo.transform != null)
            {
                // 어떤 아이템을 획득했는지 확인
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "획득했습니다.");
                // 인벤토리 스크립트 작성 후 추가
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                // 획득한 아이템 파괴
                Destroy(hitInfo.transform.gameObject);
                ItemInfoDisappear();
            }
        }
    }

    private void CheckItem()
    {
        // 레이를 쏴서 플레이어가 바라보는 방향으로 충돌체의 정보를 확인하고 레이의 사정거리를 지정
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            if(hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else // 아이템 획득하게 되면 정보 비활성화
        {
            ItemInfoDisappear();
        }
    }

    // 아이템 정보가 보이는 메서드
    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "획득" +"<color=yellow>" + "(E)"+ "</color>";
    }

    // 아이템 정보를 사라지게 하는 메서드
    private void ItemInfoDisappear()
    {
        pickupActivated = false;
        //
        actionText.gameObject.SetActive(false);
    }
}
