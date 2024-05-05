using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerSelectUI : MonoBehaviour
{
    private PlayerComponent[] jobs;
    private PlayerAttackType[] types;
    [SerializeField] private SelectWindow playerSelectWindow;


    private void Awake()
    {
        jobs = Resources.LoadAll<PlayerComponent>(FilePath.Job);
    }

    public void SetJobSelect()
    {
        string[] names = new string[jobs.Length];
        for (int i = 0; i < jobs.Length; i++)
        {
            names[i] = jobs[i].MyJob.GetDescription();
        }
        playerSelectWindow.SelectButtons.SetButtonName(names);

        for (int i = 0; i < jobs.Length; i++)
        {
            int idx = i;
            playerSelectWindow.SelectButtons.SetButtonAction(idx, () => SelectJob(idx));
        }
    }

    private void SelectJob(int idx)
    {
        SetTypeSelect(jobs[idx]);
    }

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
            playerSelectWindow.SelectButtons.SetButtonAction(idx, () => LoadSceneAndInstantiateSelectedType(job, idx));
        }
        //Undo버튼 설정
        playerSelectWindow.SetUndoButtonAct(
            () => {
                SetJobSelect();
                playerSelectWindow.HideUndoButton();
            });
    }

    private void LoadSceneAndInstantiateSelectedType(PlayerComponent job, int idx)
    {
        GameManager.Instance.SetSelectedInfo(job, idx);
        // 씬 로드
        Loading.LoadScene(2);
    }

}