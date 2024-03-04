using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeon.ResetExtention;

public class TestExtension : MonoBehaviour
{
    public MonsterData monsterData;
    public Monster monster;
    public bool trigger1;
    public bool trigger2;
    // Start is called before the first frame update
    void Start()
    {
        monster = monsterData.CreateMonster();
        trigger1 = false;
        trigger2 = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(trigger1)
        {
            monster.Speed *= 1.2f;
            trigger1 = false;
            Debug.Log(monster.Speed);
        }
        if(trigger2)
        {
            //monster.Speed.ResetCeof();
            trigger2 = false;
            Debug.Log(monster.Speed);
        }
    }
}
