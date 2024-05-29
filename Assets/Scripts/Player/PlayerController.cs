using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float runSpeed;
    public float jumpPower;
    public bool IsWalking;
    public bool IsRunning; 
    public bool IsJumping;
    public float useStamina;
    public Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minLook;
    public float maxLook;
    private float camCurRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;

    public bool canLook = true;

    private Rigidbody _rigidbody;
    public Animator animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
        IsGrounded();
    }
    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }
    void Move()
    {
        float currentSpeed = IsRunning ? runSpeed : moveSpeed;
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= currentSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
        animator.SetBool("IsWalking", IsWalking && !IsRunning);
        animator.SetBool("IsRunning", IsRunning);
    }


    void CameraLook()
    {
        camCurRot += mouseDelta.y * lookSensitivity;
        camCurRot = Mathf.Clamp(camCurRot, minLook, maxLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurRot, 0, 0);
        //부호를 바꾸는이유는 실제로 마우스를 내렸을때 rotation값이 +가 되어야 되기때문

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
            IsWalking = curMovementInput != Vector2.zero;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
            IsWalking = false;
            IsRunning = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            IsRunning = curMovementInput.y > 0 && CharacterManager.Instance.Player.condition.UpdateRunningState();
            if (IsRunning)
            {
                StartCoroutine("ConsumeStamina");
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            IsRunning = false;
            StopCoroutine("ConsumeStamina");
        }
    }

    IEnumerator ConsumeStamina()
    {
        while (IsRunning)
        {
            CharacterManager.Instance.Player.condition.UseStamina(useStamina);
            yield return new WaitForSeconds(1f);
        }
    }


    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            animator.SetTrigger("IsJumping");
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);    
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                animator.SetBool("IsFreefall", false);
                return true;
            }
        }
        animator.SetBool("IsFreefall", true);
        return false;
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
