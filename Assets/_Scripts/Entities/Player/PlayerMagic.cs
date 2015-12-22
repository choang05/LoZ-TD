using UnityEngine;
using System.Collections;
using PathologicalGames;

public class PlayerMagic : MonoBehaviour
{
    [SerializeField]
    private bool isCasting = false;

    //  Spell Attributes
    [SerializeField] private bool canTeleport;
    [SerializeField] private float teleportCooldown;
    [SerializeField] private float teleportDistance;
    [SerializeField] private int teleportManaDrain;

    public int fireballTravelSpeed;
    public float fireballDuration;

    public float boulderDuration;

    //  Spell Prefabs
    public GameObject[] Spells;
    public GameObject[] SpellOrder;
    private GameObject currentSpell;

    //  Spell keybinding
    private const string aButton = "A";
    private const string xButton = "X";
    private const string yButton = "Y";
    private const string bButton = "B";

    // Scripts
    private PlayerMotor _playerMotor;
    private PlayerMana _playerMana;
    private BaseCharacter _baseCharacter;

    //  Animations
    private Animator animator;
    static int spellTypeHash = Animator.StringToHash("SpellType");
    static int attackTriggerHash = Animator.StringToHash("AttackTrigger");

    //  Audio
    private AudioSource audioSource;

    void OnEnable()
    {
        _playerMotor.CanMove = true;
        _playerMotor.CanRotate = true;
    }

    void Awake()
    {
        _playerMotor = GetComponent<PlayerMotor>();
        _playerMana = GetComponent<PlayerMana>();
        _baseCharacter = GetComponent<BaseCharacter>();

        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //  Teleport (will always be the 'A' button)
        if (canTeleport && Input.GetButtonDown(aButton) && _playerMana.CurrentMana >= teleportManaDrain && !isCasting)
        {
            canTeleport = false;

            //  Audio
            audioSource.pitch = Random.Range(.75f, 1.25f);
            audioSource.PlayOneShot(SoundManager.soundArray[4], 3);

            //  Spawn teleport effect (before)
            Transform tempTeleport = PoolManager.Pools["Spells"].Spawn(Spells[0]);
            tempTeleport.transform.position = transform.position;
            tempTeleport.rotation = transform.rotation;

            _playerMotor.CharacterController.SimpleMove(transform.TransformDirection(Vector3.forward) * teleportDistance);

            //  Spawn teleport effect (after)
            Transform tempTeleport2 = PoolManager.Pools["Spells"].Spawn(Spells[0]);
            tempTeleport2.transform.position = transform.position;
            tempTeleport2.rotation = transform.rotation;

            StartCoroutine(ProcessTeleportCooldown());

            _playerMana.AdjustMana(-teleportManaDrain);
        }

        //  Input handler
        //  if the X button is being held down, start held state
        if (Input.GetButtonDown(xButton) && !isCasting)
        {
            isCasting = true;
        }
        else if (Input.GetButtonUp(xButton) && isCasting) //  if the X button is no longer held, release the held state
        {
            isCasting = false;
            CastSpell(0);
        }
        //  if the Y button is being held down, start held state
        if (Input.GetButtonDown(yButton) && !isCasting)
        {
            isCasting = true;
        }
        else if (Input.GetButtonUp(yButton) && isCasting) //  if the Y button is no longer held, release the held state
        {
            isCasting = false;
            CastSpell(1);
        }
        //  if the B button is being held down, start held state
        if (Input.GetButtonDown(bButton) && !isCasting)
        {
            isCasting = true;
        }
        else if (Input.GetButtonUp(bButton) && isCasting) //  if the B button is no longer held, release the held state
        {
            isCasting = false;
            CastSpell(2);
        }
    }

    void CastSpell(int spellKey)
    {
        if(SpellOrder[spellKey] == Spells[1])       //  Fireball
        { animator.SetInteger(spellTypeHash, 1); }  
        else if(SpellOrder[spellKey] == Spells[2])  //  Boulder
        { animator.SetInteger(spellTypeHash, 2); }
        else if (SpellOrder[spellKey] == Spells[3])
        { }

    }

    public void CastFireball()
    {
        GameObject tempFireball = Instantiate(Spells[1], transform.position + transform.forward + transform.up, transform.rotation) as GameObject;
        Fireball _fireball = tempFireball.GetComponent<Fireball>();
        _fireball.TravelSpeed = fireballTravelSpeed;
        _fireball.Duration = fireballDuration;
        _fireball.Source = gameObject;
        _fireball.Damage = _baseCharacter.Attack;
        _fireball.IsCrit = CriticalChance.CheckCritical(_baseCharacter.CriticalChance);
        _fireball.TargetTag = Tags.Enemy;
        animator.SetInteger(spellTypeHash, 0);
    }
    public void CastBoulder()
    {
        GameObject tempBoulder = Instantiate(Spells[2], transform.position + transform.forward * 2, transform.rotation) as GameObject;
        Boulder _boulder = tempBoulder.GetComponent<Boulder>();
        _boulder.Duration = boulderDuration;
        animator.SetInteger(spellTypeHash, 0);
    }

    IEnumerator ProcessTeleportCooldown()
    {
        yield return new WaitForSeconds(TeleportCooldown);
        canTeleport = true;
    }

    // Actuators and Mutators
    public bool CanTeleport
    {
        get { return canTeleport; }
        set { canTeleport = value; }
    }
    public float TeleportCooldown
    {
        get { return teleportCooldown; }
        set { teleportCooldown = value; }
    }
}
