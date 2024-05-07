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
        string path = sb.ToString();                            //���: AttackType/JobName
        types = Resources.LoadAll<PlayerAttackType>(path);

        sb = new StringBuilder(FilePath.AttackType);
        sb.Append("/");
        sb.Append(job.GetType().Name);
        sb.Append("/Skill");
        path = sb.ToString();        //���: AttackType/JobName/Skill
        PlayerSkill[] skills = Resources.LoadAll<PlayerSkill>(path);

        //�÷��̾� ����
        Player player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
        //�÷��̾� �̸� ����
        player.gameObject.name = "Player";
        //��������Ʈ ����
        job.MyEffect = types[idx];
        //���� ����
        PlayerComponent clone = Instantiate(job, player.RotatingBody);
        //���� ���� �� ����
        clone.MySkillEffect = Instantiate(skills[idx], clone.transform);
        clone.MySkillEffect.gameObject.SetActive(false);
        //Mage��� MageHand�� ����
        if (clone is Mage mage)
        {
            GameObject go = Instantiate(mageHandTypes[idx]);
            mage.SetHandEffect(go);
        }
        //�÷��̾� �ʱ⼳��(job(PlayerComponent)�� �����͸� �޾Ƽ� ����)
        player.InitPlayerSetting(clone);
        //���� �ູ ����
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
