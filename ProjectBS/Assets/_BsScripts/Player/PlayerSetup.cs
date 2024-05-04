using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class PlayerSetup : MonoBehaviour
{
    private GameObject playerPrefab;
    private PlayerComponent[] jobs;
    private PlayerAttackType[] types;
    private GameObject[] mageHandTypes;
    private BlessID[] jobBlessIDs;
    private SelectWindow playerSelectWindow;
    [SerializeField] Transform Canvas;
    [SerializeField] private MainCameraAction mainCameraAction;

    private void Awake()
    {
        playerPrefab = Resources.Load<GameObject>(FilePath.PlayerPrefab);
        jobs = Resources.LoadAll<PlayerComponent>(FilePath.Job);
        mageHandTypes = Resources.LoadAll<GameObject>(FilePath.MageHandTypes);
        jobBlessIDs = new BlessID[] { BlessID.WARRIOR, BlessID.ARCHER, BlessID.MAGE, };
    }
    private void Start()
    {
        UIManager.Instance.SetPool(UIID.DamageUI, 30, 30);
        playerSelectWindow = UIManager.Instance.CreateUI(UIID.PlayerSelectWindow, CanvasType.Canvas) as SelectWindow;
        SetJobSelect();
    }

    #region Select Job
    private void SetJobSelect()
    {
        string[] names = new string[jobs.Length];
        for (int i = 0; i < jobs.Length; i++)
        {
            names[i] = jobs[i].MyJob.GetDescription();
        }
        playerSelectWindow.SelectButtons.SetButtonName(names);
        
        for(int i = 0; i < jobs.Length; i++)
        {
            int idx = i;
            playerSelectWindow.SelectButtons.SetButtonAction(idx, () => SelectJob(idx));
        }
    }

    private void SelectJob(int idx)
    {
        SetTypeSelect(jobs[idx]);
    }
    #endregion

    #region Select Type
    private void SetTypeSelect(PlayerComponent job)
    {
        StringBuilder sb = new StringBuilder(FilePath.AttackType);
        sb.Append("/");
        sb.Append(job.GetType().Name);
        string path = sb.ToString();                            //���: AttackType/JobName
        types = Resources.LoadAll<PlayerAttackType>(path);    
        string[] names = new string[types.Length];
        for (int i = 0; i < types.Length; i++)
        {
            names[i] = types[i].Name;
        }

        //���� ��ư����
        playerSelectWindow.SelectButtons.SetButtonName(names);
        for (int i = 0; i < types.Length; i++)
        {
            int idx = i;
            playerSelectWindow.SelectButtons.SetButtonAction(idx, () => SelectType(job, idx));
        }
        //Undo��ư ����
        playerSelectWindow.SetUndoButton(
            () => {
                SetJobSelect();
                playerSelectWindow.HideUndoButton();
            });
    }

    private void SelectType(PlayerComponent job, int idx)
    {
        StringBuilder sb = new StringBuilder(FilePath.AttackType);
        sb.Append("/");
        sb.Append(job.GetType().Name);
        sb.Append("/Skill");
        string path = sb.ToString();        //���: AttackType/JobName/Skill
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
        //����ī�޶� Ÿ�ټ���
        mainCameraAction.Target = player.transform;
        //�÷��̾� ����â ����
        Destroy(playerSelectWindow.gameObject);

        UIManager.Instance.CreateUI(UIID.SkillIconUI, CanvasType.DynamicCanvas);
    }

    public void OnPlay()
    {
        Loading.LoadScene(3);
    }
    #endregion
}
