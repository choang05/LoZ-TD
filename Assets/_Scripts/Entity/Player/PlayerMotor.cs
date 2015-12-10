using UnityEngine;
using System.Collections;

// This script requires a character controller to be attached
[RequireComponent (typeof (CharacterController))]
public class PlayerMotor : MonoBehaviour
{
    // Attributes
    [SerializeField] private bool canMove = true;
	[SerializeField] private float moveSpeed = 6.0f; // Character movement speed.
    [SerializeField] private bool canRotate = true;
    [SerializeField] private int rotationSpeed = 8; // How quick the character rotate to target location.
    private float gravity = 20.0f; // Gravity for the character.
	private Vector3 moveDirection = Vector3.zero; // The move direction of the player.

    private Camera myCamera;
	private CharacterController controller;

    // Animation
	private Animator animator;
    static int forwardHash = Animator.StringToHash("Forward");

    void Awake()
    {
		myCamera = Camera.main; // Get main camera as the camera will not always be a child GameObject.
		if(myCamera == null)
		{
			Debug.LogError("No main camera, please add camera or set camera to MainCamera in the tag option.");
		}
		// Get the player character controller.
		controller = transform.GetComponent<CharacterController>();			
		// Get the player animator in child.			
        animator = transform.GetComponentInChildren<Animator>();
    }
	
	public void Update()
	{			
		// Reset player rotation to look in the same direction as the camera.
		/*Quaternion tempRotation = myCamera.transform.rotation;
		tempRotation.x = 0;
		tempRotation.z = 0;
        transform.rotation = tempRotation;
        */
		
		float xMovement = Input.GetAxis("Horizontal");// The horizontal movement.
		float zMovement = Input.GetAxis("Vertical");// The vertical movement.

        // Are we able to move and grounded, yes then move.
        if (canMove && IsGrounded())
        {
            moveDirection = new Vector3(xMovement, 0, zMovement).normalized;
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

        if (animator != null)
            animator.SetFloat(forwardHash, Mathf.Abs(xMovement) + Mathf.Abs(zMovement));

        if(canMove)
        {
            // Apply gravity.
            moveDirection.y -= gravity * Time.deltaTime;
		    controller.Move(moveDirection * MoveSpeed * Time.deltaTime);	
        }
        				
	}
	
	// Check if the player is grounded.
	bool IsGrounded () {
		return controller.isGrounded;
	}

    // Actuators and Mutators
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
    public int RotationSpeed
    {
        get { return rotationSpeed; }
        set { rotationSpeed = value; }
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

