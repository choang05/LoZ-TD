using System.Collections;

[System.Serializable]
public class BaseHealth : BaseStat
{
    public BaseHealth()
    {
        StatName = "Health";
        StatDescription = "Represents how much HP a player has";
        StatType = StatTypes.HEALTH;
        StatBaseValue = 0;
        StatModifiedValue = 0;
    }
}
