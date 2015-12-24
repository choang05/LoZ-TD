using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using PathologicalGames;

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

    //  Global method that handles all source-to-target damage interactions
    public static void ProcessAttack(GameObject Source, GameObject secondarySource, float Damage, bool didCrit, GameObject Target)
    {

        int adjustedDamage = ModifyDamage(Source, Damage, didCrit, Target);

        //  if attack was blocked
        int blockLevel = GetBlockCheck(Source, Target);
        if(blockLevel == 1)    //  If block was normal, adjust the attack damage
        {
            adjustedDamage = (int)(adjustedDamage * Target.GetComponent<PlayerMelee>().BlockReductionMultiplier);
            //  Process the block
            Target.GetComponent<PlayerMelee>().ProcessBlock(blockLevel, adjustedDamage);
        }
        if (blockLevel == 2)    // if block was perfect
        {
            //  If player does a perfect block and the source is a projectile, 'reflect' the projectile back with adjusted parameters.  
            if(secondarySource)
            {
                PlayerRange _playerRange = Target.GetComponent<PlayerRange>();
                Arrow _arrow = secondarySource.GetComponent<Arrow>();
                secondarySource.transform.LookAt(2 * Source.transform.position - Target.transform.position + Target.transform.up);
                _arrow.ResetProjectile();
                _arrow.Source = _playerRange.gameObject;
                _arrow.MaxBounces = _playerRange.MaxBounces;
                _arrow.ProjectileSpeed = _playerRange.ProjectileSpeed;
                _arrow.IsCrit = CriticalChance.CheckCritical(_playerRange.GetComponent<BaseCharacter>().CriticalChance);
                _arrow.TargetTag = Tags.Enemy;
            }
            //  Process the block
            Target.GetComponent<PlayerMelee>().ProcessBlock(blockLevel, adjustedDamage);
        }
        else  // else if attack was not blocked.
        {
            //  deal the adjusted damage
            Target.GetComponent<Health>().AdjustHP(adjustedDamage * -1);

            //  Extra aftereffects (Processes, Sounds, knockback, etc.)
            if (Target.CompareTag(Tags.Enemy))
            {
                Target.GetComponent<AIStatePattern>().ProcessHit(Source, adjustedDamage);
            }

            //  UI
            DisplayCombatText(Source, Target, didCrit, damageText.transform.parent, Target.transform.position + Target.transform.up * 1.5f, adjustedDamage.ToString());
        }
        //  Set the current target of the attacker
        SetAttacker(Source, Target);

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
        //  Modified damage = Damage +- the attack deviation
        float modifiedDamage = damage + UnityEngine.Random.Range(-sourceStats.AttackDeviation, sourceStats.AttackDeviation + 1);
        if (didCrit)
            modifiedDamage *= sourceStats.CritMultiplier;

        //  Final damage calculated after reductions and modifiers
        int adjustedDamage = Mathf.RoundToInt(modifiedDamage * damageReduction);

        // We do not want the attack to heal the damage if the damage is negative.
        if (adjustedDamage <= 0)
            adjustedDamage = 0;

        return adjustedDamage;
    }

	private static void DisplayCombatText(GameObject source, GameObject target, bool didCrit, Transform textObject, Vector3 displayPosition, string displayText)
	{
        //  Create the combat text object in space
        Transform tempTextObject = PoolManager.Pools["UI"].Spawn(textObject);
        tempTextObject.transform.position = displayPosition;
        //Transform tempTextObject = Instantiate(textObject, displayPosition, Quaternion.identity) as Transform; ;
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

    private static void SetAttacker(GameObject Source, GameObject Target)
    {
        // If a player attacks an enemy, set their enemys target to attacker
        if (Source.CompareTag(Tags.Player) && Target.CompareTag(Tags.Enemy))
        {
            AIStatePattern tempState = Target.GetComponent<AIStatePattern>();
            if(tempState.canChase || tempState.canAttack || tempState.CanFlee)
            {
                tempState.currentTarget = Source.transform;
                if(tempState.canChase)
                    tempState.currentState = tempState.attackState;
            }
        }
    }

    //  Method that checks for blocking and returns the 'level' of blocking based on if they didn't block, if they blocked, and if it was a perfect block (0, 1, 2)
    private static int GetBlockCheck(GameObject attacker, GameObject blocker)
    {
        if(blocker.CompareTag(Tags.Player))
        {
            PlayerMelee _playerMelee = blocker.GetComponent<PlayerMelee>();
            if (_playerMelee.IsBlocking && Vector3.Angle(blocker.transform.forward, attacker.transform.position - blocker.transform.position) <= _playerMelee.GetBlockingAngle)
            {
                if (_playerMelee.IsPerfectBlock)
                {
                    //Debug.Log("Perfect Block!");
                    return 2;
                }
                //Debug.Log("Blocked Attack! " + Vector3.Angle(blocker.transform.forward, attacker.transform.position - blocker.transform.position));
                return 1;
            }
        }
        return 0;
    }
}
