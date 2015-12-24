using UnityEngine;
using System.Collections;

public class AIStatePattern : MonoBehaviour
{
    //  State Types
    [SerializeField] private StateTypes StateType;
    public enum StateTypes
    {
        Wander,
        Patrol,
        Idle
    }

    // Attributes
    public TargetsInSight _lookArea;
    public TargetsInSight _attackArea;
    private bool canFlee;                                    //  Does the entity flee?
    public bool canChase;                                   //  Does the entity Chase?
    public bool canAttack;                                  //  Does the entity Attack?
    [HideInInspector] public float thinkInterval = .15f;    //  How often unit will 'think' and do commands. 
    [Range(0, 360)] public int sightRange = 170;            //  Widest angle unit is able to see targets up to 360 degrees
    public int wanderRadius = 10;                           //  Max distance unit will wander from its origin
    [Range(0, 1)] public float fleeHealthThreshold;         //  The minimum hp percentage for the entity to consider fleeing
    [HideInInspector] public Vector3 wanderOrigin;          //  The original position the unit will return wander from within its radius.
    public float idleMaxTime = 3;                           //  Max time that unit will idle during IdleState
    public float chaseMaxTime = 5;                          //  Max time that unit will chase the target before returning its base state.
    //[HideInInspector]
    public Transform currentTarget;
    public Transform[] wayPoints;                           //  Patrol type: used for patrols between waypoints 
    [HideInInspector] public Renderer meshRendererFlag;     //  Gameobject used for visual debugging states

    [HideInInspector] public IEnemyState currentState;
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public WanderState wanderState;
    [HideInInspector] public IdleState idleState;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public FleeState fleeState;

    // Scripts
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public EnemyAttack _enemyAttack;
    private Health _health;

    // Animation
    private Animator animator;
    static int forwardHash = Animator.StringToHash("Forward");
    static int hitTriggerHash = Animator.StringToHash("HitTrigger");

    //  Audio
    private AudioSource audioSource;
    public AudioClip[] SoundArray;

    private void Awake()
    {
        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
        wanderState = new WanderState(this);
        idleState = new IdleState(this);
        attackState = new AttackState(this);
        fleeState = new FleeState(this);

        navMeshAgent = GetComponent<NavMeshAgent>();
        _enemyAttack = GetComponent<EnemyAttack>();
        _health = GetComponent<Health>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();

        meshRendererFlag = transform.FindChild("StatePatternFlag").GetComponent<Renderer>();
    }

    // Use this for initialization
    void Start()
    {
        if (fleeHealthThreshold > 0)
            canFlee = true;

        wanderOrigin = transform.position;

        //Determine initial state
        ToBaseState();

        StartCoroutine(Thinking());
    }

    IEnumerator Thinking()
    {
        while(!_health.IsDead)
        {
            yield return new WaitForSeconds(thinkInterval);

            //  Health check every frame 
            if(canFlee && currentState != fleeState && _health.HPPercentage <= fleeHealthThreshold && currentTarget)
            {
                currentState = fleeState;
                currentState.StartState();
            }

            currentState.UpdateState();
            
            if (currentState == idleState || currentState == attackState)
                animator.SetFloat(forwardHash, 0);
            else
                animator.SetFloat(forwardHash, 1f);
        }
        navMeshAgent.Stop();
    }

    public void ProcessHit(GameObject Source, int damage)
    {
        animator.SetTrigger(hitTriggerHash);
        //  Audio
        audioSource.pitch = Random.Range(.5f, 1.5f);
        audioSource.PlayOneShot(SoundArray[0], 1.5f);
        audioSource.PlayOneShot(SoundArray[1], 2);
    }

    public void ToBaseState()
    {
        if (StateType == StateTypes.Idle)
            currentState = idleState;
        else if (StateType == StateTypes.Wander)
            currentState = wanderState;
        else if (StateType == StateTypes.Patrol)
            currentState = patrolState;

        currentState.StartState();
    }


    // Actuators and Mutators
    public bool CanFlee
    {
        get { return canFlee; }
        set { canFlee = value; }
    }
}