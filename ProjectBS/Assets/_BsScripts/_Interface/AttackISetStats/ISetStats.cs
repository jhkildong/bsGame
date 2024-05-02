using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISetPointStats
{
    void SetPointStats(float atk = 1, float radius = 1, float size = 1, float speed = 1, float atkDelay = 1,float hitDelay = 1, float durTime = 1); //0501
}

public interface ISetProjectileStats
{
    void SetProjectileStats(float atk = 1, float size = 1, float speed = 1,float range = 1, bool canPenetrate = false, int penetrateCount = 0);
}

public interface ISetMeeleStats
{
    void SetMeeleStats(float atk = 1, float radius = 1, float size = 1, float speed = 1, float atkDealy = 1);
}

