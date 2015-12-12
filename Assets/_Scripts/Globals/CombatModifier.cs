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

    // Animation
    static int isCriticalHash = Animator.StringToHash("isCritical");

    // Use this for initialization
    void Awake ()
    {
		damageText = damageTextObject.transform.GetChild(0).GetComponent<Text>();
	}

    public static void ProcessAttack(GameObject Source, float Damage, bool didCrit, GameObject Target)
    {
        float adjustedDamage = ModifyDamage(Source, Damage, didCrit, Target);

        Target.GetComponent<Health>().AdjustHP(adjustedDamage * -1);

        DisplayCombatText(Source, Target, didCrit, damageText.transform.parent, Target.transform.position + Target.transform.up * 1.5f, adjustedDamage.ToString());

        setAttacker(Source, Target);

        //Debug.Log(Source.GetComponent<BaseCharacter>().Name + " dealt " + adjustedDamage + " damage to " + Target.GetComponent<BaseCharacter>().Name);
    }

    // returns the final applied damage after modifying calculations
    private static int ModifyDamage(GameObject source, float damage, bool didCrit, GameObject target)
    {
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
        int adjustedDamage = Mathf.RoundToInt(modifiedAttack * damageReduction);

        // We do not want the attack to heal the damage if the damage is negative.
        if (adjustedDamage <= 0)
            adjustedDamage = 0;

        return adjustedDamage;
    }

	private static void DisplayCombatText(GameObject source, GameObject target, bool didCrit, Transform textObject, Vector3 displayPosition, string displayText)
	{
        //  Create the combat text object in space
        Transform tempTextObject = Instantiate(textObject, displayPosition, Quaternion.identity) as Transform; ;
        if (didCrit)
            tempTextObject.GetComponent<Animator>().SetBool(isCriticalHash, true);
        else
            tempTextObject.GetComponent<Animator>().SetBool(isCriticalHash, false);

        Text tempText = tempTextObject.GetChild(0).GetComponent<Text>();
        tempText.text = displayText;
        
        // Change text color based on source type
        if(didCrit)
            tempText.color = Color.yellow;
        else if (source.CompareTag(Tags.Player))
           tempText.color = Color.white;
        else if(source.CompareTag(Tags.Enemy))
            tempText.color = Color.red;
    }

    public static void setAttacker(GameObject Source, GameObject Target)
    {
        // If a player attacks an enemy, set their enemys target to attacker
        if (Source.CompareTag(Tags.Player) && Target.CompareTag(Tags.Enemy))
        {
            AIStatePattern tempState = Target.GetComponent<AIStatePattern>();
            if(tempState.canChase || tempState.canAttack)
            {
                tempState.chaseTarget = Source.transform;
                tempState.currentState = tempState.chaseState;
            }
        }
    }
}
