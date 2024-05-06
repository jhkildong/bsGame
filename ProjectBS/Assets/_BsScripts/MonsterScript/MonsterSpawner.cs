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
    [SerializeField, ReadOnly]
    private MonsterData[] monsterDatas;                         //몬스터 데이터(scriptable object) Resource폴더에서 로드
    private Dictionary<int, NormalMonster> normalMonsterDict = new Dictionary<int, NormalMonster>();    //ObjectPoolManager에 등록할 몬스터의 사본
    private Dictionary<int, BossMonster> bossMonsterDict = new Dictionary<int, BossMonster>();
    private WeightedRandomPicker<NormalMonster> monsterWeightRandom = new WeightedRandomPicker<NormalMonster>();
    private List<int> stageInBossIdList = new List<int>();
    [SerializeField] private float respawnDist = 20.0f;
    [SerializeField] private float respawnTime = 1.5f;
    private float nightRespawnTime;
    private float bossRespawnTime = 30.0f;

    private Coroutine spawnCoroutine;

    public Stage CurStage
    {
        get => _curStage;
        set
        {
            if (_curStage != value)
            {
                _curStage = value;
                if(spawnCoroutine != null)
                {
                    StopAllCoroutines();
                    spawnCoroutine = null;
                }
                spawnCoroutine = StartCoroutine(RandomMonsterSpawnByStage(_curStage));
                Debug.Log("Stage Change : " + _curStage);
            }
        }
    }
    [SerializeField] private Stage _curStage;


    private int init = 100;
    private int max = 100;

    // Start is called before the first frame update
    void Start()
    {
        nightRespawnTime = 1.0f;
        monsterDatas = Resources.LoadAll<MonsterData>(FilePath.Monsters);

        DataSetPool(monsterDatas);
        spawnCoroutine = StartCoroutine(RandomMonsterSpawnByStage(Stage.Stage1));
        GameManager.Instance.ChangeDayAct += (day) =>
        {
            if (day >= 10)
                day = 10;
            CurStage = (Stage)day;
        };
        GameManager.Instance.ChangeAMPMAct += (state) => { nightRespawnTime = state ? 0.5f : 1.0f; };

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
            Monster monster = data.CreateClone();   //데이터를 기반으로 사본생성
            if (monster is BossMonster bossMonster)
            {
                bossMonsterDict.Add(data.ID, bossMonster);                       //사본을 리스트에 추가(추가 호출을 위해)
                ObjectPoolManager.Instance.SetPool(monster, 10, 10);
            }
            else if (monster is NormalMonster normalMonster)
            {
                normalMonsterDict.Add(data.ID, normalMonster);                   //사본을 리스트에 추가(추가 호출을 위해)
                ObjectPoolManager.Instance.SetPool(monster, init, max);
            }
            ObjectPoolManager.Instance.ReleaseObj(monster);
        }
    }

    public void Test()
    {
        CurStage += 1;
    }


    //임시 surroundMonster체크, 범위설정
    Vector3 tempPos;

    private IEnumerator RandomMonsterSpawnByStage(Stage stage)
    {
        var WaitforSeconds = new WaitForSeconds(respawnTime * nightRespawnTime);
        monsterWeightRandom = new WeightedRandomPicker<NormalMonster>();

        foreach (int id in StageMonsterIdDict[stage])
        {
            if (normalMonsterDict.ContainsKey(id))
            {
                monsterWeightRandom.Add(normalMonsterDict[id], normalMonsterDict[id].NormalData.Weight);
            }
            else if (bossMonsterDict.ContainsKey(id))
            {
                stageInBossIdList.Add(id);
            }
        }
        if (stageInBossIdList.Count > 0)
        {
            StartCoroutine(BossMonsterSpawn());
        }
        while (true)
        {
            NormalMonster spawnMonster = monsterWeightRandom.GetRandomPick();
            switch (spawnMonster.NormalData.Type)
            {
                case var type when type.HasFlag(MonsterType.Single):
                    SingleSpawn(spawnMonster);
                    break;
                case var type when type.HasFlag(MonsterType.Group):
                    GroupSpawn(spawnMonster);
                    break;
                case var type when type.HasFlag(MonsterType.Surround):
                    float surroundRange = (spawnMonster as SurroundMonster).SurroundData.SurroundRange;
                    StartCoroutine(SurroundSpawn(spawnMonster, surroundRange));
                    monsterWeightRandom.Remove(spawnMonster);
                    tempPos = transform.position;
                    StartCoroutine(SurronudSpawnCheck(spawnMonster, surroundRange));
                    break;
            }
            yield return WaitforSeconds;
        }
    }

    private Vector3 GetRandomPosInCircle(Vector3 thisPos)
    {
        Random.InitState((int)(System.DateTime.Now.Ticks % int.MaxValue));
        float rndAngle = Random.value * Mathf.PI * 2.0f;
        Vector3 rndPos = new Vector3(Mathf.Cos(rndAngle), 0f, Mathf.Sin(rndAngle)) * respawnDist;
        rndPos += thisPos;
        return rndPos;
    }

    private void SingleSpawn(NormalMonster monster)
    {
        GameObject go;
        go = ObjectPoolManager.Instance.GetObj(monster).This.gameObject;
        go.transform.position = GetRandomPosInCircle(transform.position);
    }

    private void GroupSpawn(NormalMonster monster)
    {
        for (int i = 0; i < (monster as GroupMonster).GroupData.Amount; ++i)
        {
            Random.InitState((int)(System.DateTime.Now.Ticks % int.MaxValue));
            Vector3 addPos = new Vector3(Random.Range(0, 2.0f), 0, Random.Range(0, 2.0f));
            GameObject go = ObjectPoolManager.Instance.GetObj(monster).This.gameObject;
            go.transform.position = GetRandomPosInCircle(transform.position) + addPos;
            if (monster.NormalData.Type.HasFlag(MonsterType.StraightMove))
            {
                go.transform.LookAt(transform.position);
            }
        }
    }

    IEnumerator SurroundSpawn(NormalMonster monster, float range)
    {
        int amount = 100;
        Vector3 startPos = transform.position;


        float anlge = 360.0f / amount;
        for (int i = 0; i < amount; i++)
        {
            Quaternion rot = Quaternion.Euler(0, anlge * i, 0);
            Vector3 spawnPos = startPos + rot * Vector3.forward * range;
            GameObject go = ObjectPoolManager.Instance.GetObj(monster).This.gameObject;
            go.transform.position = spawnPos;

            if (i % 10 == 0)
                yield return null;
        }
        yield break;
    }

    IEnumerator SurronudSpawnCheck(NormalMonster monster, float range)
    {
        var WaitforSeconds = new WaitForSeconds(10.0f);
        while (true)
        {
            if ((transform.position - tempPos).magnitude > range * 2.5f)
            {
                monsterWeightRandom.Add(monster, monster.NormalData.Weight);
                yield break;
            }
            yield return WaitforSeconds;
        }
    }
    IEnumerator BossMonsterSpawn()
    {
        while (true)
        {
            foreach (var Id in stageInBossIdList)
            {
                Monster monster = bossMonsterDict[Id];
                GameObject go;
                go = ObjectPoolManager.Instance.GetObj(monster).This.gameObject;
                go.transform.position = GetRandomPosInCircle(transform.position);

                yield return new WaitForSeconds(bossRespawnTime);
            }
        }
    }

    private Dictionary<Stage, HashSet<int>> StageMonsterIdDict = new Dictionary<Stage, HashSet<int>>()
    {
        { Stage.Stage1,  new HashSet<int>(){0, 1 }                  },
        { Stage.Stage2,  new HashSet<int>(){0, 1, 2}                },
        { Stage.Stage3,  new HashSet<int>(){0, 1, 2}                },
        { Stage.Stage4,  new HashSet<int>(){0, 2, 3}                },
        { Stage.Stage5,  new HashSet<int>(){0, 2, 3, 6,    }        },
        { Stage.Stage6,  new HashSet<int>(){0, 2, 3, 7, 500}        },
        { Stage.Stage7,  new HashSet<int>(){0, 2, 3, 4, 6, 500}     },
        { Stage.Stage8,  new HashSet<int>(){0, 2, 3, 5, 6, 500}     },
        { Stage.Stage9,  new HashSet<int>(){0, 2, 3, 4, 7, 9, 500}  },
        { Stage.Stage10, new HashSet<int>(){0, 2, 3, 4, 5, 7, 9, 500}},
    };
}

public enum Stage
{
    Stage1,
    Stage2,
    Stage3,
    Stage4,
    Stage5,
    Stage6,
    Stage7,
    Stage8,
    Stage9,
    Stage10,
}