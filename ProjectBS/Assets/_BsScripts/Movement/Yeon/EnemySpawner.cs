using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
��������
Player
�� EmemySpawner
-> �÷��̾� ��ġ�� �������� ������ �ϱ����� �÷��̾��� �ڽ����� ���

applyRespawn�� ���� ����������
respawnTime���� ������ �ֱ� ����
*/

public class EnemySpawner : MonoBehaviour
{
    public ObjectPoolManager poolManager;
    [SerializeField]
    private MonsterData[] monsterDatas;
    public float respawnDist = 20.0f;
    public bool applyRespawn;
    public float respawnTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
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
            poolManager.GetMonster(md.ID).transform.position = rndPos;
        }
    }
}
