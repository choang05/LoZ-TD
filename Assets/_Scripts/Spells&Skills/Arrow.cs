using UnityEngine;
using System.Collections;
using PathologicalGames;

public class Arrow : MonoBehaviour 
{

    // Attributes
	[SerializeField] private int projectileSpeed = 0;
    [SerializeField] private int lifeDuration = 3;
    [SerializeField] private LayerMask collisionMask;

	private int maxBounces = 0;
	private int bounceCounter = 0;
    private string targetTag;
	private GameObject source;
	private float damage = 0;
    private bool isCrit = false;
	private bool didHitTarget = false;

    //  Particles
    private ParticleSystem _particleSystem;

    void Awake()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    //  PoolManager
    public void OnSpawned()  
    {
        // Start the timer as soon as this instance is spawned.
       // StartCoroutine(DespawnDelay());
    }
    public void OnDespawned()
    {
        // Handle destruction visuals, like explosions and sending damage
        // information to nearby objects
        // ...
        didHitTarget = false;
        bounceCounter = 0;
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
                bounceCounter += 1;
                if (bounceCounter > maxBounces)
                {
                    didHitTarget = true;
                    transform.SetParent(hit.transform);
                    _particleSystem.Stop();
                    StartCoroutine(DespawnDelay());
                }
                else
                {
                    Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);
                    float rot = 90 - Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg;
                    transform.eulerAngles = new Vector3(0, rot, 0);
                }
            }
		}
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag(targetTag) && !didHitTarget)
        {
            didHitTarget = true;

            transform.SetParent(other.transform);

            damage = source.GetComponent<BaseCharacter>().Attack;
            CombatModifier.ProcessAttack(source, gameObject, damage, isCrit, other.gameObject);

            _particleSystem.Stop();
            StartCoroutine(DespawnDelay());
        }
    }

    public void ResetProjectile()
    {
        didHitTarget = false;
        bounceCounter = 0;

        transform.SetParent(null);

        _particleSystem.Play();
        StartCoroutine(DespawnDelay());
    }

    IEnumerator DespawnDelay()
    {
        yield return new WaitForSeconds(lifeDuration);
        PoolManager.Pools["Projectiles"].Despawn(this.transform);
    }

    // Actuators and Mutators
    public GameObject Source
    {
        get { return source; }
        set { source = value; }
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
