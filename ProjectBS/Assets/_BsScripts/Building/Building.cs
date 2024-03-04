using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building
{
    public BuildingData Data {get; private set; }

    public Building(BuildingData data) => Data = data;
}
