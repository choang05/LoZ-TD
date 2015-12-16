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

    //  Scripts
    private BaseCharacter _baseCharacter;
    [SerializeField] private TargetsInSight _lookArea;
    [SerializeField] private TargetsInSight _attackArea;

    //  Animations
    private Animator animator;
    AnimatorStateInfo stateInfo;
    static int attackTriggerHash = Animator.StringToHash("Attack");
    static int attackTypeHash = Animator.StringToHash("MeleeAttackType");

    void Awake()
    {
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
            if (hasMeleeAttack)
                StartCoroutine(MeleeAttackRoutine());
            else if (hasRangeAttack)
                StartCoroutine(RangeAttackRoutine());


            if (attackCooldown != 0)
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

    IEnumerator MeleeAttackRoutine()
    {
        float animationSpeed = 1;
        float animationDuration = .833f / animationSpeed;
        float eventDelay = .25f / animationSpeed;

        //  Animation
        animator.SetInteger(attackTypeHash, Random.Range(0, 2));
        animator.SetTrigger(attackTriggerHash);

        yield return new WaitForSeconds(eventDelay);   // animation event delay

        // Do attack script
        List<GameObject> targets = _attackArea.GetTargetsInView();
        for (int i = 0; i < targets.Count; i++)
        {
            if(targets[i] != null)
                CombatModifier.ProcessAttack(gameObject, null, _baseCharacter.Attack, CriticalChance.CheckCritical(_baseCharacter.CriticalChance), targets[i]);
        }
        yield return new WaitForSeconds(animationDuration - eventDelay);
    }

    IEnumerator RangeAttackRoutine()
    {
        float animationSpeed = 1;
        float animationDuration = .833f / animationSpeed;
        float eventDelay = .25f / animationSpeed;

        //  Animation
        animator.SetInteger(attackTypeHash, Random.Range(0, 2));
        animator.SetTrigger(attackTriggerHash);

        yield return new WaitForSeconds(eventDelay);   // animation event delay

        // Spawn projectile object
        GameObject tempProjectile = Instantiate(projectile, transform.position + transform.up + transform.forward, transform.rotation) as GameObject;
        tempProjectile.GetComponent<Arrow>().SetSource(gameObject, 1, projectileSpeed, CriticalChance.CheckCritical(_baseCharacter.CriticalChance), Tags.Player);
            
        yield return new WaitForSeconds(animationDuration - eventDelay);
    }
}
