using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour 
{

    // Attributes
	[SerializeField] private int projectileSpeed;
    [SerializeField] private int lifeDuration = 3;
    [SerializeField] private LayerMask collisionMask;

	private int maxBounces = 0;
	private int bounceCounter = 0;
    private string targetableTag;
	private GameObject source;
	private float damage;
    private bool isCrit = false;
	private bool didHitTarget = false;

    //  Particles
    private ParticleSystem particleSystem;

    void Awake()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }

	void Start()
	{
        Destroy(gameObject, lifeDuration);
	}
	
	// Update is called once per frame
    
	void Update () 
	{
		if(!didHitTarget)
		{
			transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);

            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            // If the ray has hit something that is within our collision paramters
            if (Physics.Raycast(ray, out hit, Time.deltaTime * projectileSpeed + .1f, collisionMask))
            {
                Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);
                float rot = 90 - Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, rot, 0);
                bounceCounter += 1;
            }

            if (bounceCounter > maxBounces)
            {
                Destroy(gameObject);
            }
		}
	}
    
	public void SetSource(GameObject Source, int MaxBounces, int Speed, bool IsCrit, string targetTag)
	{
        source = Source;
		maxBounces = MaxBounces;
        projectileSpeed = Speed;
        isCrit = IsCrit;
        targetableTag = targetTag;
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(targetableTag) && !didHitTarget)
        {
            didHitTarget = true;

            transform.SetParent(other.transform);

            int damage = source.GetComponent<BaseCharacter>().Attack;
            CombatModifier.ProcessAttack(source, gameObject, damage, isCrit, other.gameObject);

            particleSystem.Stop();
            Destroy(gameObject, lifeDuration);
        }
    }

    public void ResetProjectile()
    {
        didHitTarget = false;
        bounceCounter = 0;

        transform.SetParent(null);

        particleSystem.Play();
        Destroy(gameObject, lifeDuration);
    }

    public int ProjectileSpeed
    {
        get { return projectileSpeed; }
        set { projectileSpeed = value; }
    }
    public int MaxBounces
    {
        get { return maxBounces; }
        set { maxBounces = value; }
    }
}
