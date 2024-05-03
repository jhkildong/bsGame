using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSkillParticleCollsion : MonoBehaviour
{
    public float Attack { get; set; }

    private void OnParticleCollision(GameObject other)
    {
        if((1 << other.layer & ((int)BSLayerMasks.Monster | (int)BSLayerMasks.SurroundMonster)) != 0 )
        {
            IDamage damage = other.GetComponent<IDamage>();
            if (damage != null)
            {
                damage.TakeDamageEffect(Attack);
            }
        }
    }
}
