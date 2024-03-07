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
