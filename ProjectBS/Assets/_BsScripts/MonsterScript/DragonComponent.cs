using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonComponent : MonsterComponent
{
    public DragonFire DragonFire => _dragonFire;
    public ParticleSystem MyEffect => _myEffect;
    [SerializeField] DragonFire _dragonFire;
    [SerializeField] private ParticleSystem _myEffect;
}
