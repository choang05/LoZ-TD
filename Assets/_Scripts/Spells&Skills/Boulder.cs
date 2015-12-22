using UnityEngine;
using System.Collections;

public class Boulder : MonoBehaviour
{
    //  Attributes
    [SerializeField]
    private float duration = 0;

    //  Animation
    private Animation anim;
    private string depspawnClip = "Boulder_Despawn";

    //  Audio
    private AudioSource audioSource;
    public AudioClip spawnSound;
    public AudioClip despawnSound;

    void Awake()
    {
        anim = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(DespawnDelay());
    }

    IEnumerator DespawnDelay()
    {

        //  Audio
        audioSource.pitch = Random.Range(.5f, 1.5f);
        audioSource.PlayOneShot(spawnSound, 3);

        yield return new WaitForSeconds(duration);

        anim.Play(depspawnClip);

        Destroy(gameObject, 1);
        //audioSource.pitch = Random.Range(.5f, 1.5f);
        //audioSource.PlayOneShot(despawnSound, 2);

    }

    // Actuators and Mutators
    public float Duration
    {
        get { return duration; }
        set { duration = value; }
    }
}
