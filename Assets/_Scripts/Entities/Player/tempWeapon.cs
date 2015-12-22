using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//  This class is intended for quick prototyping of switching weapons/classes until proper system is setup
public class tempWeapon : MonoBehaviour
{
    public WeaponType weaponType;
    public enum WeaponType
    {
        Melee,
        Range,
        Magic
    }
    //  Equipment
    public GameObject meleeSword;
    public GameObject meleeShield;
    public GameObject rangeBow;
    public GameObject magicStaff;

    //  Scripts
    private BaseCharacter _baseCharacter;

    private PlayerMelee _playerMelee;
    private PlayerRange _playerRange;
    private PlayerMagic _playerMagic;

    private PlayerStamina _playerStamina;
    private PlayerMana _playerMana;

    public Transform leftObjectNode;
    public Transform rightObjectNode;

    // Animation
    private Animator animator;
    static int isMeleeHash = Animator.StringToHash("isMelee");
    static int isRangeHash = Animator.StringToHash("isRange");
    static int isMagicHash = Animator.StringToHash("isMagic");

    //  UI
    private GameObject ShieldBar;
    private GameObject StaminaBar;
    private GameObject ManaBar;

    void Awake()
    {
        _baseCharacter = GetComponent<BaseCharacter>();
        _playerMelee = GetComponent<PlayerMelee>();
        _playerRange = GetComponent<PlayerRange>();
        _playerMagic = GetComponent<PlayerMagic>();

        _playerStamina = GetComponent<PlayerStamina>();
        _playerMana = GetComponent<PlayerMana>();

        //leftObjectNode = GameObject.Find("L_ObjectNode").transform;
        //rightObjectNode = GameObject.Find("R_ObjectNode").transform;

        ShieldBar = GameObject.Find("ShieldBar");
        StaminaBar = GameObject.Find("StaminaBar");
        ManaBar = GameObject.Find("ManaBar");

        animator = transform.GetComponentInChildren<Animator>();
    }

    // Use this for initialization
    void Start ()
    {
        UpdateWeapon();
    }

    public void ChangeToMelee()
    {
        weaponType = WeaponType.Melee;
        UpdateWeapon();
    }
    public void ChangeToRange()
    {
        weaponType = WeaponType.Range;
        UpdateWeapon();
    }
    public void ChangeToMagic()
    {
        weaponType = WeaponType.Magic;
        UpdateWeapon();
    }

    public void UpdateWeapon()
    {
        foreach (Transform child in leftObjectNode)
            Destroy(child.gameObject);
        foreach (Transform child in rightObjectNode)
            Destroy(child.gameObject);

        if (weaponType == WeaponType.Melee)
        {
            _baseCharacter.Attack = 20;
            _baseCharacter.AttackDeviation = 15;

            _playerRange.enabled = false;
            _playerMagic.enabled = false;
            _playerMelee.enabled = true;

            _playerStamina.enabled = false;
            _playerMana.enabled = false;

            GameObject tempSword = Instantiate(meleeSword, rightObjectNode.position, Quaternion.identity) as GameObject;
            tempSword.transform.SetParent(rightObjectNode.transform);
            tempSword.transform.localRotation = Quaternion.identity;

            GameObject tempShield = Instantiate(meleeShield, leftObjectNode.position, Quaternion.identity) as GameObject;
            tempShield.transform.SetParent(leftObjectNode.transform);
            tempShield.transform.localRotation = Quaternion.identity;

            //  Animation
            animator.SetBool(isRangeHash, false);
            animator.SetBool(isMagicHash, false);
            animator.SetBool(isMeleeHash, true);
            //  UI
            ShieldBar.SetActive(true);
            StaminaBar.SetActive(false);
            ManaBar.SetActive(false);
        }
        else if(weaponType == WeaponType.Range)
        {
            _baseCharacter.Attack = 15;
            _baseCharacter.AttackDeviation = 3;

            _playerMagic.enabled = false;
            _playerMelee.enabled = false;
            _playerRange.enabled = true;

            _playerStamina.enabled = true;
            _playerMana.enabled = false;

            GameObject tempBow = Instantiate(rangeBow, leftObjectNode.position, Quaternion.identity) as GameObject;
            tempBow.transform.SetParent(leftObjectNode.transform);
            tempBow.transform.localRotation = Quaternion.identity;

            //  Animation
            animator.SetBool(isMagicHash, false);
            animator.SetBool(isMeleeHash, false);
            animator.SetBool(isRangeHash, true);
            //  UI
            ShieldBar.SetActive(false);
            StaminaBar.SetActive(true);
            ManaBar.SetActive(false);
        }
        else if (weaponType == WeaponType.Magic)
        {
            _baseCharacter.Attack = 30;
            _baseCharacter.AttackDeviation = 0;

            _playerMelee.enabled = false;
            _playerRange.enabled = false;
            _playerMagic.enabled = true;

            _playerStamina.enabled = false;
            _playerMana.enabled = true;

            GameObject tempStaff = Instantiate(magicStaff, rightObjectNode.position, Quaternion.identity) as GameObject;
            tempStaff.transform.SetParent(rightObjectNode.transform);
            tempStaff.transform.localRotation = Quaternion.identity;

            //  Animation
            animator.SetBool(isMeleeHash, false);
            animator.SetBool(isRangeHash, false);
            animator.SetBool(isMagicHash, true);
            //  UI
            ShieldBar.SetActive(false);
            StaminaBar.SetActive(false);
            ManaBar.SetActive(true);
        }
    }
}
