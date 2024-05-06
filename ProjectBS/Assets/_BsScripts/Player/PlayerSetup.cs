using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerSetup : MonoBehaviour
{
    private GameObject playerPrefab;
    private PlayerAttackType[] types;
    private GameObject[] mageHandTypes;
    private BlessID[] jobBlessIDs;

    private void Awake()
    {
        playerPrefab = Resources.Load<GameObject>(FilePath.PlayerPrefab);
        mageHandTypes = Resources.LoadAll<GameObject>(FilePath.MageHandTypes);
        jobBlessIDs = new BlessID[] { BlessID.WARRIOR, BlessID.ARCHER, BlessID.MAGE, };
    }
    private void Start()
    {
        UIManager.Instance.SetPool(UIID.DamageUI, 30, 30);
        SelectType(GameManager.Instance.myJob, GameManager.Instance.myTypeIdx);
    }


    private void SelectType(PlayerComponent job, int idx)
    {
        StringBuilder sb = new StringBuilder(FilePath.AttackType);
        sb.Append("/");
        sb.Append(job.GetType().Name);
        string path = sb.ToString();                            //경로: AttackType/JobName
        types = Resources.LoadAll<PlayerAttackType>(path);

        sb = new StringBuilder(FilePath.AttackType);
        sb.Append("/");
        sb.Append(job.GetType().Name);
        sb.Append("/Skill");
        path = sb.ToString();        //경로: AttackType/JobName/Skill
        PlayerSkill[] skills = Resources.LoadAll<PlayerSkill>(path);

        //플레이어 생성
        Player player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
        //플레이어 이름 설정
        player.gameObject.name = "Player";
        //직업이펙트 설정
        job.MyEffect = types[idx];
        //직업 생성
        PlayerComponent clone = Instantiate(job, player.RotatingBody);
        //직업 생성 및 설정
        clone.MySkillEffect = Instantiate(skills[idx], clone.transform);
        clone.MySkillEffect.gameObject.SetActive(false);
        //Mage라면 MageHand도 생성
        if (clone is Mage mage)
        {
            GameObject go = Instantiate(mageHandTypes[idx]);
            mage.SetHandEffect(go);
        }
        //플레이어 초기설정(job(PlayerComponent)의 데이터를 받아서 설정)
        player.InitPlayerSetting(clone);
        //직업 축복 생성
        int jobBlessIdx = (int)job.MyJob;
        JobBless jobBless = BlessManager.Instance.CreateBless(jobBlessIDs[jobBlessIdx]) as JobBless;
        BlessManager.Instance.SetJobBlessIcon((int)jobBlessIDs[jobBlessIdx]);
        clone.MyJobBless = jobBless;
        for(int i = 0; i < jobBlessIDs.Length; i++)
        {
            if(i == jobBlessIdx)
                continue;
            BlessManager.Instance.RemoveBlessInSelectPool((int)jobBlessIDs[i]);
        }
        
        MainCameraAction mca = Camera.main.GetComponent<MainCameraAction>();
        if(mca == null)
        {
            mca = Camera.main.gameObject.AddComponent<MainCameraAction>();
        }
        mca.Target = player.transform;

        SkillIcon skillIcon = UIManager.Instance.CreateUI(UIID.SkillIconUI, CanvasType.DynamicCanvas) as SkillIcon;
        skillIcon.SetIcon(jobBlessIdx);
        GameManager.Instance.Player.SetSkillicon(skillIcon);
        player.SetGameManagerBuff();
        Destroy(gameObject);
    }
}
