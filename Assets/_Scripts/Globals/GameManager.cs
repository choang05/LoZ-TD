using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameObject[] Players;
    public GameObject[] players;

	// Use this for initialization
	void Start ()
    {
        if (Players == null)
            Players = GameObject.FindGameObjectsWithTag(Tags.Player);

        players = Players;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
