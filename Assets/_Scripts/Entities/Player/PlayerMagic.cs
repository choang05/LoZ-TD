using UnityEngine;
using System.Collections;
using PathologicalGames;

public class PlayerMagic : MonoBehaviour
{
    [SerializeField]
    private bool isCasting = false;
    private string currentSpellKey;

    //  ------------ Spell Attributes ------------
    //  Teleport
    public int teleportMana;
    public bool canTeleport;
    public float teleportCooldown;
    public float teleportDistance;
    //  Fireball
    public int fireballMana;
    public int fireballSpeed;
    //  Flamethrower
    public int flamethrowerMana;
    public float flamethrowerInterval;
    [Range(0, 180)] public int flamethrowerAngle;
    public float flamethrowerDistance;
    [Range(0, 1)] public float flamethrowerDamageMultiplier;
    //  Boulder
    public int boulderMana;
    public float boulderDuration;
    //  Ice Spear
    public bool canIceSpear;
    public int iceSpearMana;
    public int iceSpearSpeed;
    public float iceSpearDamageMultiplier;
    [Range(0, 1)] public float iceSpearFreezeChance;
    //  --------------------------------------------------------------------
    //  Spell Prefabs
    public GameObject[] Spells;
    public GameObject[] SpellOrder;
    private GameObject currentSpell;

    //  Spell keybinding
    private const string A1Button = "A1";
    private const string X2Button = "X2";
    private const string Y3Button = "Y3";
    private const string B4Button = "B4";

    // Scripts
    private PlayerMotor _playerMotor;
    private PlayerMana _playerMana;
    private BaseCharacter _baseCharacter;

    //  Animations
    private Animator animator;
    static int spellTypeHash = Animator.StringToHash("SpellType");

    //  Audio
    private AudioSource audioSource;
    public AudioClip[] SoundArray;

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
        if (canTeleport && Input.GetButtonDown(A1Button) && _playerMana.CurrentMana >= teleportMana && !isCasting)
        {
            canTeleport = false;

            //  Audio
            audioSource.pitch = Random.Range(.75f, 1.25f);
            audioSource.PlayOneShot(SoundArray[0], 3);

            //  Spawn teleport effect (before)
            Transform tempTeleport = PoolManager.Pools["Spells"].Spawn(Spells[0]);
            tempTeleport.transform.position = transform.position;
            tempTeleport.rotation = transform.rotation;

            _playerMotor.CharacterController.SimpleMove(transform.TransformDirection(Vector3.forward) * teleportDistance);

            //  Spawn teleport effect (after)
            Transform tempTeleport2 = PoolManager.Pools["Spells"].Spawn(Spells[0]);
            tempTeleport2.transform.position = transform.position;
            tempTeleport2.rotation = transform.rotation;
            //  Cooldown
            StartCoroutine(ProcessTeleportCooldown());
            //  Mana
            _playerMana.AdjustMana(-teleportMana);
        }

