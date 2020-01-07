using UnityEngine;
using System.Collections;
using DebugSystemCollections;
using System.Collections.Generic;
[RequireComponent (typeof(CharacterController))]
[RequireComponent (typeof(AudioSource))]
public class PlayerController : MonoBehaviour {
    #region variables
    // Floats
    [Range(0,20)]
	public float movementSpeed;
	[Range(0,10)]
	public float mouseSensitivity;
    [Range(0, 10)]
    public float ADSSensitivity;
    public float currentSensitivity;
	[Range(0,30f)]
	public float jumpSpeed;

    [Range(0, 20)]
    public float walkSpeed;
    [Range(0,20)]
	public float sprintSpeed;
    [Range(0, 20)]
    public float gravity;

    
	
	// DO NOT TOUCH
	public float sideSpeed;
	public float forwardSpeed; 
	public float verticalRotation = 0;
	public float upDownRange = 60.0f;
	
	public float verticalVelocity = 0;


    //Weapons

    public GameObject weapon1;
    public GameObject weapon2;
    [SerializeField]
    GameObject lastWeapon;
	// Character Controller
	public CharacterController characterController;


    // Audio
    AudioSource audioSource;
    public List<AudioClip> footsteps = new List<AudioClip>();

	// Transforms.

	// Bools
	public  bool playerIsSprinting = false;

	
	// Vector3's
	public Vector3 speed;

    //FSM

    public PlayerBaseState currentState;
    public readonly Player_IdleState playerIdleState = new Player_IdleState();
    public readonly Player_RunningState playerRunningState = new Player_RunningState();
    public readonly Player_JumpingState playerJumpingState = new Player_JumpingState();
    public readonly Player_SprintState playerSprintState = new Player_SprintState();
    #endregion
    void Awake()
    {
        movementSpeed = walkSpeed;
        TransitionToState(playerIdleState);
        currentSensitivity = mouseSensitivity;
    }

	// Use this for initialization
	void Start () {
		
		characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () 
	{
        PlayerInputs();
        currentState.UpdateState(this);
    }


	private void FixedUpdate() 
	{
        //int clip = Random.Range(0, 1);
        audioSource.clip = footsteps[1];
        Rotation();
        PlayerGravity();
	}

    public void TransitionToState(PlayerBaseState state)
    {
        currentState = state;

        currentState.EnterState(this);
    }


	public void Rotation()
	{
		// Rotation
		
		float rotLeftRight = Input.GetAxis("Mouse X") * currentSensitivity;
		transform.Rotate(0, rotLeftRight, 0);

		
		verticalRotation -= Input.GetAxis("Mouse Y") * currentSensitivity;
		verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
		Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
	}

	public void Movement()
	{
        sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;
        forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
        speed = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);
        speed = transform.rotation * speed;
        characterController.Move(speed * Time.deltaTime);

        //InvokeRepeating("PlayWalkSound", 0f, 0.5f);
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = sprintSpeed;
        }
        else
        {
            movementSpeed = walkSpeed;
        }

        
    }

	public void PlayerInputs()
	{
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weapon2.SetActive(false);
            weapon1.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weapon1.SetActive(false);
            weapon2.SetActive(true);
        }

	}
	
    public void PlayerGravity()
    {
        if (!characterController.isGrounded)
        {
            characterController.Move(new Vector3(0, -gravity * Time.deltaTime, 0));
        }
        else if (characterController.isGrounded)
        {
            characterController.Move(new Vector3(0, 0, 0));
        }
    }

    public IEnumerator PlayFootSteps(float speed)
    {
       
       
        yield return new WaitForSeconds(speed);
        StartCoroutine(PlayFootSteps(speed));
        
    }

    void PlayWalkSound() 
    {
        if (Input.GetButton("Vertical") || Input.GetButton("Horizontal") || !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}
