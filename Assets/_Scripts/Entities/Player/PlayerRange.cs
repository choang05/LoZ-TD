using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerRange : MonoBehaviour
{
    // Attributes
    [SerializeField] private bool canAttack = true;     //  Can the player attack
    private bool isAttacking = false;                   //  Is Player currently attacking
    [SerializeField]
    private bool isCharging = false;                    //  Is player currently charging
    [SerializeField]
    private bool isRolling = false;                     //  Is player currently rolling
    [SerializeField]
    private int rollSpeedMultiplier;                    //  Is player currently rolling
    [SerializeField] [Range(0, 1)]
    private float chargeMoveSpeedMultiplier = 1;        //  The applied movement speed multiplier during charge
    [SerializeField] [Range(0, 1)]
    private float chargeCritChanceMultiplier = 1;       //  How fast will the crit chance rise as player charges
    [SerializeField] [Range(0, 1)]
    private float maxBonusChargeCrit;                   //  The maximum bonus percentage the crit can charge up to
    [SerializeField] [Range(0, 1)]
    private float bonusCritChance = 0;                  //  The current amount of bonus crit chance
    [SerializeField]
    private float chargeStaminaDrainRate;               //  Amount of Stamina to drain per second during charge
    [SerializeField] private float rollStaminaDrain;    //  Amount of stamina drained for a roll
    [SerializeField] private int projectileSpeed;       //  How fast does the projectile travel
    [SerializeField] private int maxBounces;            //  How many times can the projectile bounce
    [SerializeField] private GameObject projectile;     //  Prefab of the projectile

    // Scripts
    private PlayerMotor _playerMotor;
    private PlayerStamina _playerStamina;
    private BaseCharacter _baseCharacter;
    private TargetsInSight _targetsInSight;

    // Animation
    private Animator animator;
    //AnimatorStateInfo stateInfo;
    static int attackTriggerHash = Animator.StringToHash("Attack");
    static int isChargingHash = Animator.StringToHash("isChargingShot");
    static int isRollingHash = Animator.StringToHash("isRolling");

    void OnEnable()
    {
        isAttacking = false;
        isCharging = false;
        _playerMotor.ResetMotor();
        //_playerMotor.CanMove = true;
        //_playerMotor.CurrentMoveSpeed = _playerMotor.MaxMoveSpeed;
    }

    void Awake()
    {
        _playerMotor = GetComponent<PlayerMotor>();
        _playerStamina = GetComponent<PlayerStamina>();
        _baseCharacter = GetComponent<BaseCharacter>();
        animator = transform.GetComponentInChildren<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //  if the shoot button is being held down, start charge state
        if (Input.GetButtonDown("Fire1") && !isCharging)
        {
            isCharging = true;
            //  Motor
            _playerMotor.CurrentMoveSpeed *= chargeMoveSpeedMultiplier;
            //  Animation
            animator.SetBool(isChargingHash, true);
        }
        //  if the button is no longer held, start the shoot animation
        else if ((Input.GetButtonUp("Fire1") && isCharging))
        {
            isCharging = false;
            isAttacking = true;
            //  Motor
            _playerMotor.CurrentMoveSpeed /= chargeMoveSpeedMultiplier;
            _playerMotor.CanMove = false;
            _playerMotor.CanRotate = false;
            //  Animation
            animator.SetBool(isChargingHash, isCharging);
        }

        //  If the player has Stamina
        if(_playerStamina.CurrentStamina > 0)
        {
            //  If the player has stamina, isn't attacking, and charging, drain the stamina
            if(isCharging && !isAttacking && bonusCritChance <= maxBonusChargeCrit)
            {
                bonusCritChance += chargeCritChanceMultiplier * Time.deltaTime;
                _playerStamina.AdjustStamina(-chargeStaminaDrainRate * Time.deltaTime);
            }
            //  If the player pushes the roll button, isn't charging, isn't attacking, and isn't rolling
            if(Input.GetButtonDown("Fire2") && !isCharging && !isAttacking && !isRolling)
            {
                isRolling = true;
                _playerMotor.CanMove = false;
                _playerMotor.CanRotate = false;
                _playerStamina.AdjustStamina(-rollStaminaDrain);
                StartCoroutine(RollMove(rollSpeedMultiplier));
                //  Animation
                animator.SetBool(isRollingHash, isRolling);
            }
        }
    }

    //  This method is called from the AnimationEventHelper when the correct frame in the attack has reached
    public void ApplyAttack()
    {
        // Spawn projectile object
        GameObject tempProjectile = Instantiate(projectile, transform.position + transform.up + transform.forward, transform.rotation) as GameObject;
        tempProjectile.GetComponent<Arrow>().SetSource(gameObject, maxBounces, projectileSpeed, CriticalChance.CheckCritical(_baseCharacter.CriticalChance + bonusCritChance), Tags.Enemy);
        bonusCritChance = 0;  //  Reset the bonus crit chance
    }

    public void ResetAttack()
    {
        isAttacking = false;
        _playerMotor.CanMove = true;
        _playerMotor.CanRotate = true;
    }
    public void ResetRoll()
    {
        isRolling = false;
        _playerMotor.CanMove = true;
        _playerMotor.CanRotate = true;
        animator.SetBool(isRollingHash, isRolling);
    }

    IEnumerator RollMove(int rollSpeed)
    {
        while(isRolling)
        {
            _playerMotor.CharacterController.SimpleMove(transform.TransformDirection(Vector3.forward) * rollSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // Actuators and Mutators
    public bool CanAttack
    {
        get{ return canAttack;}
        set{ canAttack = value;}
    }
    public bool IsCharging
    {
        get { return isCharging; }
        //set { isCharging = value; }
    }
}
