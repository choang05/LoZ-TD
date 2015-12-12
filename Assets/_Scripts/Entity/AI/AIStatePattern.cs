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
    public bool canChase;
    public bool canAttack;
    [HideInInspector] public float thinkInterval = .15f;    // How often unit will 'think' and do commands. 
    [HideInInspector] public float thinkTimer = 0;
    [Range(0, 360)] public int sightRange = 170;            // Widest angle unit is able to see targets up to 360 degrees
    public int wanderRadius = 10;                           // Max distance unit will wander from its origin
    [HideInInspector] public Vector3 wanderOrigin;          // The original position the unit will return wander from within its radius.
    public float idleMaxTime = 3;                           // Max time that unit will idle during IdleState
    public float chaseMaxTime = 5;                          // Max time that unit will chase the target before returning its base state.
    public Transform[] wayPoints;                           // Used for patrolling types
    public Vector3 offset = new Vector3(0, .5f, 0);
    public MeshRenderer meshRendererFlag;                   // Gameobject used for debugging states

    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public IEnemyState currentState;
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public WanderState wanderState;
    [HideInInspector] public IdleState idleState;
    [HideInInspector] public AttackState attackState;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public EnemyAttack _enemyAttack;

    // Animation
    private Animator animator;
    static int forwardHash = Animator.StringToHash("Forward");

    private void Awake()
    {
        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
        wanderState = new WanderState(this);
        idleState = new IdleState(this);
        attackState = new AttackState(this);

        navMeshAgent = GetComponent<NavMeshAgent>();
        _enemyAttack = GetComponent<EnemyAttack>();
        animator = GetComponentInChildren<Animator>();
    }

    // Use this for initialization
    void Start()
    {
        wanderOrigin = transform.position;

        //Determine initial state
        ToBaseState();
    }

    // Update is called once per frame
    void Update()
    {
        thinkTimer += Time.deltaTime;
        if(thinkTimer >= thinkInterval)
        {
            currentState.UpdateState();
            thinkTimer = 0;
        }
        if(currentState == idleState || currentState == attackState)
            animator.SetFloat(forwardHash, 0);
        else if(currentState == chaseState || currentState == patrolState || currentState == wanderState)
            animator.SetFloat(forwardHash, .75f);
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
}