using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flamethrower : MonoBehaviour
{
    // Attributes 
    [Range(0, 180)]
    private int attackAngle = 110;
    private int angleFromTarget;
    private float sphereRadius = 1;

    private GameObject source;
    private int damage;
    
    //  Scripts
    private SphereCollider sphereCollider;

    [SerializeField]
    List<GameObject> targetsInRange = new List<GameObject>();


    void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    void Start()
    {
        sphereCollider.radius = sphereRadius;
    }

    void OnTriggerExit(Collider other)
    {
        targetsInRange.Remove(other.gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        angleFromTarget = (int)Vector3.Angle(other.transform.position - transform.parent.position, transform.parent.forward);

        //  If the target is within view range
        if (angleFromTarget <= attackAngle * 0.5f)
        {
            //Debug.Log("within view angle");
            //  If target isn't in the view list already & isn't dead
            if (other.CompareTag(Tags.Enemy))
            {
                Health _health = other.GetComponent<Health>();
                if (!targetsInRange.Contains(other.gameObject) && !_health.IsDead)
                {
                    targetsInRange.Add(other.gameObject);
                }
                else if (_health.IsDead)
                {
                    targetsInRange.Remove(other.gameObject);
                }
            }
        }
        else if (angleFromTarget > attackAngle * 0.5f && targetsInRange.Contains(other.gameObject))
            targetsInRange.Remove(other.gameObject);
    }

    public void ApplyDamage()
    {
        for (int i = 0; i < targetsInRange.Count; i++)
        {
            CombatModifier.ProcessAttack(source, null, damage, false, targetsInRange[i]);
        }
    }

    // Actuators and Mutators
    public int AttackAngle
    {
        get { return attackAngle; }
        set { attackAngle = value; }
    }
    public float Distance
    {
        get { return sphereRadius; }
        set { sphereRadius = value; }
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
}
