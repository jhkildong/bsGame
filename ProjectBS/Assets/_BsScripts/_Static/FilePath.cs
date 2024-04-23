public static class FilePath
{
    #region Player
    /// <summary>Player/Player</summary>
    public const string PlayerPrefab = "Player/Player";
    /// <summary>Player/Job</summary>
    public const string Job = "Player/Job";
    #endregion

    #region AttackType
    /// <summary>AttackType</summary>
    public const string AttackType = "AttackType";
    /// <summary>AttackType/Warrior</summary>
    public const string WarriorAttackTypes = "AttackType/Warrior";
    /// <summary>AttackType/Archer</summary>
    public const string ArcherAttackTypes = "AttackType/Archer";
    /// <summary>AttackType/Mage</summary>
    public const string MageAttackTypes = "AttackType/Mage";
    /// <summary>AttackType/MageHand</summary>
    public const string MageHandTypes = "AttackType/MageHand";
    /// <summary>AttackType/MagicHit</summary>
    public const string MagicHitTypes = "AttackType/MagicHit";
    #endregion
    
    #region Monster
    /// <summary>Monster</summary>
    public const string Monsters = "Monster";
    /// <summary>Monster/BossMonster</summary>
    public const string BossMonsters = "Monster/BossMonster";
    /// <summary>Monster/09_NM_Demolition_Imp</summary>
    public const string Imp = "Monster/09_NM_Demolition_Imp";
    #endregion

    #region UI
    /// <summary>Prefabs/UI/Popup</summary>
    public const string Popup = "Prefabs/UI/Popup";
    /// <summary>Prefabs/UI/PlayerSelectWindow</summary>
    public const string PlayerSelectWindow = "Prefabs/UI/PlayerSelectWindow";
    /// <summary>Prefabs/UI/PlayerUI</summary>
    public const string PlayerUI = "Prefabs/UI/PlayerUI";
    #endregion

#if UNITY_EDITOR
    /// <summary>Assets/_BsData/Resources/BlessLevelTable.json</summary>
    public const string BlessLevelTableJson = "Assets/_BsData/Resources/BlessLevelTable.json";
    /// <summary>Assets/_BsScripts/_Static/Key.cs</summary>
    public const string KeyCs = "Assets/_BsScripts/_Static/Key.cs";
#endif
}
