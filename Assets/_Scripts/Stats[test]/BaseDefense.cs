using System.Collections;

[System.Serializable]
public class BaseDefense : BaseStat
{
    public BaseDefense()
    {
        StatName = "Defense";
        StatDescription = "Represents how much damage a player can resist";
        StatType = StatTypes.DEFENSE;
        StatBaseValue = 0;
        StatModifiedValue = 0;
    }
}
