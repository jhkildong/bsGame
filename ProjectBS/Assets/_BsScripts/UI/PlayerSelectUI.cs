using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerSelectUI : MonoBehaviour
{
    private PlayerComponent[] jobs;
    private BlessData[] jobBlesses;
    private PlayerAttackType[] types;
    [SerializeField] private SelectWindow playerSelectWindow;


    private void Awake()
    {
        jobs = Resources.LoadAll<PlayerComponent>(FilePath.Job);
        jobBlesses = Resources.LoadAll<BlessData>(FilePath.JobBless);
    }

    public void SetJobSelect()
    {
        string[] names = new string[jobs.Length];
        string[] descriptions = new string[jobs.Length];
        Sprite[] sprites = new Sprite[jobs.Length];
        for (int i = 0; i < jobs.Length; i++)
        {
            names[i] = jobBlesses[i].Name;
            descriptions[i] = jobBlesses[i].Description;
            sprites[i] = jobBlesses[i].Icon;
        }
        playerSelectWindow.SelectButtons.SetNames(names);
        playerSelectWindow.SelectButtons.SetDecriptions(descriptions);
        playerSelectWindow.SelectButtons.SetImages(sprites);

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
        string[] descriptions = new string[jobs.Length];
        Sprite[] sprites = new Sprite[jobs.Length];
        for (int i = 0; i < types.Length; i++)
        {
            names[i] = types[i].Name;
            descriptions[i] = types[i].Description;
            sprites[i] = types[i].Icon;
        }
        playerSelectWindow.SelectButtons.SetNames(names);
        playerSelectWindow.SelectButtons.SetDecriptions(descriptions);
        playerSelectWindow.SelectButtons.SetImages(sprites);

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