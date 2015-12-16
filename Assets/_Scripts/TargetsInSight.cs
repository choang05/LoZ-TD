using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetsInSight : MonoBehaviour
{
    // Attributes 
    public float fieldOfViewAngle = 110f;
    private float angleFromTarget;
    public bool isEnemy = false;
    public bool isPlayer = false;

    [SerializeField]
    List<GameObject> targetsInRange = new List<GameObject>();
    [SerializeField]
    List<GameObject> targetsInView = new List<GameObject>();

    //private CapsuleCollider capsuleCollider;

    void Awake()
    {
        //capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Start()
    {
        if (transform.parent.CompareTag(Tags.Player))
            isPlayer = true;
        else if (transform.parent.CompareTag(Tags.Enemy))
            isEnemy = true;
    }

    public List<GameObject> GetTargetsInView()
    {
        /*
        List<GameObject> tempTargets = new List<GameObject>();

        for (int i = 0; i < targets.Count; i++)
        {
            // Create a vector from the origine to the target and store the angle between it and forward.
            Vector3 direction = targets[i].transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            // If the angle between forward and where the target is, is less than half the angle of view...
            if (angle < fieldOfViewAngle * 0.5f)
                targets.Add(targets[i]);
        }

        return tempTargets;*/
        return targetsInView;
    }

    void OnTriggerEnter(Collider other)
    {
        if(isPlayer)
        {
            if (other.CompareTag(Tags.Enemy))
            {
                targetsInRange.Add(other.gameObject);
            }
        }

        else if (isEnemy)
        {
            if (other.CompareTag(Tags.Player))
            {
                targetsInRange.Add(other.gameObject);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(targetsInRange.Contains(other.gameObject))
            targetsInRange.Remove(other.gameObject);
        if (targetsInView.Contains(other.gameObject))
            targetsInView.Remove(other.gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        angleFromTarget = Vector3.Angle(other.transform.position - transform.parent.position, transform.parent.forward);

        //  If the target is within view range
        if (angleFromTarget <= fieldOfViewAngle * 0.5f)
        {
            //  If target isn't in the view list already & isn't dead
            if(!targetsInView.Contains(other.gameObject) && other.GetComponent<Health>() && !other.GetComponent<Health>().IsDead)
            {
                if (isPlayer)
                {
                    if (other.CompareTag(Tags.Enemy))
                        targetsInView.Add(other.gameObject);
                }
                else if (isEnemy)
                {
                    if (other.CompareTag(Tags.Player))
                        targetsInView.Add(other.gameObject);
                }
            }
            else if(other.GetComponent<Health>() && other.GetComponent<Health>().IsDead)
            {
                targetsInView.Remove(other.gameObject);
            }
        }
        else if(angleFromTarget > fieldOfViewAngle * 0.5f)
            targetsInView.Remove(other.gameObject);
    }
}
