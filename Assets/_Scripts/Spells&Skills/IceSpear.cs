using UnityEngine;
using System.Collections;

public class IceSpear : MonoBehaviour
{
    [SerializeField]
    private LayerMask collisionMask;

    private int projectileSpeed = 0;
    private int projectileDuration = 1;
    private GameObject source;
    private int damage = 0;
    private string targetTag;
    private float freezeChance = 0;

    // Use this for initialization
    void Start ()
    {
        Destroy(gameObject, projectileDuration);
	}

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);
        transform.Rotate(0, 0, Time.deltaTime * 720);

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        // If the ray has hit something that is within our collision paramters
        if (Physics.Raycast(ray, out hit, Time.deltaTime * projectileSpeed + .1f, collisionMask))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(targetTag))
        {
            bool isCrit = CriticalChance.CheckCritical(source.GetComponent<BaseCharacter>().CriticalChance);
            CombatModifier.ProcessAttack(source, gameObject, damage, isCrit, other.gameObject);
            if(CriticalChance.CheckCritical(freezeChance))
            {
                Debug.Log("Freeze: " + other.name);
            }
        }
    }

    // Actuators and Mutators
    public int ProjectileSpeed
    {
        get { return projectileSpeed; }
        set { projectileSpeed = value; }
    }
    public int ProjectileDuration
    {
        get { return projectileDuration; }
        set { projectileDuration = value; }
    }
    public GameObject Source
    {
        get { return source; }
        set { source = value; }
    }
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }
    public float FreezeChance
    {
        get { return freezeChance; }
        set { freezeChance = value; }
    }
    public string TargetTag
    {
        get { return targetTag; }
        set { targetTag = value; }
    }
}
