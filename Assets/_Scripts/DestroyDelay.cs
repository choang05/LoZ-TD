using UnityEngine;
using System.Collections;

public class DestroyDelay : MonoBehaviour
{
    public float DelaySeconds = 0;

	// Use this for initialization
	void Start ()
    {
        Destroy(gameObject, DelaySeconds);
	}
}
