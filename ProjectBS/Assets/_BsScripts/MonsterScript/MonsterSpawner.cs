using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    private MonsterData[] normalMonsterDatas;                         //���� ������(scriptable object) Resource�������� �ε�
    [SerializeField, ReadOnly]
    private BossMonsterData[] bossMonsterDatas;                //�������� ������(scriptable object) Resource�������� �ε�
    private List<NormalMonster> normalMonsterList = new List<NormalMonster>();    //ObjectPoolManager�� ����� ������ �纻
    private List<BossMonster> bossMonsterList = new List<BossMonster>();
    public float respawnDist = 20.0f;
    public bool applyRespawn;
    public float respawnTime = 0.5f;
    public float bossRespawnTime = 20.0f;


    public int init = 10;
    public int max = 10;

    // Start is called before the first frame update
    void Start()
    {
        normalMonsterDatas = Resources.LoadAll<MonsterData>(FilePath.Monsters);
        bossMonsterDatas = Resources.LoadAll<BossMonsterData>(FilePath.BossMonsters);

        DataSetPool(normalMonsterDatas);
        DataSetPool(bossMonsterDatas);

        applyRespawn = true;
        StartCoroutine(NormalMonsterSpawn());
        //StartCoroutine(BossMonsterSpawn());
        StartCoroutine(TestSpawn());
    }

    void DataSetPool(MonsterData[] datas)
    {
        foreach (MonsterData data in datas)
        {
            if (data.Prefab == null)
            {
                Debug.Log("MonsterData Prefab is null");
                continue;
            }
            Monster monster = data.CreateClone();   //�����͸� ������� �纻����
            if(monster is BossMonster bossMonster)
            {
                bossMonsterList.Add(bossMonster);                       //�纻�� ����Ʈ�� �߰�(�߰� ȣ���� ����)
                ObjectPoolManager.Instance.SetPool(monster, 10, 10);
            }
            else if(monster is NormalMonster normalMonster)
            {
                normalMonsterList.Add(normalMonster);                   //�纻�� ����Ʈ�� �߰�(�߰� ȣ���� ����)
                ObjectPoolManager.Instance.SetPool(monster, init, max);
            }
            ObjectPoolManager.Instance.ReleaseObj(monster);
        }
    }


    //�ӽ� surroundMonsterüũ, ��������
    bool checkSurround = false;
    float surroundRange = 30.0f;
    Vector3 tempPos;

    IEnumerator NormalMonsterSpawn()
    {
        while(true)
        {
            foreach (NormalMonster monster in normalMonsterList)
            {
                float rndAngle = Random.value * Mathf.PI * 2.0f;
                Vector3 rndPos = new Vector3(Mathf.Cos(rndAngle), 0f, Mathf.Sin(rndAngle)) * respawnDist;
                rndPos += transform.position;
                GameObject go;
                var MonsterType = monster.NormalData.Type;
                switch (MonsterType)
                {
                    case var type when type.HasFlag(MonsterType.Single):
                        go = ObjectPoolManager.Instance.GetObj(monster).This.gameObject;
                        go.transform.position = rndPos;
                        break;
                    case var type when type.HasFlag(MonsterType.Group):
                        for (int i = 0; i < (monster as GroupMonster).GroupData.Amount; ++i)
                        {
                            Vector3 addPos = new Vector3(Random.Range(0, 2.0f), 0, Random.Range(0, 2.0f));
                            go = ObjectPoolManager.Instance.GetObj(monster).This.gameObject;
                            go.transform.position = rndPos + addPos;
                            if (type.HasFlag(MonsterType.StraightMove))
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

    IEnumerator BossMonsterSpawn()
    {
        while (true)
        {
            foreach (BossMonster monster in bossMonsterList)
            {
                float rndAngle = Random.value * Mathf.PI * 2.0f;
                Vector3 rndPos = new Vector3(Mathf.Cos(rndAngle), 0f, Mathf.Sin(rndAngle)) * respawnDist;
                rndPos += transform.position;
                GameObject go;

                go = ObjectPoolManager.Instance.GetObj(monster).This.gameObject;
                go.transform.position = rndPos;

                yield return new WaitForSeconds(bossRespawnTime);
            }
        }
    }

    IEnumerator TestSpawn()
    {
        Monster test = null;
        foreach(NormalMonster monster in normalMonsterList)
        {
            if (monster is DemolitionMonster dm)
            {
                test = dm;
                break;
            }
        }
        for(int i = 0; i < 5; i++)
        {
            float rndAngle = Random.value * Mathf.PI * 2.0f;
            Vector3 rndPos = new Vector3(Mathf.Cos(rndAngle), 0f, Mathf.Sin(rndAngle)) * respawnDist;
            rndPos += transform.position;
            GameObject go;

            go = ObjectPoolManager.Instance.GetObj(test).This.gameObject;
            go.transform.position = rndPos;

            yield return new WaitForSeconds(2f);
        }
    }
}
