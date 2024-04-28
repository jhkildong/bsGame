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
    public static void TakeDamageEffect(this IDamage obj, float dmg, GameObject effectprefab = null, Vector3 myPos = default, float radius = 0.0f)
    {
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
            //position값을 전달 하지 않았을 경우 중앙에 생성
            if(myPos == default)
            {
                effectSpawn = objPos + obj.Height * 0.5f * Vector3.up;
            }
            //position값 전달 했을 경우 계산된 위치에 생성
            else
            {
                Vector3 objTop = objPos + Vector3.up * obj.Height;
                Vector3 v1 = objPos - myPos;    //내 중앙에서 IDamage 위치까지의 벡터
                Vector3 v2 = objTop - myPos;    //내 중앙에서 IDamage의 상단까지의 벡터
                if (radius == 0) radius = v1.magnitude * 0.5f;  //거리값이 설정이 안된 경우 두 지점의 중간값으로 설정
                v1.Normalize();
                v2.Normalize();
                float angle = Vector3.Angle(v1, v2);
                float spawnHeight = Mathf.Tan(angle * Mathf.Deg2Rad) * radius;    //h = tan(θ) * r
                effectSpawn = myPos + v1 * radius + Vector3.up * spawnHeight;
            }
            //이펙트 생성
            UIManager.Instance.testcode(effectprefab, effectSpawn);
        }
    }
}
