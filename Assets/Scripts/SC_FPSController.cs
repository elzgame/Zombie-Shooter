using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

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
    private bool isShooting;
    public Weapon weapon;
    public Knife knife;
    public WeaponManager weaponManager;
    public Sprite weaponSprite;
    public Sprite knifeSprite;
    public GameObject weaponShootButton;
    private bool isStepping;
    private Animator animator;
    public AudioClip soundWalking;
    public AudioClip soundJumping;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        float vertical = input.y;
        float horizontal = input.x;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * vertical : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * horizontal : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        if (moveDirection.x != 0 && moveDirection.z != 0 && isStepping == false)
        {
            isStepping = true;
            StartCoroutine(Walking());
        }
        else if (moveDirection.x == 0 && moveDirection.z == 0)
        {
            StopCoroutine(Walking());
        }

        if (isJumping && canMove && characterController.isGrounded)
        {
            isJumping = false;
            GameManager.audioSource.PlayOneShot(soundJumping);
            moveDirection.y = jumpSpeed;
        }
        else
        {
            isJumping = false;
            moveDirection.y = movementDirectionY;
        }

        if (isShooting)
        {
            Shoot();
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -touchField.TouchDist.y * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, touchField.TouchDist.x * lookSpeed, 0);
        }
    }

    IEnumerator Walking()
    {
        GameManager.audioSource.PlayOneShot(soundWalking);
        yield return new WaitForSeconds(.5f);
        isStepping = false;
        StopCoroutine(Walking());
    }

    public void Shoot()
    {
        if (weapon.isCanShoot && weaponManager.weaponUsed == 0)
        {
            weapon.gameObject.GetComponent<Animator>().SetBool("Shoot", true);
        }
        else if (weaponManager.weaponUsed == 1)
        {
            knife.gameObject.GetComponent<Animator>().SetBool("Stab", true);
        }
    }

    public void SwitchWeapon()
    {
        if (weaponManager.weaponUsed == 0)
        {
            weaponShootButton.GetComponent<Image>().sprite = knifeSprite;
            weaponManager.isSwitching = true;
            weapon.animator.SetInteger("Switch", 1);
            weaponManager.weaponUsed = 1;
        }
        else if (weaponManager.weaponUsed == 1)
        {
            weaponShootButton.GetComponent<Image>().sprite = weaponSprite;
            weaponManager.isSwitching = true;
            knife.animator.SetInteger("Switch", 1);
            weaponManager.weaponUsed = 0;
        }
    }

    public void Jump()
    {
        isJumping = true;
    }

    public void ShootingDown()
    {
        isShooting = true;
    }

    public void ShootingUp()
    {
        isShooting = false;
    }

}