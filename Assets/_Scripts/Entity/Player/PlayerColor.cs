using UnityEngine;
using System.Collections;

public class PlayerColor : MonoBehaviour
{
    // Player colors
    [SerializeField]
    private ColorTypes Type;
    public enum ColorTypes
    {
        Red,
        Blue,
        Green,
        Yellow
    }

    public static Color GetPlayerColorType(GameObject player)
    {
        PlayerColor tempType = player.GetComponent<PlayerColor>();
        if (tempType.ColorType == ColorTypes.Red)
            return Color.red;
        else if (tempType.ColorType == ColorTypes.Blue)
            return Color.blue;
        else if (tempType.ColorType == ColorTypes.Green)
            return Color.green;
        else
            return Color.yellow;
    }

    // Actuators and Mutators
    public ColorTypes ColorType
    {
        get { return Type; }
        set { Type = value; }
    }
}
