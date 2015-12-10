using System.Collections;

[System.Serializable]
public class BaseAttack : BaseStat
{
    public BaseAttack()
    {
        StatName = "Attack";
        StatDescription = "Represents how much damage a player can do";
        StatType = StatTypes.ATTACK;
        StatBaseValue = 0;
        StatModifiedValue = 0;
    }
}
