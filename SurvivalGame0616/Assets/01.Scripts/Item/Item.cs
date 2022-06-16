using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "New Item /Item", order = 0)]
public class Item : ScriptableObject 
{
    
    public string itemName; // 아이템 이름
    
    public ItemType itemType; //아이템 유형
    
    public Sprite itemImage; // 아이템의 이미지 : Sprite 사용
    
    public GameObject itemPrefab; // 아이템의 프리팹

   
    public string weaponType;  // 무기유형

    
    public enum ItemType // 아이템 상태 변화 
    {
        Equipment, // 장비류
        Used, // 소모품류
        Ingredient,  // 재료
        ETC // 기타
    }


}
