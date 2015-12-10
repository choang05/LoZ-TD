using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class CombatModifier : MonoBehaviour
{
    // Attributes
    private static float armorMultiplier = 0.05f; // The lower = higher armor requirement for reduction

    // Combat displays/text
	[SerializeField] private GameObject damageTextObject;
	private static Text damageText;

	// Use this for initialization
	void Start ()
    {
		damageText = damageTextObject.transform.GetChild(0).GetComponent<Text>();
	}

    public static void ProcessAttack(GameObject Source, float Damage, bool didCrit, GameObject Target)
    {
        float adjustedDamage = ModifyDamage(Source, Damage, didCrit, Target);

        Target.GetComponent<Health>().AdjustHP(adjustedDamage * -1);
		DisplayCombatText(Target, damageText.transform.parent, Target.transform.position + Target.transform.up * 1.5f, adjustedDamage.ToString());  

        Debug.Log(Source.GetComponent<BaseCharacter>().Name + " dealt " + adjustedDamage + " damage to " + Target.GetComponent<BaseCharacter>().Name);
    }

    // returns the final applied damage after modifying calculations
    private static int ModifyDamage(GameObject source, float damage, bool didCrit, GameObject target)
    {
        int adjustedDamage = 0;
        // Get stats from source and target to compute with
        BaseCharacter sourceStats = source.GetComponent<BaseCharacter>();
        BaseCharacter targetStats = target.GetComponent<BaseCharacter>();
        int targetDefense = targetStats.Defense;

        //  Damage reduction forumla based on DOTA2 with some adjustments.
        //  The damage multiplier for both positive and negative armor: 
        //  Original Damage Multiplier Forumla = 1 - 0.06 * armor ÷ (1 + (0.06 * |armor|)) 
        //  17 Armor = 50%, 0 Armor = 100%, -17 Armor = 150% etc.
        float damageReduction =  (1 - armorMultiplier * targetDefense / (1 + (armorMultiplier * Mathf.Abs(targetDefense))));

        //  Damage forumla before reduction by armor
        float modifiedAttack = damage;
        if (didCrit)
            modifiedAttack *= sourceStats.CritMultiplier;

        //  Final damage calculated after reductions and modifiers
        adjustedDamage = Mathf.RoundToInt(modifiedAttack * damageReduction);

        // We do not want the attack to heal the damage if the damage is negative.
        if (adjustedDamage <= 0)
            adjustedDamage = 0;

        return adjustedDamage;
    }

	private static void DisplayCombatText(GameObject target, Transform textObject, Vector3 displayPosition, string displayText)
	{
        //  Create the combat text object in space
		Transform tempTextObject = Instantiate(textObject, displayPosition, Quaternion.identity) as Transform;
        Text tempText = tempTextObject.GetChild(0).GetComponent<Text>();
        tempText.text = displayText;

        // Change text color based on target type
        if (target.CompareTag(Tags.Player))
            tempText.color = Color.red;
        else if(target.CompareTag(Tags.Enemy))
            tempText.color = Color.white;
    }
}
