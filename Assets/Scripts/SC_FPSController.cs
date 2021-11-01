using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class SC_FPSController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    private PlayerInput playerInput;
    public FixedTouchField touchField;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;
    private bool isJumping;
    public Weapon weapon;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        // // Lock cursor
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        float vertical = input.y;
        float horizontal = input.x;
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * vertical : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * horizontal : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (isJumping && canMove && characterController.isGrounded)
        {
            isJumping = false;
            moveDirection.y = jumpSpeed;
        }
        else
        {
            isJumping = false;
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -touchField.TouchDist.y * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, touchField.TouchDist.x * lookSpeed, 0);
        }
    }

    public void Shoot()
    {
        weapon.gameObject.GetComponent<Animator>().SetBool("Shoot", true);
        Debug.Log("Shooting!");
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
        {
            if (hit.transform.tag == "Zombie")
            {
                hit.transform.gameObject.GetComponent<Zombie>().zombieHealth -= weapon.damage;
                moveDirection = hit.transform.gameObject.GetComponent<Rigidbody>().transform.position - transform.position;
                moveDirection = new Vector3(moveDirection.x, moveDirection.y + 0.75f, moveDirection.z);
                Debug.Log(moveDirection);
                hit.transform.gameObject.GetComponent<Rigidbody>().AddForce(moveDirection.normalized * 50f);
                // hit.transform.gameObject.GetComponent<Zombie>().animator.SetBool("Knockback", true);
                // // if (hit.transform.gameObject.GetComponent<Zombie>().animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                // // { 
                // //     hit.transform.gameObject.GetComponent<Zombie>().animator.SetBool("Knockback", false);
                // // }
                // // else
                // // {
                // //     Debug.Log("playing");
                // // }
                Debug.Log(hit.transform.name + " : " + hit.transform.gameObject.GetComponent<Zombie>().zombieHealth);
            }
        }
    }

    public void Jump()
    {
        isJumping = true;
        Debug.Log("Jumping!");
    }
}