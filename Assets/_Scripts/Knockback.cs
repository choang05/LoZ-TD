using UnityEngine;
using System.Collections;

public class Knockback : MonoBehaviour
{
    [SerializeField]
    private float knockbackMultiplier = 0;
    [SerializeField]
    private float knockbackDuration = 1;
    private float knockbackTimer = 0;

    private CharacterController _characterController;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }
	
    void Start()
    {
        knockbackTimer = knockbackDuration;
    }

    public void ProcessKnockback(Vector3 direction)
    {
        knockbackTimer = knockbackDuration;
        StartCoroutine(KnackbackCouroutine(direction));
    }

    IEnumerator KnackbackCouroutine(Vector3 direction)
    {
        while(knockbackTimer > 0)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            _characterController.SimpleMove(direction * knockbackMultiplier * Time.deltaTime);
            yield return null;
        }
    }

    // Actuators and Mutators
    public float KnockbackMultiplier
    {
        get { return knockbackMultiplier; }
        set { knockbackMultiplier = value; }
    }
    public float KnockbackDuration
    {
        get { return knockbackDuration; }
        set { knockbackDuration = value; }
    }
}
