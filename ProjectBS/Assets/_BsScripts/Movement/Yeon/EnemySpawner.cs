using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
계층구조
Player
ㄴ EmemySpawner
-> 플레이어 위치를 기준으로 리스폰 하기위해 플레이어의 자식으로 등록

applyRespawn을 통해 리스폰여부
respawnTime으로 리스폰 주기 결정
*/

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private MonsterData[] monsterDatas;
    public float respawnDist = 20.0f;
    public bool applyRespawn;
    public float respawnTime = 0.1f;


    public int init = 10;
    public int max = 10;
    // Start is called before the first frame update
    void Start()
    {
        foreach(MonsterData monster in monsterDatas)
        {
            //SetMonsterPool(monster, init, max);
        }
        applyRespawn = true;
        StartCoroutine(EnemySpawn(monsterDatas));
    }
    IEnumerator EnemySpawn(MonsterData[] md)
    {
        while(true)
        {
            if(applyRespawn)
            {
                RandomMonsterGenerate(md);
            }
            yield return new WaitForSeconds(respawnTime);
        }
    }

    private void RandomMonsterGenerate(MonsterData[] monsterDatas)
    {
        foreach(MonsterData md in monsterDatas)
        {
            float rndAngle = Random.value * Mathf.PI * 2.0f;
            Vector3 rndPos = new Vector3(Mathf.Cos(rndAngle), 0f, Mathf.Sin(rndAngle)) * respawnDist;
            rndPos += transform.position;
            //GetMonster(md.ID).gameObject.transform.position = rndPos;
        }
    }
}
