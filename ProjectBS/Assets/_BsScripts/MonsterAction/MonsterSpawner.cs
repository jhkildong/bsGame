using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
계층구조
Player
ㄴ MonsterSpawner
-> 플레이어 위치를 기준으로 리스폰 하기위해 플레이어의 자식으로 등록

applyRespawn을 통해 리스폰여부
respawnTime으로 리스폰 주기 결정
*/

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private MonsterData[] monsterDatas;
    public float respawnDist = 20.0f;
    public bool applyRespawn;
    public float respawnTime = 0.5f;


    public int init = 10;
    public int max = 10;
    // Start is called before the first frame update
    void Start()
    {
        foreach(MonsterData data in monsterDatas)
        {
            ObjectPoolManager.Instance.SetPool(data, init, max);
        }
        applyRespawn = true;
        StartCoroutine(EnemySpawn(monsterDatas));
    }
    IEnumerator EnemySpawn(MonsterData[] monsterDatas)
    {
        while(true)
        {
            foreach (MonsterData md in monsterDatas)
            {
                float rndAngle = Random.value * Mathf.PI * 2.0f;
                Vector3 rndPos = new Vector3(Mathf.Cos(rndAngle), 0f, Mathf.Sin(rndAngle)) * respawnDist;
                rndPos += transform.position;
                if (md is NormalMonsterData nmd)
                {
                    switch (nmd.Type)
                    {
                        case MonsterType.Single:
                            ObjectPoolManager.Instance.GetObj(md).gameObject.transform.position = rndPos;
                            break;
                        case MonsterType.Group:
                            for (int i = 0; i < (nmd as GroupNormalMonsterData).Amount; ++i)
                            {
                                Vector3 addPos = new Vector3(Random.Range(0, 2.0f), 0, Random.Range(0, 2.0f));
                                ObjectPoolManager.Instance.GetObj(md).gameObject.transform.position = rndPos + addPos;
                            }
                            break;
                        case MonsterType.Surround:

                            break;
                    }
                }
                yield return new WaitForSeconds(respawnTime);
            }
            
        }
    }
    /*
    private void RandomMonsterGenerate(MonsterData[] monsterDatas)
    {
        foreach(MonsterData md in monsterDatas)
        {
            float rndAngle = Random.value * Mathf.PI * 2.0f;
            Vector3 rndPos = new Vector3(Mathf.Cos(rndAngle), 0f, Mathf.Sin(rndAngle)) * respawnDist;
            rndPos += transform.position;
            if (md is NormalMonsterData nmd)
            {
                switch(nmd.Type)
                {
                    case MonsterType.Single:
                        ObjectPoolManager.Instance.GetObj(md).gameObject.transform.position = rndPos;
                        break;
                    case MonsterType.Group:
                        for(int i = 0; i<(nmd as GroupNormalMonsterData).Amount; ++i)
                        {
                            Vector3 addPos = new Vector3(Random.Range(0, 2.0f), 0, Random.Range(0, 2.0f));
                            ObjectPoolManager.Instance.GetObj(md).gameObject.transform.position = rndPos + addPos;
                        }
                        break;
                    case MonsterType.Surround:

                        break;
                }
            }
        }
    }
    */

}
