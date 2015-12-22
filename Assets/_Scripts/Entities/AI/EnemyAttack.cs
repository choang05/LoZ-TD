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
    [SerializeField] private int projectileSpeed;
    [SerializeField] private float attackCooldown = 2;
    private bool isAttackOnCooldown = false;
    private bool isAttacking = false;

    //  Scripts
    private BaseCharacter _baseCharacter;
    [SerializeField] private TargetsInSight _lookArea;
    [SerializeField] private TargetsInSight _attackArea;

    //  Animations
    private Animator animator;
    static int attackTriggerHash = Animator.StringToHash("AttackTrigger");

    void Awake()
    {
        _baseCharacter = GetComponent<BaseCharacter>();
        animator = transform.GetComponentInChildren<Animator>();
    }

    public void StartAttackAnimation()
    {
        if(canAttack && !isAttackOnCooldown && !isAttacking)
        {
            isAttacking = true;
            //  Animation
            animator.SetTrigger(attackTriggerHash);
        }
        else
        {
            //Debug.Log("Attack on cooldown!");
        }
    }

    public void Attack()
    {
        StartCoroutine(doCooldown(attackCooldown));

        List<GameObject> targets = _attackArea.GetTargetsInView();
        for (int i = 0; i < targets.Count; i++)
        {
            CombatModifier.ProcessAttack(gameObject, null, _baseCharacter.Attack, CriticalChance.CheckCritical(_baseCharacter.CriticalChance), targets[i]);
        }
    }

    IEnumerator doCooldown(float cooldownTime)
    {
        if(!isAttackOnCooldown)
            isAttackOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        if(isAttackOnCooldown)
            isAttackOnCooldown = false;
    }

    public void ResetAttack()
    {
        isAttacking = false;
    }

    IEnumerator RangeAttackRoutine()
    {

        //  Animation
        //animator.SetInteger(attackTypeHash, Random.Range(0, 2));
        //animator.SetTrigger(attackTriggerHash);

        yield return new WaitForSeconds(0);   // animation event delay

        // Spawn projectile object
        GameObject tempProjectile = Instantiate(projectile, transform.position + transform.up + transform.forward, transform.rotation) as GameObject;
        Arrow _arrow = tempProjectile.GetComponent<Arrow>();
        _arrow.Source = gameObject;
        _arrow.ProjectileSpeed = projectileSpeed;
        _arrow.IsCrit = CriticalChance.CheckCritical(_baseCharacter.CriticalChance);
        _arrow.TargetTag = Tags.Player;
        
        yield return new WaitForSeconds(0);
    }

    // Actuators and Mutators
    public bool IsAttacking
    {
        get { return isAttacking; }
        //set { isAttacking = value; }
    }
}
