using UnityEngine;
using System.Collections;

// This script requires a character controller to be attached
[RequireComponent (typeof (CharacterController))]
public class PlayerMotor : MonoBehaviour
{
    // Attributes
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canRotate = true;
	[SerializeField] private float moveSpeed = 0; // Character movement speed.
    [SerializeField] private float maxMoveSpeed = 0;
	private Vector3 moveDirection = Vector3.zero; // The move direction of the player.

    private Camera myCamera;
	private CharacterController _characterController;

    // Animation
    private Animator animator;
    static int forwardHash = Animator.StringToHash("Forward");
    static int moveSpeedHash = Animator.StringToHash("MoveSpeed");

    void Awake()
    {
		if(myCamera == null)
		{
		    myCamera = Camera.main; // Get main camera as the camera will not always be a child GameObject.
		}
        // Get the player character controller.
        _characterController = transform.GetComponent<CharacterController>();
        // Get the player animator in child.			
        animator = transform.GetComponentInChildren<Animator>();
    }

    void Start()
    {
        ResetMotor();
    }

	public void FixedUpdate()
	{			
		float xMovement = Input.GetAxis("Horizontal");// The horizontal movement.
		float zMovement = Input.GetAxis("Vertical");// The vertical movement.

        // Are we able to move and grounded, yes then move.
        if (canMove)
        {
            moveDirection = new Vector3(xMovement, 0, zMovement);
        }
        else
        {
            xMovement = 0;
            zMovement = 0;
        }

        if (CanRotate && (xMovement != 0 || zMovement != 0))
        {
            transform.forward = moveDirection;
        }

        // Keep the max magnitude of the movement to 1, this keeps additive diagonal speed the same as vertical/horizontal speed.
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1);

        if(canMove)
        {
            _characterController.SimpleMove(moveDirection * moveSpeed * Time.deltaTime);	
        }

        // Animation
        if (animator != null)
        {
            if (xMovement < 0)
                xMovement = -xMovement;
            if (zMovement < 0)
                zMovement = -zMovement;

            animator.SetFloat(forwardHash,  Mathf.Max(xMovement, zMovement));   //  Optimized, as Mathf.Abs(xMovement) = (xMovement > 0 ? xMovement : -xMovement)
            animator.SetFloat(moveSpeedHash, moveSpeed / maxMoveSpeed);
        }
    }

    public void ResetMotor()
    {
        canMove = true;
        canRotate = true;
        moveSpeed = maxMoveSpeed;
    }

    // Actuators and Mutators
    public CharacterController CharacterController
    {
        get { return _characterController; }
        //set { _characterController = value; }
    }
    public float CurrentMoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
    public float MaxMoveSpeed
    {
        get { return maxMoveSpeed; }
        set { maxMoveSpeed = value; }
    }
    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }
    public bool CanRotate
    {
        get { return canRotate; }
        set { canRotate = value; }
    }
}

