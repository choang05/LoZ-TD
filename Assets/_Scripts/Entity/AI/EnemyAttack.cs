using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAttack : MonoBehaviour
{
    //  Attributes
    [SerializeField] private bool canAttack = false;
    [SerializeField] private bool hasRangeAttack = false;
    [SerializeField] private bool hasMeleeAttack = false;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float attackCooldown = 2;
    private bool isAttackOnCooldown = false;

    //  Scripts
    private AIStatePattern _AIStatePattern;
    private BaseCharacter _baseCharacter;
    [SerializeField] private TargetsInSight _lookArea;
    [SerializeField] private TargetsInSight _attackArea;

    //  Animations
    private Animator animator;
    AnimatorStateInfo stateInfo;

    void Awake()
    {
        _AIStatePattern = GetComponent<AIStatePattern>();
        _baseCharacter = GetComponent<BaseCharacter>();
        animator = transform.GetComponentInChildren<Animator>();
    }

    // Use this for initialization
    void Start ()
    {
	    
	}

    public void Attack()
    {
        if(!isAttackOnCooldown && canAttack)
        {
            List<GameObject> targets = _attackArea.GetTargetsInView();
            for (int i = 0; i < targets.Count; i++)
            {
                // Do attack script
                CombatModifier.ProcessAttack(gameObject, _baseCharacter.Attack, CriticalChance.CheckCritical(_baseCharacter.CriticalChance), targets[i]);
            }

            if(attackCooldown != 0)
                StartCoroutine(doCooldown(attackCooldown));
        }
        else
        {
            //Debug.Log("Attack on cooldown!");
        }
    }

    IEnumerator doCooldown(float cooldownTime)
    {
        isAttackOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isAttackOnCooldown = false;
    }
}
