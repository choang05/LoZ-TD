using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour
{
    [SerializeField]
    private LayerMask collisionMask;

    [SerializeField]
    private int projectileSpeed;
    [SerializeField]
    private float projectileDuration;
    [SerializeField]
    private GameObject source;
    [SerializeField]
    private float damage;
    [SerializeField]
    private bool isCrit;
    [SerializeField]
    private string targetTag;
	
    void Start()
    {
        Destroy(gameObject, projectileDuration);
    }

	// Update is called once per frame
	void Update ()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);

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
        if (other.transform.CompareTag(targetTag))
        {
            CombatModifier.ProcessAttack(source, gameObject, damage, isCrit, other.gameObject);
            Destroy(gameObject);
        }
    }

    // Actuators and Mutators
    public int ProjectileSpeed
    {
        get { return projectileSpeed; }
        set { projectileSpeed = value; }
    }
    public float ProjectileDuration
    {
        get { return projectileDuration; }
        set { projectileDuration = value; }
    }
    public GameObject Source
    {
        get { return source; }
        set { source = value; }
    }
    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }
    public bool IsCrit
    {
        get { return isCrit; }
        set { isCrit = value; }
    }
    public string TargetTag
    {
        get { return targetTag; }
        set { targetTag = value; }
    }
}
