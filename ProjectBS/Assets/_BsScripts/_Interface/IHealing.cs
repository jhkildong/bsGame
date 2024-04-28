using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealing
{
    void ReceiveHeal(float dmg);
    float Height { get; }

    float MaxHp {  get; }
    float CurHp { get; set; }
}

public static class IHealableExtension
{
    /// <summary>
    /// 데미지 이펙트 생성
    /// </summary>
    /// <param name="dmg">데미지 수치</param>
    /// <param name="effectprefab">이펙트 프리팹</param>
    /// <param name="myPos">trasform.position</param>
    public static void ReceiveHealEffect(this IHealing obj, float dmg, GameObject effectprefab = null)
    {
        dmg += Random.Range(-1, 2);
        if (dmg < 1)
            dmg = 1;

        obj.ReceiveHeal(dmg);
        if(obj.CurHp >= obj.MaxHp)
        {
            return;
        }
        MonoBehaviour mono = obj as MonoBehaviour;
        Vector3 objPos = mono.transform.position;
        //데미지 이펙트 생성
        Vector3 damageSpawn = objPos + Vector3.up * obj.Height;
        FloatingFontUI ui = UIManager.Instance.GetUI(UIID.HealFontUI) as FloatingFontUI;
        ui.SetDamageUI((int)dmg, damageSpawn);
        //이펙트 프리팹이 있을 경우 생성
        if (effectprefab != null)
        {
            Vector3 effectSpawn;
            //중앙에 생성
            effectSpawn = objPos + obj.Height * 0.5f * Vector3.up;
            //이펙트 생성
            UIManager.Instance.testcode(effectprefab, effectSpawn);
        }
    }
}