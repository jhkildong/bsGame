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
    /// ������ ����Ʈ ����
    /// </summary>
    /// <param name="dmg">������ ��ġ</param>
    /// <param name="effectprefab">����Ʈ ������</param>
    /// <param name="myPos">trasform.position</param>
    public static void TakeDamageEffect(this IDamage obj, float dmg, GameObject effectprefab = null)
    {
        dmg += Random.Range(-1, 2);
        if (dmg < 1)
            dmg = 1;

        if(obj == null)
        {
            return;
        }
        obj.TakeDamage(dmg);
        MonoBehaviour mono = obj as MonoBehaviour;
        Vector3 objPos = mono.transform.position;
        //������ ����Ʈ ����
        Vector3 damageSpawn = objPos + Vector3.up * obj.Height;
        FloatingFontUI ui = UIManager.Instance.GetUI(UIID.DamageUI) as FloatingFontUI;
        ui.SetDamageUI((int)dmg, damageSpawn);
        //����Ʈ �������� ���� ��� ����
        if(effectprefab != null)
        {
            Vector3 effectSpawn;
            //�߾ӿ� ����
            effectSpawn = objPos + obj.Height * 0.5f * Vector3.up;
            //����Ʈ ����
            UIManager.Instance.testcode(effectprefab, effectSpawn);
        }
    }
}
