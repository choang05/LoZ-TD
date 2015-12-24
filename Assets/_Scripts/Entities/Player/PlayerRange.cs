using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

public class PlayerRange : MonoBehaviour
{
    // Attributes
    [SerializeField] private bool canAttack = true;     //  Can the player attack
    private bool isAttacking = false;                   //  Is Player currently attacking
    [SerializeField] private bool isCharging = false;   //  Is player currently charging
    [SerializeField] private bool isRolling = false;    //  Is player currently rolling
    [SerializeField]
    private int rollDistanceMultiplier;                 //  The amount of distance covered under a second
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
    [SerializeField] private Transform[] Arrows;        //  Prefab of the projectile
    private int currentArrowIndex = 0;                  //  the current index of the current arrow
    [SerializeField] private Transform currentArrow;

    // Scripts
    private PlayerMotor _playerMotor;
    private PlayerStamina _playerStamina;
    private BaseCharacter _baseCharacter;
    private TargetsInSight _targetsInSight;

    //  Key keybinding
    private string currentSpellKey;
    private const string A1Button = "A1";
    private const string X2Button = "X2";
    private const string Y3Button = "Y3";
    private const string B4Button = "B4";

    // Animation
    private Animator animator;
    //AnimatorStateInfo stateInfo;
    static int isChargingHash = Animator.StringToHash("isChargingShot");
    static int isRollingHash = Animator.StringToHash("isRolling");

    //  Audio
    private AudioSource audioSource;
    public AudioClip[] SoundArray;

    void OnEnable()
    {
        isAttacking = false;
        isCharging = false;
        _playerMotor.ResetMotor();
        //_playerMotor.CanMove = true;
        //_playerMotor.CurrentMoveSpeed = _playerMotor.MaxMoveSpeed;
        currentArrow = Arrows[currentArrowIndex];
    }

    void Awake()
    {
        _playerMotor = GetComponent<PlayerMotor>();
        _playerStamina = GetComponent<PlayerStamina>();
        _baseCharacter = GetComponent<BaseCharacter>();
        animator = transform.GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
	
    void Start()
    {
        currentArrow = Arrows[0];
    }

	// Update is called once per frame
	void Update ()
    {
        //  if the shoot button is being held down, start charge state
        if (Input.GetButtonDown("X2") && !isCharging)
        {
            isCharging = true;
            //  Motor
            _playerMotor.CurrentMoveSpeed *= chargeMoveSpeedMultiplier;
            //  Animation
            animator.SetBool(isChargingHash, true);
        }
        //  if the button is no longer held, start the shoot animation
        else if ((Input.GetButtonUp(X2Button) && isCharging))
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
            if(Input.GetButtonDown(A1Button))
            {
                if(!isCharging && !isAttacking && !isRolling && _playerStamina.CurrentStamina >= rollStaminaDrain)
                {
                    isRolling = true;
                    _playerMotor.CanMove = false;
                    _playerMotor.CanRotate = false;
                    _playerStamina.AdjustStamina(-rollStaminaDrain);
                    StartCoroutine(RollMove());
                    //  Animation
                    animator.SetBool(isRollingHash, isRolling);
                }
            }
        }

        //  Input handler
        //  if the Y button is being held down, start held state
        if (Input.GetButtonDown(Y3Button))
        {
            currentSpellKey = Y3Button;
            Cast(1);
        }
        //  if the B button is being held down, start held state
        else if (Input.GetButtonDown(B4Button))
        {
            currentSpellKey = B4Button;
            Cast(2);
        }
    }

    //  Check which spell is associated with which input key
    void Cast(int spellKey)
    {
        if (spellKey == 1)    //  Switch arrows
        {
            CastSwitchArrows();
        }
        /*else if (SpellOrder[spellKey] == Spells[2]) //  FlameThrower
        {
            StartCoroutine(CastFlamethrower());
        }
        else if (SpellOrder[spellKey] == Spells[3] && _playerMana.CurrentMana >= boulderMana)  //  Boulder
        {
            animator.SetInteger(spellTypeHash, 2);
        }
        else if (SpellOrder[spellKey] == Spells[4] && _playerMana.CurrentMana >= iceSpearMana)  //  Ice Spear
        {
            StartCoroutine(CastIceSpear());
        }
        else
            isCasting = false;*/

    }

    //  This method is called from the AnimationEventHelper when the correct frame in the attack has reached
    public void ApplyAttack()
    {
        // Spawn projectile object
        Transform tempProjectile = PoolManager.Pools["Projectiles"].Spawn(currentArrow);
        tempProjectile.transform.position = transform.position + transform.up + transform.forward;
        tempProjectile.rotation = transform.rotation;
        Arrow _arrow = tempProjectile.GetComponent<Arrow>();
        _arrow.Source = gameObject;
        _arrow.MaxBounces = maxBounces;
        _arrow.ProjectileSpeed = projectileSpeed;
        _arrow.IsCrit = CriticalChance.CheckCritical(_baseCharacter.CriticalChance + bonusCritChance);
        _arrow.TargetTag = Tags.Enemy;

        bonusCritChance = 0;  //  Reset the bonus crit chance

        audioSource.pitch = Random.Range(.5f, 1.5f);
        audioSource.PlayOneShot(SoundArray[0], 2);
    }

    public void ResetAttack()
    {
        isAttacking = false;
        _playerMotor.CanMove = true;
        _playerMotor.CanRotate = true;
    }
    public void ResetRoll()
    {
        _playerMotor.CanMove = true;
        _playerMotor.CanRotate = true;
        isRolling = false;
        animator.SetBool(isRollingHash, isRolling);
    }

    IEnumerator RollMove()
    {
        while(isRolling)
        {
            _playerMotor.CharacterController.SimpleMove(transform.TransformDirection(Vector3.forward) * rollDistanceMultiplier * Time.deltaTime);
            yield return null;
        }
    }

    //  --------------  CASTING --------------
    //  -------------- Switch Arrows --------------
    public void CastSwitchArrows()
    {
        currentArrowIndex = (currentArrowIndex + 1) % Arrows.Length;
        currentArrow = Arrows[currentArrowIndex];
        //Debug.Log(currentArrowIndex);
        //  Audio
        audioSource.pitch = Random.Range(.5f, 1.5f);
        audioSource.PlayOneShot(SoundArray[1], 2);
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
    public int MaxBounces
    {
        get { return maxBounces; }
        set { maxBounces = value; }
    }
    public int ProjectileSpeed
    {
        get { return projectileSpeed; }
        set { projectileSpeed = value; }
    }
}
