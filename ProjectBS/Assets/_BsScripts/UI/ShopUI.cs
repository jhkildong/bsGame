using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : WindowUI
{
    const int MAX_LEVEL = 5;

    public Button Attack;
    public Button AttackSpeed;
    public Button Speed;
    public Button MagnetFieldRange;
    public Button Hp;
    public Button ExpBonus;

    private Button[] buttons;

    private Image[] attackCount = new Image[MAX_LEVEL];
    private Image[] attackSpeedCount = new Image[MAX_LEVEL];
    private Image[] speedCount = new Image[MAX_LEVEL];
    private Image[] magnetFieldRangeCount = new Image[MAX_LEVEL];
    private Image[] hpCount = new Image[MAX_LEVEL];
    private Image[] expBonusCount = new Image[MAX_LEVEL];



    public Sprite origin;
    public Sprite change;

    public TextMeshProUGUI goldText;

    private void Awake()
    {
        buttons = new Button[] { Attack, AttackSpeed, Speed, MagnetFieldRange, Hp, ExpBonus };

        CopyExecptFirst(Attack.transform.parent.GetComponentsInChildren<Image>(), attackCount);
        CopyExecptFirst(AttackSpeed.transform.parent.GetComponentsInChildren<Image>(), attackSpeedCount);
        CopyExecptFirst(Speed.transform.parent.GetComponentsInChildren<Image>(), speedCount);
        CopyExecptFirst(MagnetFieldRange.transform.parent.GetComponentsInChildren<Image>(), magnetFieldRangeCount);
        CopyExecptFirst(Hp.transform.parent.GetComponentsInChildren<Image>(), hpCount);
        CopyExecptFirst(ExpBonus.transform.parent.GetComponentsInChildren<Image>(), expBonusCount);
        Attack.onClick.AddListener(
            () =>
            {
                GameManager inst = GameManager.Instance;
                inst.ChangeGold(-100 * ++inst.SaveData.Attack);
                SetImageAndButton(inst.SaveData.Attack, attackCount);
                if(inst.SaveData.Attack >= MAX_LEVEL)
                    Attack.interactable = false;
                
            });
        AttackSpeed.onClick.AddListener(() =>
        {
            GameManager inst = GameManager.Instance;
            inst.ChangeGold(-100 * ++inst.SaveData.AkSp);
            SetImageAndButton(inst.SaveData.AkSp, attackSpeedCount);
            if (inst.SaveData.AkSp >= MAX_LEVEL)
                AttackSpeed.interactable = false;
            
        });
        Speed.onClick.AddListener(() =>
        {
            GameManager inst = GameManager.Instance;
            inst.ChangeGold(-100 * ++inst.SaveData.MvSp);
            SetImageAndButton(inst.SaveData.MvSp, speedCount);
            if (inst.SaveData.MvSp >= MAX_LEVEL)
                Speed.interactable = false;
            
        });
        MagnetFieldRange.onClick.AddListener(() =>
        {
            GameManager inst = GameManager.Instance;
            inst.ChangeGold(-100 * ++inst.SaveData.MagnetFieldRange);
            SetImageAndButton(inst.SaveData.MagnetFieldRange, magnetFieldRangeCount);
            if (inst.SaveData.MagnetFieldRange >= MAX_LEVEL)
                MagnetFieldRange.interactable = false;
        });
        Hp.onClick.AddListener(() =>
        {
            GameManager inst = GameManager.Instance;
            inst.ChangeGold(-100 * ++inst.SaveData.MaxHp);
            SetImageAndButton(inst.SaveData.MaxHp, hpCount);
            if (inst.SaveData.MaxHp >= MAX_LEVEL)
                Hp.interactable = false;
            
        });
        ExpBonus.onClick.AddListener(() =>
        {
            GameManager inst = GameManager.Instance;
            inst.ChangeGold(-100 * ++inst.SaveData.ExpBonus);
            SetImageAndButton(inst.SaveData.ExpBonus, expBonusCount);
            if (GameManager.Instance.SaveData.ExpBonus >= MAX_LEVEL)
                ExpBonus.interactable = false;
            
        });
        GameManager.Instance.GoldChangeAct += (gold) => goldText.text = gold.ToString();
    }

    private void CopyExecptFirst(Image[] allImages, Image[] outImages)
    {
        System.Array.Copy(allImages, 1, outImages, 0, allImages.Length - 1);
    }

    private void OnEnable()
    {
        SetImageAndButton(GameManager.Instance.SaveData.Attack, attackCount);
        SetImageAndButton(GameManager.Instance.SaveData.AkSp, attackSpeedCount);
        SetImageAndButton(GameManager.Instance.SaveData.MvSp, speedCount);
        SetImageAndButton(GameManager.Instance.SaveData.MagnetFieldRange, magnetFieldRangeCount);
        SetImageAndButton(GameManager.Instance.SaveData.MaxHp, hpCount);
        SetImageAndButton(GameManager.Instance.SaveData.ExpBonus, expBonusCount);

        goldText.text = GameManager.Instance.CurGold().ToString();
    }


    void SetImageAndButton(int count, Image[] images)
    {
        int i = 0;
        for(i = 0; i < count; i++)
        {
            images[i].sprite = change;
        }
        for (; i < images.Length; i++)
        {
            images[i].sprite = origin;
        }
        CheckValidButton();
    }

    void CheckValidButton()
    {
        GameManager inst = GameManager.Instance;
        Attack.interactable = inst.SaveData.Attack * 100 < inst.CurGold();
        AttackSpeed.interactable = inst.SaveData.AkSp * 100 < inst.CurGold();
        Speed.interactable = inst.SaveData.MvSp * 100 < inst.CurGold();
        MagnetFieldRange.interactable = inst.SaveData.MagnetFieldRange * 100 < inst.CurGold();
        Hp.interactable = inst.SaveData.MaxHp * 100 < inst.CurGold();
        ExpBonus.interactable = inst.SaveData.ExpBonus * 100 < inst.CurGold();
    }

}
