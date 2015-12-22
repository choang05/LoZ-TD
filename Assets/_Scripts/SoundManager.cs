using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static AudioClip[] soundArray;

    [SerializeField]
    private AudioClip[] audioClips; 

	// Use this for initialization
	void Awake ()
    {
        soundArray = audioClips;
	}
}
