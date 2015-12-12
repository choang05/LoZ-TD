using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour 
{

    // Attributes
	[SerializeField] private float speed;
    [SerializeField] private int lifeDuration;
    [SerializeField] private LayerMask collisionMask;

	private int maxBounces;
    private string targetableTag;
	private GameObject source;
	private float damage;
    private bool isCrit;
	private int bounceCounter = 0;
	private bool didHitTarget = false;
	
	void Start()
	{
		StartCoroutine(DestroyDelay(lifeDuration));
	}
	
	// Update is called once per frame
    
	void Update () 
	{
		if(!didHitTarget)
		{
			transform.Translate(Vector3.forward * Time.deltaTime * speed);
			
			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;
            // If the ray has hit something that is within our collision paramters
			if (Physics.Raycast(ray, out hit, Time.deltaTime * speed + .1f, collisionMask)) 
			{
				Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);
				float rot = 90 - Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg;
				transform.eulerAngles = new Vector3(0, rot, 0);
				bounceCounter += 1;
			}

            if(bounceCounter > maxBounces)
            {
                Destroy(gameObject);
            }
		}
	}
    
	public void SetSource(GameObject Source, int MaxBounces, float Speed, bool IsCrit, string targetTag)
	{
        source = Source;
		maxBounces = MaxBounces;
        speed = Speed;
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
            CombatModifier.ProcessAttack(source, damage, isCrit, other.gameObject);

            //GetComponent<ParticleSystem>().enableEmission = false;
            StartCoroutine(DestroyDelay(lifeDuration));
        }
    }

	IEnumerator DestroyDelay(float delay) 
	{
		yield return new WaitForSeconds(delay);
		Destroy(gameObject);
	}

}
