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
    public static void TakeDamageEffect(this IDamage obj, float dmg, GameObject effectprefab = null, Vector3 myPos = default, float radius = 0.0f)
    {
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
            //position���� ���� ���� �ʾ��� ��� �߾ӿ� ����
            if(myPos == default)
            {
                effectSpawn = objPos + obj.Height * 0.5f * Vector3.up;
            }
            //position�� ���� ���� ��� ���� ��ġ�� ����
            else
            {
                Vector3 objTop = objPos + Vector3.up * obj.Height;
                Vector3 v1 = objPos - myPos;    //�� �߾ӿ��� IDamage ��ġ������ ����
                Vector3 v2 = objTop - myPos;    //�� �߾ӿ��� IDamage�� ��ܱ����� ����
                if (radius == 0) radius = v1.magnitude * 0.5f;  //�Ÿ����� ������ �ȵ� ��� �� ������ �߰������� ����
                v1.Normalize();
                v2.Normalize();
                float angle = Vector3.Angle(v1, v2);
                float spawnHeight = Mathf.Tan(angle * Mathf.Deg2Rad) * radius;    //h = tan(��) * r
                effectSpawn = myPos + v1 * radius + Vector3.up * spawnHeight;
            }
            //����Ʈ ����
            UIManager.Instance.testcode(effectprefab, effectSpawn);
        }
    }
}
