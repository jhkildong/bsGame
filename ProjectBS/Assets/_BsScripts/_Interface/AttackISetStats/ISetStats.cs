using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISetPointStats
{
    void SetPointStats(short atk = 1, float radius = 1, float size = 1, float speed = 1, float delay = 1, float durTime = 1);
}

public interface ISetProjectileStats
{
    void SetProjectileStats(short atk = 1, float size = 1, float speed = 1,float range = 1, bool canPenetrate = false, int penetrateCount = 0);
}

public interface ISetMeeleStats
{
    void SetMeeleStats(short atk = 1, float radius = 1, float size = 1, float speed = 1, float atkDealy = 1);
}

