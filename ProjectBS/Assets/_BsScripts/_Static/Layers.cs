[System.Flags]
public enum BSLayerMasks
{
    PlayerAttackEffect = 1 << 12,
    PerceptionField = 1 << 13,
    Player = 1 << 14,
    Monster = 1 << 15,
    MagneticField = 1 << 16,
    Item = 1 << 17,
    SurroundMonster = 1<<18,
    Trap = 1 << 22,
    Building = 1 << 24,
    InCompletedBuilding = 1 << 25,
    BuildCheckObject = 1 << 26,
    Ground = 1 << 29
}

public enum BSLayers
{
    PlayerAttackEffect = 12,
    PerceptionField = 13,
    Player = 14,
    Monster = 15,
    MagneticField = 16,
    Item = 17,
    SurroundMonster = 18,
    Trap = 22,
    Building = 24,
    InCompletedBuilding = 25,
    BuildCheckObject = 26,
    Ground = 29
}