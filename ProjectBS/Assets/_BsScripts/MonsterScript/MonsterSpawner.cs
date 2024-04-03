using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Yeon;

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
    [SerializeField, ReadOnly]
    private MonsterData[] monsterDatas;
    private List<Monster> monsterList = new List<Monster>();
    public float respawnDist = 20.0f;
    public bool applyRespawn;
    public float respawnTime = 0.5f;


    public int init = 10;
    public int max = 10;

    // Start is called before the first frame update
    void Start()
    {
        monsterDatas = Resources.LoadAll<MonsterData>("Monster");

        foreach(MonsterData data in monsterDatas)
        {
            Monster monster = data.CreateClone();
            monsterList.Add(monster);
            ObjectPoolManager.Instance.SetPool(monster, init, max);
            ObjectPoolManager.Instance.ReleaseObj(monster);
        }
        applyRespawn = true;
        StartCoroutine(EnemySpawn());
    }

    public QuadTree quadTree;

    //임시
    bool checkSurround = false;
    float surroundRange = 30.0f;
    IEnumerator EnemySpawn()
    {
        while(true)
        {
            foreach (Monster monster in monsterList)
            {
                float rndAngle = Random.value * Mathf.PI * 2.0f;
                Vector3 rndPos = new Vector3(Mathf.Cos(rndAngle), 0f, Mathf.Sin(rndAngle)) * respawnDist;
                rndPos += transform.position;
                GameObject go;
                if (monster is NormalMonster nMonster)
                {
                    var MonsterType = nMonster.NormalData.Type;
                    switch (MonsterType)
                    {
                        case var type when type.HasFlag(MonsterType.Single):
                            go = ObjectPoolManager.Instance.GetObj(monster).Data.gameObject;
                            go.transform.position = rndPos;
                            //quadTree.OnItemSpawned(go); //임시
                            //quadTree.ShowStats(); //임시
                            break;
                        case var type when type.HasFlag(MonsterType.Group):
                            for (int i = 0; i < (nMonster as GroupMonster).GroupData.Amount; ++i)
                            {
                                Vector3 addPos = new Vector3(Random.Range(0, 2.0f), 0, Random.Range(0, 2.0f));
                                go = ObjectPoolManager.Instance.GetObj(monster).Data.gameObject;
                                go.transform.position = rndPos + addPos;
                                if(type.HasFlag(MonsterType.StraightMove))
                                {
                                    go.transform.LookAt(transform.position);
                                }
                                //quadTree.OnItemSpawned(go); //임시
                                //quadTree.ShowStats(); //임시
                            }
                            break;
                        case var type when type.HasFlag(MonsterType.Surround):
                            if (checkSurround == false)
                            {
                                StartCoroutine(SurroundSpawn(monster));
                                checkSurround = true;
                            }
                            break;
                    }
                }
                
                yield return new WaitForSeconds(respawnTime);
            }
        }
    }

    IEnumerator SurroundSpawn(Monster monster)
    {
        float anlge = 3.6f;
        float angleStep = 360.0f / anlge;
        for (int i = 0; i < angleStep; i++)
        {
            float ang = anlge * i * Mathf.Deg2Rad;
            Vector3 pos = new Vector3(Mathf.Cos(ang), 0f, Mathf.Sin(ang)) * surroundRange;
            GameObject go = ObjectPoolManager.Instance.GetObj(monster).Data.gameObject;
            go.transform.position = pos + transform.position;
            //quadTree.OnItemSpawned(go); //임시
            //quadTree.ShowStats(); //임시
            yield return null;
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
