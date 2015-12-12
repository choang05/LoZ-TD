using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerRangeAttack : MonoBehaviour
{
    // Attributes
    [SerializeField] private bool canAttack = true;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private GameObject projectile;

    // Scripts
    private PlayerMotor _playerMotor;
    private BaseCharacter _baseCharacter;
    private TargetsInSight _targetsInSight;

    // Animation
    private Animator animator;
    //AnimatorStateInfo stateInfo;
    List<int> attackHashes = new List<int>();
    int hashIndex = 0;
    static int attackHash_1 = Animator.StringToHash("Attack1");

    void Awake()
    {
        _playerMotor = GetComponent<PlayerMotor>();
        _baseCharacter = GetComponent<BaseCharacter>();
        animator = transform.GetComponentInChildren<Animator>();
    }

    // Use this for initialization
    void Start ()
    {
        attackHashes.Add(attackHash_1);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if(canAttack)
        {
            if (Input.GetButton("Fire1") && !isAttacking)
            {
                //Debug.Log("Attack");
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        float animationSpeed = 1;
        float animationDuration = .833f / animationSpeed;
        float eventDelay = .25f / animationSpeed;

        isAttacking = true;
        _playerMotor.CanMove = false;
        _playerMotor.CanRotate = false;
        animator.SetTrigger(attackHashes[hashIndex]);

        yield return new WaitForSeconds(eventDelay);   // animation event delay before attack is applied

        // Spawn projectile object
        GameObject tempProjectile = Instantiate(projectile, transform.position + transform.up + transform.forward, transform.rotation) as GameObject;
        tempProjectile.GetComponent<Arrow>().SetSource(gameObject, 1, projectileSpeed, CriticalChance.CheckCritical(_baseCharacter.CriticalChance), Tags.Player);

        yield return new WaitForSeconds(animationDuration - eventDelay);
        _playerMotor.CanMove = true;
        _playerMotor.CanRotate = true;
        isAttacking = false;
    }

    // Actuators and Mutators
    public bool CanAttack
    {
        get{ return canAttack;}
        set{ canAttack = value;}
    }
}
