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
    private PlayerSelectWindow playerSelectWindow;
    [SerializeField] Transform Canvas;
    [SerializeField] private MainCameraAction mainCameraAction;

    private void Awake()
    {
        playerPrefab = Resources.Load<GameObject>("Player/Player");
        jobs = Resources.LoadAll<PlayerComponent>("Player/Job");
        playerSelectWindow = Resources.Load<PlayerSelectWindow>("Prefabs/UI/PlayerSelectWindow");
    }
    private void Start()
    {
        playerSelectWindow = Instantiate(playerSelectWindow, Canvas);
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

        playerSelectWindow.playerSelectUI.SetSelectButtons(names);
        for(int i = 0; i < jobs.Length; i++)
        {
            int idx = i;
            playerSelectWindow.playerSelectUI.SetButtonAction(idx, () => SelectJob(idx));
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
        StringBuilder sb = new StringBuilder("Effect/");
        sb.Append(job.gameObject.name.Substring(3));
        string path = sb.ToString();                            //경로: Effect/JobName
        types = Resources.LoadAll<Effect>(path);    
        string[] names = new string[types.Length];
        for (int i = 0; i < types.Length; i++)
        {
            names[i] = types[i].gameObject.name.Substring(5);   //0000_TypeName 형식으로 되있음
        }

        playerSelectWindow.playerSelectUI.SetSelectButtons(names);
        for (int i = 0; i < types.Length; i++)
        {
            int idx = i;
            playerSelectWindow.playerSelectUI.SetButtonAction(idx, () => SelectType(job, idx));
        }
        playerSelectWindow.SetUndoButton(
            () => {
                SetJobSelect();
                playerSelectWindow.HideUndoButton();
            });
    }

    private void SelectType(PlayerComponent job, int idx)
    {
        job.MyEffect = types[idx];
        Player player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
        player.gameObject.name = "Player";
        Instantiate(myJobPrefab, player.transform);
        player.InitPlayerSetting();
        mainCameraAction.Target = player.transform;
        Destroy(playerSelectWindow.gameObject);
    }
    #endregion
}
