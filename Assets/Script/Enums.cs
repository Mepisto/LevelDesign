

public enum eLevelDesignCategory
{
    Spawner = 0x0100,
    Trigger = 0x0200,
}

public enum eLevelDesignType
{
    Monster = (int)eLevelDesignCategory.Spawner << 16 | 0x0001,

    SpawnTrigger = (int)eLevelDesignCategory.Trigger << 16 | 0x0011
}

public enum eMessageCategory : int
{
    Trigger = 0x0600,
    Spawner = 0x0601,
}

public enum eMessage : int
{
    None = 0,

    SpawnMonster = eMessageCategory.Spawner << 16 | 0x0001,
    DeadMonster = eMessageCategory.Spawner << 16 | 0x0002,

    TriggerEnter = eLevelDesignCategory.Trigger << 16 | 0x0010,
    TriggerExit = eLevelDesignCategory.Trigger << 16 | 0x0011
}

public enum eNextWaveCondition
{
    SpawnCount,
    DeathCount,
    KillThemAll,
    KillBoss,
    //KillBossAndSpawnObjects,
    //Custom,
    //Time,
}

