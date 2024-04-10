using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
��������
Player
�� MonsterSpawner
-> �÷��̾� ��ġ�� �������� ������ �ϱ����� �÷��̾��� �ڽ����� ���

applyRespawn�� ���� ����������
respawnTime���� ������ �ֱ� ����
*/

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private MonsterData[] monsterDatas;                         //���� ������(scriptable object) Resource�������� �ε�
    private List<Monster> monsterList = new List<Monster>();    //ObjectPoolManager�� ����� ������ �纻
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
            if(data.Prefab == null)
            {
                Debug.Log("MonsterData Prefab is null");
                continue;
            }
            Monster monster = data.CreateClone();   //�����͸� ������� �纻����
            monsterList.Add(monster);               //�纻�� ����Ʈ�� �߰�(�߰� ȣ���� ����)
            ObjectPoolManager.Instance.SetPool(monster, init, max);
            ObjectPoolManager.Instance.ReleaseObj(monster);
        }
        applyRespawn = true;
        StartCoroutine(EnemySpawn());
    }

    //�ӽ� surroundMonsterüũ, ��������
    bool checkSurround = false;
    float surroundRange = 30.0f;
    Vector3 tempPos;

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
                            go = ObjectPoolManager.Instance.GetObj(monster).This.gameObject;
                            go.transform.position = rndPos;
                            break;
                        case var type when type.HasFlag(MonsterType.Group):
                            for (int i = 0; i < (nMonster as GroupMonster).GroupData.Amount; ++i)
                            {
                                Vector3 addPos = new Vector3(Random.Range(0, 2.0f), 0, Random.Range(0, 2.0f));
                                go = ObjectPoolManager.Instance.GetObj(monster).This.gameObject;
                                go.transform.position = rndPos + addPos;
                                if(type.HasFlag(MonsterType.StraightMove))
                                {
                                    go.transform.LookAt(transform.position);
                                }
                            }
                            break;
                        case var type when type.HasFlag(MonsterType.Surround):
                            if (checkSurround == false)
                            {
                                tempPos = transform.position;

                                int amount = 100; //�ӽ�
                                StartCoroutine(SurroundSpawn(monster, amount, tempPos));
                                checkSurround = true;
                                break;
                            }
                            if ((transform.position - tempPos).sqrMagnitude > surroundRange * surroundRange * 9)
                            {
                                checkSurround = false;
                            }

                            break;
                    }
                }
                
                yield return new WaitForSeconds(respawnTime);
            }
        }
    }

    IEnumerator SurroundSpawn(Monster monster, int amount, Vector3 startPos)
    {
        float anlge = 360.0f / amount;
        for (int i = 0; i < amount; i++)
        {
            Quaternion rot = Quaternion.Euler(0, anlge * i, 0);
            Vector3 spawnPos = startPos + rot * Vector3.forward * surroundRange;
            GameObject go = ObjectPoolManager.Instance.GetObj(monster).This.gameObject;
            go.transform.position = spawnPos;
            
            if(i % 10 == 0)
                yield return null;
        }
        yield return null;
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
