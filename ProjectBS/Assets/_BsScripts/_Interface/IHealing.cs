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
    /// ������ ����Ʈ ����
    /// </summary>
    /// <param name="dmg">������ ��ġ</param>
    /// <param name="effectprefab">����Ʈ ������</param>
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
        //������ ����Ʈ ����
        Vector3 damageSpawn = objPos + Vector3.up * obj.Height;
        FloatingFontUI ui = UIManager.Instance.GetUI(UIID.HealFontUI) as FloatingFontUI;
        ui.SetDamageUI((int)dmg, damageSpawn);
        //����Ʈ �������� ���� ��� ����
        if (effectprefab != null)
        {
            Vector3 effectSpawn;
            //�߾ӿ� ����
            effectSpawn = objPos + obj.Height * 0.5f * Vector3.up;
            //����Ʈ ����
            UIManager.Instance.testcode(effectprefab, effectSpawn);
        }
    }
}