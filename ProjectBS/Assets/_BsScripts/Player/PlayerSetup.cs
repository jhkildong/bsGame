using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerSetup : MonoBehaviour
{
    private GameObject playerPrefab;
    private PlayerComponent[] jobs;
    private PlayerAttackType[] types;
    private GameObject[] mageHandTypes;
    private BlessID[] jobBlessIDs;
    private SelectWindow playerSelectWindow;

    private void Awake()
    {
        playerPrefab = Resources.Load<GameObject>(FilePath.PlayerPrefab);
        jobs = Resources.LoadAll<PlayerComponent>(FilePath.Job);
        mageHandTypes = Resources.LoadAll<GameObject>(FilePath.MageHandTypes);
        jobBlessIDs = new BlessID[] { BlessID.WARRIOR, BlessID.ARCHER, BlessID.MAGE, };
    }
    private void Start()
    {
        //UIManager.Instance.SetPool(UIID.DamageUI, 30, 30);
        playerSelectWindow = UIManager.Instance.CreateUI(UIID.PlayerSelectWindow, CanvasType.Canvas) as SelectWindow;
        SetJobSelect();
        gameObject.SetActive(false);
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
        string path = sb.ToString();                            //경로: AttackType/JobName
        types = Resources.LoadAll<PlayerAttackType>(path);    
        string[] names = new string[types.Length];
        for (int i = 0; i < types.Length; i++)
        {
            names[i] = types[i].Name;
        }

        //선택 버튼설정
        playerSelectWindow.SelectButtons.SetButtonName(names);
        for (int i = 0; i < types.Length; i++)
        {
            int idx = i;
            playerSelectWindow.SelectButtons.SetButtonAction(idx, () => SelectType(job, idx));
        }
        //Undo버튼 설정
        playerSelectWindow.SetUndoButtonAct(
            () => {
                SetJobSelect();
                playerSelectWindow.HideUndoButton();
            });
    }

    private PlayerComponent tempJob;
    private int tempIdx;

    private void LoadSceneAndInstantiateSelectedType(PlayerComponent job, int idx)
    {
        tempJob = job;
        tempIdx = idx;
        // 씬 로드
        Loading.LoadScene(3);

        // 씬이 완전히 로드된 후에 게임 오브젝트 생성
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 3)
        {
            SelectType(tempJob, tempIdx);

            // 이벤트 핸들러 제거
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }



    private void SelectType(PlayerComponent job, int idx)
    {
        StringBuilder sb = new StringBuilder(FilePath.AttackType);
        sb.Append("/");
        sb.Append(job.GetType().Name);
        sb.Append("/Skill");
        string path = sb.ToString();        //경로: AttackType/JobName/Skill
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
        Camera.main.GetComponent<MainCameraAction>().Target = player.transform;
        //플레이어 선택창 제거
        Destroy(playerSelectWindow.gameObject);

        UIManager.Instance.CreateUI(UIID.SkillIconUI, CanvasType.DynamicCanvas);
    }
    #endregion
}
