using UnityEngine;
using System.Collections;

public class BillboardEffect : MonoBehaviour 
{

	public Camera target;

	void Start()
	{
		if (!target)
			target = Camera.main;
	}

	// Update is called once per frame
	void Update () 
	{	
		transform.LookAt(transform.position + target.transform.rotation * Vector3.forward, target.transform.rotation * Vector3.up);
	}
}
