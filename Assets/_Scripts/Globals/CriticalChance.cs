using UnityEngine;
using System.Collections;

public class CriticalChance
{
    private static float randValue = 0;
    public static bool CheckCritical(float critChance)
    {

        if (critChance < 1)
        {
            randValue = Random.value;
            if (randValue <= critChance)
            {
                //Debug.Log("Critical hit!");
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            //Debug.Log("Critical hit!");
            return true;
        }
    }
}
