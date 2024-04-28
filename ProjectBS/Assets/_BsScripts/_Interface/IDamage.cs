using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    void TakeDamage(float dmg);
    float Height { get; }
}

public interface IDamage<T> : IDamage where T : MonoBehaviour
{

}

public static class IDamageExtension
{
    /// <summary>
    /// 데미지 이펙트 생성
    /// </summary>
    /// <param name="dmg">데미지 수치</param>
    /// <param name="effectprefab">이펙트 프리팹</param>
    /// <param name="myPos">trasform.position</param>
    public static void TakeDamageEffect(this IDamage obj, float dmg, GameObject effectprefab = null)
    {
        dmg += Random.Range(-1, 2);
        if (dmg < 1)
            dmg = 1;

        obj.TakeDamage(dmg);
        MonoBehaviour mono = obj as MonoBehaviour;
        Vector3 objPos = mono.transform.position;
        //데미지 이펙트 생성
        Vector3 damageSpawn = objPos + Vector3.up * obj.Height;
        FloatingFontUI ui = UIManager.Instance.GetUI(UIID.DamageUI) as FloatingFontUI;
        ui.SetDamageUI((int)dmg, damageSpawn);
        //이펙트 프리팹이 있을 경우 생성
        if(effectprefab != null)
        {
            Vector3 effectSpawn;
            //중앙에 생성
            effectSpawn = objPos + obj.Height * 0.5f * Vector3.up;
            //이펙트 생성
            UIManager.Instance.testcode(effectprefab, effectSpawn);
        }
    }
}
