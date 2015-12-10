using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour
{
    // Attributes
    [SerializeField] private bool canAttack = true;
    [SerializeField] private bool isAttacking = false;

    // Scripts
    private PlayerMotor _playerMotor;
    private BaseCharacter _baseCharacter;
    private TargetsInSight _targetsInSight;

    // Animation
    private Animator animator;
    AnimatorStateInfo stateInfo;
    List<int> attackHashes = new List<int>();
    int hashIndex = 0;
    static int attackHash_0 = Animator.StringToHash("Attack0");
    static int attackHash_1 = Animator.StringToHash("Attack1");
    static int attackStateHash_0 = Animator.StringToHash("Base Layer.Attack0");
    static int attackStateHash_1 = Animator.StringToHash("Base Layer.Attack1");

    void Awake()
    {
        _playerMotor = GetComponent<PlayerMotor>();
        _baseCharacter = GetComponent<BaseCharacter>();
        _targetsInSight = GetComponentInChildren<TargetsInSight>();
        animator = transform.GetComponentInChildren<Animator>();
    }

    // Use this for initialization
    void Start ()
    {
        attackHashes.Add(attackHash_0);
        attackHashes.Add(attackHash_1);
    }
	
	// Update is called once per frame
	void Update ()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if(canAttack && !isAttacking)
        {
            if (Input.GetButton("Fire1") || stateInfo.fullPathHash == attackHashes[hashIndex])
            {
                StartCoroutine(Attack());
            }
            else
            {
                _playerMotor.CanMove = true;
                _playerMotor.CanRotate = true;
                animator.SetBool(attackHashes[hashIndex], false);
            }
        }
    }

    IEnumerator Attack()
    {
        float animationSpeed = 2;
        float eventDelay = .2f / animationSpeed;
        float animationDuration = 1.15f / animationSpeed;

        isAttacking = true;
        _playerMotor.CanMove = false;
        _playerMotor.CanRotate = false;
        animator.SetBool(attackHashes[hashIndex], true);

        yield return new WaitForSeconds(eventDelay);   // animation event delay

        List<GameObject> targets = _targetsInSight.GetTargetsInView();
        for (int i = 0; i < targets.Count; i++)
        {
            // Do attack script
            CombatModifier.ProcessAttack(gameObject, _baseCharacter.Attack, targets[i]);
        }

        yield return new WaitForSeconds(animationDuration - eventDelay);
        isAttacking = false;
    }

    // Actuators and Mutators
    public bool CanAttack
    {
        get{ return canAttack;}
        set{ canAttack = value;}
    }
}
