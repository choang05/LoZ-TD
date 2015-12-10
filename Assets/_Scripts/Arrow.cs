using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour 
{
	public float speed;
	public LayerMask collisionMask;
	public int maxBounces;
	public GameObject shooter;

	//private CombatModifier _combatModifer;

	float damage;
	int bounceCounter = -1;
	bool hitTarget = false;
	
	// Use this for initialization
	void Awake () 
	{
		//_combatModifer = GameObject.Find("CombatModifers").GetComponent<CombatModifier>();
	}

	void Start()
	{
		StartCoroutine("destory", 2f);
	}
	
	// Update is called once per frame
    /*
	void Update () 
	{
		if(!hitTarget)
		{
			transform.Translate(Vector3.forward * Time.deltaTime * speed);
			
			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, Time.deltaTime * speed + .1f, collisionMask) && bounceCounter < maxBounces) 
			{
				Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);
				float rot = 90 - Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg;
				transform.eulerAngles = new Vector3(0, rot, 0);
				bounceCounter += 1;
			}
			else if(Physics.Raycast(ray, out hit, Time.deltaTime * speed + .1f) && bounceCounter < maxBounces)
			{
				if(hit.collider.CompareTag(Tags.enemy) && hit.collider == hit.transform.GetComponent<BoxCollider>())
				{
					transform.SetParent(hit.transform);
					hitTarget = true;
					_combatModifer.modifyDamage(damage, hit.collider.gameObject, shooter);
					GetComponent<ParticleSystem>().enableEmission = false;
					StartCoroutine("destory", 10f);
				}
			}
			else if(bounceCounter >= maxBounces)
				Destroy(gameObject);
			else
				StartCoroutine("destory", 2f);
		}
	}
    */
	public void setShooter(GameObject Shooter, int MaxBounces, float Damage)
	{
		shooter = Shooter;
		maxBounces = MaxBounces;
		damage = Damage;
	}

	IEnumerator destory(float delay) 
	{
		yield return new WaitForSeconds(delay);
		Destroy(gameObject);
	}
}