        //  Input handler
        //  if the X button is being held down, start held state
        if (Input.GetButtonDown(X2Button) && !isCasting)
        {
            isCasting = true;
            currentSpellKey = X2Button;
            Cast(0);
        }
        //  if the Y button is being held down, start held state
        else if (Input.GetButtonDown(Y3Button) && !isCasting)
        {
            isCasting = true;
            currentSpellKey = Y3Button;
            Cast(1);
        }
        //  if the B button is being held down, start held state
        else if (Input.GetButtonDown(B4Button) && !isCasting)
        {
            isCasting = true;
            currentSpellKey = B4Button;
            Cast(2);
        }
    }

    //  Check which spell is associated with which input key
    void Cast(int spellKey)
    {
        if (SpellOrder[spellKey] == Spells[1] && _playerMana.CurrentMana >= fireballMana)    //  Fireball
        {
            animator.SetInteger(spellTypeHash, 1);
        }
        else if (SpellOrder[spellKey] == Spells[2]) //  FlameThrower
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
            isCasting = false;

    }
    //  --------------  CASTING --------------
    //  -------------- Fire Ball --------------
    public void CastFireball()
    {
        GameObject tempFireball = Instantiate(Spells[1], transform.position + transform.forward + transform.up, transform.rotation) as GameObject;
        Fireball _fireball = tempFireball.GetComponent<Fireball>();
        _fireball.ProjectileSpeed = fireballSpeed;
        _fireball.ProjectileDuration = 5;
        _fireball.Source = gameObject;
        _fireball.Damage = _baseCharacter.Attack;
        _fireball.IsCrit = CriticalChance.CheckCritical(_baseCharacter.CriticalChance);
        _fireball.TargetTag = Tags.Enemy;
        animator.SetInteger(spellTypeHash, 0);
        //  Mana
        _playerMana.AdjustMana(-fireballMana);
        isCasting = false;
        //  Audio
        audioSource.pitch = Random.Range(.5f, 1.5f);
        audioSource.PlayOneShot(SoundArray[1], 2);
    }
    //  -------------- Flamethrower --------------
    IEnumerator CastFlamethrower()
    {
        isCasting = true;
        //_playerMotor.CurrentMoveSpeed *= _playerMotor.CurrentMoveSpeed * 0.5f;
        _playerMotor.CanRotate = false;

        //  Audio
        audioSource.loop = true;
        audioSource.pitch = Random.Range(.5f, 1.5f);
        audioSource.clip = SoundArray[2];
        audioSource.Play();

        GameObject tempFlamethrower = Instantiate(Spells[2], transform.position + transform.up + transform.forward, transform.rotation) as GameObject;
        tempFlamethrower.transform.SetParent(transform);
        Flamethrower _flameThrower = tempFlamethrower.GetComponent<Flamethrower>();
        _flameThrower.Source = gameObject;
        _flameThrower.AttackAngle = flamethrowerAngle;
        _flameThrower.Distance = flamethrowerDistance;

        //  Start cast
        while (Input.GetButton(currentSpellKey) && _playerMana.CurrentMana >= flamethrowerMana)
        {
            _flameThrower.Damage = (int)(_baseCharacter.Attack * flamethrowerDamageMultiplier);
            _flameThrower.ApplyDamage();
            //  Mana
            _playerMana.AdjustMana(-flamethrowerMana);
            yield return new WaitForSeconds(flamethrowerInterval);
        }
        //  End of cast
        isCasting = false;
        //_Movement.CurrentSpeed = _Movement.MaxSpeed;
        _playerMotor.CanRotate = true;
        //  Audio
        audioSource.Stop();
        audioSource.clip = null;
        audioSource.loop = false;

        tempFlamethrower.GetComponentInChildren<ParticleSystem>().Stop();
        yield return new WaitForSeconds(0.5f);
        Destroy(tempFlamethrower);
    }
    //  -------------- Boulder --------------
    public void CastBoulder()
    {
        GameObject tempBoulder = Instantiate(Spells[3], transform.position + transform.forward * 2, transform.rotation) as GameObject;
        Boulder _boulder = tempBoulder.GetComponent<Boulder>();
        _boulder.Duration = boulderDuration;
        animator.SetInteger(spellTypeHash, 0);
        //  Mana
        _playerMana.AdjustMana(-boulderMana);
        isCasting = false;
    }
    //  -------------- Ice Spear --------------
    IEnumerator CastIceSpear()
    {
        isCasting = true;
        _playerMotor.CanRotate = false;
        while (Input.GetButton(currentSpellKey))
        {
            yield return null;
        }
        GameObject tempIceSpear = Instantiate(Spells[4], transform.position + transform.forward + transform.up, transform.rotation) as GameObject;
        IceSpear _iceSpear = tempIceSpear.GetComponent<IceSpear>();
        _iceSpear.ProjectileSpeed = iceSpearSpeed;
        _iceSpear.ProjectileDuration = 5;
        _iceSpear.Source = gameObject;
        _iceSpear.Damage = (int)(_baseCharacter.Attack * iceSpearDamageMultiplier);
        _iceSpear.FreezeChance = iceSpearFreezeChance;
        _iceSpear.TargetTag = Tags.Enemy;
        //  Mana
        _playerMana.AdjustMana(-iceSpearMana);
        isCasting = false;
        _playerMotor.CanRotate = true;
        //  Audio
        audioSource.pitch = Random.Range(.5f, 1.5f);
        audioSource.PlayOneShot(SoundArray[3], 2);
    }

    IEnumerator ProcessTeleportCooldown()
    {
        yield return new WaitForSeconds(teleportCooldown);
        canTeleport = true;
    }
}
