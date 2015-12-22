using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMelee : MonoBehaviour
{
    // Attributes
    [SerializeField]
    private bool canAttack = true;
    [SerializeField]
    private bool isAttacking = false;
    [SerializeField]
    private bool isBlocking = false;
    [SerializeField] [Range(0, 1)]
    private float blockMoveSpeedMultiplier = 1;
    [SerializeField] [Range(0, 180)]
    private int blockingAngle = 80; 
    [SerializeField] private float blockWindowTime = 1;
    private bool isPerfectBlock;

    // Scripts
    private PlayerMotor _playerMotor;
    private PlayerShield _playerShield;
    private BaseCharacter _baseCharacter;
    private TargetsInSight _targetsInSight;

    // Animation
    private Animator animator;
    //AnimatorStateInfo stateInfo;
    static int attackTriggerHash = Animator.StringToHash("AttackTrigger");
    static int attackTypeHash = Animator.StringToHash("MeleeAttackType");
    static int isBlockingHash = Animator.StringToHash("isBlocking");

    //  Audio
    private AudioSource audioSource;

    void OnEnable()
    {
        isAttacking = false;
        isBlocking = false;
        _playerMotor.ResetMotor();
        //_playerMotor.CanMove = true;
        //_playerMotor.CanRotate = true;
        //_playerMotor.CurrentMoveSpeed = _playerMotor.MaxMoveSpeed;
    }

    void Awake()
    {
        _playerMotor = GetComponent<PlayerMotor>();
        _playerShield = GetComponent<PlayerShield>();
        _baseCharacter = GetComponent<BaseCharacter>();
        _targetsInSight = GetComponentInChildren<TargetsInSight>();
        animator = transform.GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // Attacking
        if(canAttack && !isAttacking)
        {
            if (Input.GetButton("X"))
            {
                if (!isAttacking)
                {
                    isAttacking = true;
                    _playerMotor.CanMove = false;
                    _playerMotor.CanRotate = false;

                    //  Animation
                    animator.SetInteger(attackTypeHash, Random.Range(0, 2));
                    animator.SetTrigger(attackTriggerHash);
                    //Attack();
                }
            }
        }

        // Blocking 
        if (Input.GetButton("A") && _playerShield.CurrentShieldHP > 0)
        {
            //Debug.Log("Blocking: " + isBlocking);
            if (!isBlocking && !isAttacking)
            {
                isBlocking = true;
                StartCoroutine(PerfectBlockWindow(blockWindowTime));

                _playerMotor.CanRotate = false;
                _playerMotor.CurrentMoveSpeed *= blockMoveSpeedMultiplier;
                // Animation
                animator.SetBool(isBlockingHash, true);
            }
            else if (isAttacking && isBlocking)
            {
                isBlocking = false;
                _playerMotor.CanRotate = true;
                _playerMotor.CurrentMoveSpeed /= blockMoveSpeedMultiplier;
                // Animation
                animator.SetBool(isBlockingHash, false);
            }
        }
        else if (isBlocking)
        {
            isBlocking = false;
            _playerMotor.CanRotate = true;
            _playerMotor.CurrentMoveSpeed /= blockMoveSpeedMultiplier;
            // Animation
            animator.SetBool(isBlockingHash, false);
        }
    }

    //  This method is called from the AnimationEventHelper when the correct frame in the attack has reached
    public void ApplyAttack()
    {

        List<GameObject> targets = _targetsInSight.GetTargetsInView();
        for (int i = 0; i < targets.Count; i++)
        {
            // Do attack script
            CombatModifier.ProcessAttack(gameObject, null, _baseCharacter.Attack, CriticalChance.CheckCritical(_baseCharacter.CriticalChance), targets[i]);
        }

        audioSource.pitch = Random.Range(.5f, 1.5f);
        audioSource.PlayOneShot(SoundManager.soundArray[2], 2);
    }

    public void ResetAttack()
    {
        _playerMotor.CanMove = true;
        if (!isBlocking)
            _playerMotor.CanRotate = true;
        isAttacking = false;
    }

    IEnumerator PerfectBlockWindow(float maxTime)
    {
        isPerfectBlock = true;
        yield return new WaitForSeconds(maxTime);
        isPerfectBlock = false;
    }

    //  Process the block after it has been triggered, block levels: 1 = normal block, 2 = perfect block
    public void ProcessBlock(int blockLevel)
    {
        audioSource.pitch = Random.Range(.5f, 1.5f);
        //  Normal Block
        if (blockLevel == 1)
        {
            _playerShield.AdjustShieldHP(-25);
            _playerShield.ResetCooldown();
            audioSource.PlayOneShot(SoundManager.soundArray[1], 2);
        } 
        //  Perfect block
        else if (blockLevel == 2)
        {
            audioSource.PlayOneShot(SoundManager.soundArray[0], 2);
        }

    }

    // Actuators and Mutators
    public bool CanAttack
    {
        get{ return canAttack;}
        set{ canAttack = value;}
    }
    public bool IsBlocking
    {
        get { return isBlocking; }
        //set { isBlocking = value; }
    }
    public int GetBlockingAngle
    {
        get { return blockingAngle; }
        set { blockingAngle = value; }
    }
    public float GetBlockWindowTime
    {
        get { return blockWindowTime; }
        set { blockWindowTime = value; }
    }
    public bool IsPerfectBlock
    {
        get { return isPerfectBlock; }
        //set { isPerfectBlock = value; }
    }
}
