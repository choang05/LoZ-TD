using UnityEngine;
using System.Collections;

public class tempWeapon : MonoBehaviour
{
    public WeaponType weaponType;
    public enum WeaponType
    {
        Melee,
        Range,
        Magic
    }

    public GameObject weaponNode;

    private PlayerMeleeAttack playerMeleeAttack;
    private PlayerRangeAttack playerRangeAttack;
    //public PlayerMeleeAttack playerMeleeAttack;

    // Animation
    private Animator animator;
    static int isMeleeHash = Animator.StringToHash("isMelee");
    static int isRangeHash = Animator.StringToHash("isRange");

    void Awake()
    {
        playerMeleeAttack = GetComponent<PlayerMeleeAttack>();
        playerRangeAttack = GetComponent<PlayerRangeAttack>();
        animator = transform.GetComponentInChildren<Animator>();
    }

    // Use this for initialization
    void Start ()
    {
        UpdateAnimation();
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateAnimation();
	}

    public void UpdateAnimation()
    {
        if(weaponType == WeaponType.Melee)
        {
            playerRangeAttack.enabled = false;
            playerMeleeAttack.enabled = true;
            animator.SetBool(isRangeHash, false);
            animator.SetBool(isMeleeHash, true);
        }
        else if(weaponType == WeaponType.Range)
        {
            playerMeleeAttack.enabled = false;
            playerRangeAttack.enabled = true;
            animator.SetBool(isMeleeHash, false);
            animator.SetBool(isRangeHash, true);
        }
        else if (weaponType == WeaponType.Magic)
        {

        }
    }
}
