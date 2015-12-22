using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour
{
    [SerializeField]
    private int travelSpeed;
    [SerializeField]
    private float duration;
    [SerializeField]
    private GameObject source;
    [SerializeField]
    private float damage;
    [SerializeField]
    private bool isCrit;
    [SerializeField]
    private string targetTag;
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * travelSpeed);
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
    public int TravelSpeed
    {
        get { return travelSpeed; }
        set { travelSpeed = value; }
    }
    public float Duration
    {
        get { return duration; }
        set { duration = value; }
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
