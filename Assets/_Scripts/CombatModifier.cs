using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class CombatModifier : MonoBehaviour
{
    // Attributes
    private static float armorMultiplier = 0.05f; // The lower = higher armor requirement for reduction
    private static GameObject source;
    private static GameObject target;

    // Combat displays/text
	[SerializeField] private GameObject damageTextObject;
	private static Text damageText;

	// Use this for initialization
	void Start ()
    {
		damageText = damageTextObject.transform.GetChild(0).GetComponent<Text>();
	}

    public static void ProcessAttack(GameObject Source, float Damage, GameObject Target)
    {
        source = Source;
        target = Target;
        float adjustedDamage = ModifyDamage(source, Damage, target);

        target.GetComponent<Health>().AdjustHP(adjustedDamage * -1);
		DisplayCombatText(damageText.transform.parent, target.transform.position + target.transform.up * 1.5f, adjustedDamage.ToString());  

        Debug.Log(source.GetComponent<BaseCharacter>().Name + " dealt " + adjustedDamage + " damage to " + target.GetComponent<BaseCharacter>().Name);
    }

    private static int ModifyDamage(GameObject source, float damage, GameObject target)
    {
        // Get stats from source and target to compute with
        BaseCharacter targetStats = target.GetComponent<BaseCharacter>();
        int targetDefense = targetStats.Defense;

        //  Damage forumla based on DOTA2 with some adjustments.
        //  The damage multiplier for both positive and negative armor: 
        //  Original Damage Multiplier Forumla = 1 - 0.06 * armor ÷ (1 + (0.06 * |armor|)) 
        //  17 Armor = 50%, 0 Armor = 100%, -17 Armor = 150% etc.
        int adjustedDamage = Mathf.RoundToInt(damage * (1 - armorMultiplier * targetDefense / (1 + (armorMultiplier * Mathf.Abs(targetDefense)))));

        //Debug.Log("Reduction %:" + (1 - armorMultiplier * targetDefense / (1 + (armorMultiplier * Mathf.Abs(targetDefense)))) * 100);

        // We do not want the attack to heal the damage is negative.
        if (adjustedDamage <= 0)
            adjustedDamage = 0;

        return adjustedDamage;
    }

	private static void DisplayCombatText(Transform combatObject, Vector3 displayPosition, string displayText)
	{
        //  Create the combat text object in space
		Transform tempText = Instantiate(combatObject, displayPosition, Quaternion.identity) as Transform;
        tempText.GetChild(0).GetComponent<Text>().text = displayText;

        /*
        // Change text color based on target type
        if (target.CompareTag(Tags.Player))
            damageText.color = Color.red;
        else if(target.CompareTag(Tags.Enemy))
            damageText.color = Color.white;

        //damageText.text = displayText;
        */
    }
}
