using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    private GameObject playerPrefab;
    private PlayerComponent[] jobs;
    private Effect[] types;
    private GameObject myJobPrefab;
    private SelectWindow playerSelectWindow;
    [SerializeField] Transform Canvas;
    [SerializeField] private MainCameraAction mainCameraAction;

    private void Awake()
    {
        playerPrefab = Resources.Load<GameObject>(FilePath.PlayerPrefab);
        jobs = Resources.LoadAll<PlayerComponent>(FilePath.Job);
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
            names[i] = jobs[i].gameObject.name.Substring(3);    //00_JobName 형식으로 되있음
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
        myJobPrefab = jobs[idx].gameObject;
        SetTypeSelect(jobs[idx]);
    }
    #endregion

    #region Select Type
    private void SetTypeSelect(PlayerComponent job)
    {
        StringBuilder sb = new StringBuilder(FilePath.AttackType);
        sb.Append("/");
        sb.Append(job.gameObject.name.Substring(3));
        string path = sb.ToString();                            //경로: AttackType/JobName
        types = Resources.LoadAll<Effect>(path);    
        string[] names = new string[types.Length];
        for (int i = 0; i < types.Length; i++)
        {
            names[i] = types[i].gameObject.name.Substring(5);   //0000_TypeName 형식으로 되있음
        }

        playerSelectWindow.SelectButtons.SetButtonName(names);
        for (int i = 0; i < types.Length; i++)
        {
            int idx = i;
            playerSelectWindow.SelectButtons.SetButtonAction(idx, () => SelectType(job, idx));
        }
        playerSelectWindow.SetUndoButton(
            () => {
                SetJobSelect();
                playerSelectWindow.HideUndoButton();
            });
    }

    private void SelectType(PlayerComponent job, int idx)
    {
        //이펙트설정
        job.MyEffect = types[idx];
        //플레이어 생성
        Player player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
        //플레이어 이름 설정
        player.gameObject.name = "Player";
        //직업 생성
        Instantiate(myJobPrefab, player.RotatingBody);
        //플레이어 초기설정(job(PlayerComponent)의 데이터를 받아서 설정)
        player.InitPlayerSetting();
        //메인카메라 타겟설정
        mainCameraAction.Target = player.transform;
        //플레이어 선택창 제거
        Destroy(playerSelectWindow.gameObject);
    }

    public void OnPlay()
    {
        Loading.LoadScene(3);
    }
    #endregion
}
