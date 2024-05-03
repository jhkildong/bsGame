using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _ButtonEventexRA : MonoBehaviour
{
    //버튼 프리펩에 넣어야 되는 스크립.
    private Button button;
    private OrditalWeaponRA Weapon;
    // Start is called before the first frame update
    private void Start()
    {
        button = GetComponent<Button>(); //버튼 component 가져오기
        button.onClick.AddListener(OnButtonClick); //인자가 없을 때 함수 호출
        Weapon = BlessManager.Instance.CreateBless(BlessID.RA) as OrditalWeaponRA;
    }

    private void OnButtonClick()
    {
        Weapon.LevelUp(); // OrditalWeapon의 OnOkSpawnOrditalWeapon() 메서드 호출
    }
}
