using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AnimEvent : MonoBehaviour
{
    public event UnityAction AttackAct;
    public event UnityAction DeadAct;
    public event UnityAction<int> SkillAct;
    public event UnityAction<int> ChangeWeaponAct;
    public SoundObject sound;

    public void OnAttack()
    {
        AttackAct?.Invoke();
    }

    public void OnDead()
    {
        DeadAct?.Invoke();
    }

    public void OnSkill(int onSkill)
    {
        SkillAct?.Invoke(onSkill);
    }

    public void OnChangeWeapon(int idx)
    {
        ChangeWeaponAct?.Invoke(idx);
    }
    public void BuildHammerHit() //박지민 추가. 건설 애니메이션 사운드 재생.
    {
        SoundManager.Instance.PlaySound(sound.gameObject, transform.position, sound.ID);
    }
}
