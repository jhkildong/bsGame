using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//공통적으로 사용할 싱클턴 클래스. 싱글턴을 사용할 컴포넌트는 상속받아 사용하면 된다.
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour // where -> T는 monobehavior를 상속받기 때문에 참조형이다. ( 모호성을 없애기 위해 상속한정자를 사용했다.)
{

    //싱글턴은 어디서든 손쉽게 접근가능
    static T _inst = null;
    public static T Instance
    {
        get
        {
            if (_inst == null) // 처음 호출된다면
            {
                _inst = FindObjectOfType<T>(); //유일성 보장을 위해
                if (_inst == null)//찾지못했다면 -> 존재하지 않는다면
                {
                    GameObject obj = new GameObject(); //게임오브젝트를 만들어
                    obj.name = typeof(T).ToString(); // 게임 오브젝트의 이름을 Singleton으로 바꿔준다. ( 알기 쉽게 )
                    _inst = obj.AddComponent<T>(); // 게임오브젝트에 이 컴포넌트를 추가해준다.
                }
            }
            return _inst;
        }

    }
    protected void Initialize()
    {
        //_inst = this; // 힙영역의 자기자신을 참조자에 참조하라고 해준다.
        if (_inst != null && _inst != this) //다른 인스턴스가 생성될때
        {
            Destroy(this.gameObject); //이번에 생성된 자신을 파괴하여 유일성을 유지
        }
        else
        {
            DontDestroyOnLoad(gameObject); // 씬을 이동해도 지워지지 않게.
        }
    }
}
