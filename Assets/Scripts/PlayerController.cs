using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController characterController;
    private Vector3 moveDirection;
    private float verticalVelocity;

    [SerializeField]
    private Transform playerRoot, lookRoot;
    [SerializeField]
    private float speed = 5f, gravity = 20f, jumpForce = 10f, sensivity = 5f, smoothWeight = 0.4f, rollAngle = 10f, rollSpeed = 3f;
    [SerializeField]
    private int smoothSteps = 10;
    [SerializeField]
    private bool invert, canUnlock = true;
    [SerializeField]
    private Vector2 defaultLooksLimits = new Vector2(-70, 80f);

    private Vector2 lookAngles, currentMouseLook, smoothMove;

    private float currentRollAngle;

    private int lastLookFrame;
    
    


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        LockAndUnlockCursor();
        MovePlayer();   

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            LookAround();
        }
    }

    void MovePlayer()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed * Time.deltaTime;

        ApplyGravity();

        characterController.Move(moveDirection);
    }

    void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity -= gravity * Time.deltaTime;

            Jump();
        } else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        moveDirection.y = verticalVelocity * Time.deltaTime;

    }

    void Jump()
    {
        if (characterController.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            verticalVelocity = jumpForce;
        }

    }

    void LookAround()
    {
        currentMouseLook = new Vector2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

        lookAngles.x += currentMouseLook.x * sensivity * (invert ? 1f : -1f);
        lookAngles.y += currentMouseLook.y * sensivity;

        lookAngles.x = Mathf.Clamp(lookAngles.x, defaultLooksLimits.x, defaultLooksLimits.y);

        currentRollAngle = Mathf.Lerp(currentRollAngle, Input.GetAxisRaw("Mouse X") * rollAngle, Time.deltaTime * rollSpeed);

        lookRoot.localRotation = Quaternion.Euler(lookAngles.x, 0f, currentRollAngle);
        playerRoot.localRotation = Quaternion.Euler(0f, lookAngles.y, 0f);
    }

    void LockAndUnlockCursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

}
