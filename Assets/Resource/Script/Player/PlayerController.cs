using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    public float interactionRange;
    public Transform pointTransform;
    [Header("Movement")]
    public float moveSpeed;
    public float runSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;
    private Rigidbody rigidbody;

    [Header("Look")]
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public Transform cameraContainer;

    public Transform forwardPosUp;
    public Transform forwardPosDown;
    public bool isRun = false;
    public Transform oneView;
    public Transform threeView;

    public bool isLook = true;
    public Action inventory;
    public bool isFlying = false;

    [Header("InterObject")]
    public GameObject interObject;
    public TextMeshProUGUI objectNameText;
    public TextMeshProUGUI objectDescText;

    iInteraction nowInteraction;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void LateUpdate()
    {
        if (isLook == true)
        {
            CameraLook();
        }
    }
    float test = 0;
    private void Update()
    {
        IsPlatform();
        HangingOnTheWall();
        Vector3 dir = Camera.main.transform.position - pointTransform.position;
        Ray interactionRay = new Ray(Camera.main.transform.position, -dir.normalized);

        if (Physics.Raycast(interactionRay, out var hitInfo, interactionRange))
        {
            if (hitInfo.transform.TryGetComponent(out iInteraction interaction))
            {
                nowInteraction = interaction;
            }
            else
            {
                nowInteraction = null;
            }
        }
        else
        {
            nowInteraction = null;
        }

        if (nowInteraction != null)
        {
            interObject.SetActive(true);
            objectNameText.text = nowInteraction.GetName();
            objectDescText.text = nowInteraction.GetDesc();
        }
        else
        {
            interObject.SetActive(false);
            objectNameText.text = String.Empty;
            objectDescText.text = String.Empty;
        }
    }
    private void FixedUpdate()
    {
        if (isFlying == false)
            Move();
    }
    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        if (isRun)
            dir *= runSpeed;
        else
            dir *= moveSpeed;

        dir.y = rigidbody.velocity.y;

        rigidbody.velocity = dir;
    }
    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }
    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Performed)
        {
            curMovementInput = callbackContext.ReadValue<Vector2>();
        }
        else if (callbackContext.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext callbackContext)
    {
        mouseDelta = callbackContext.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started && IsGrounded())
        {
            rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f),Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f),Vector3.down),
            new Ray(transform.position + (transform.right* 0.2f) + (transform.up * 0.01f),Vector3.down),
            new Ray(transform.position + (-transform.right* 0.2f) + (transform.up * 0.01f),Vector3.down)
        };
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }

    bool IsPlatform()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f),Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f),Vector3.down),
            new Ray(transform.position + (transform.right* 0.2f) + (transform.up * 0.01f),Vector3.down),
            new Ray(transform.position + (-transform.right* 0.2f) + (transform.up * 0.01f),Vector3.down)
        };
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], out var hitInfo, 0.1f))
            {
                if (hitInfo.transform.TryGetComponent(out Platform platform))
                {
                    transform.SetParent(platform.transform);
                    return true;
                }
            }
        }
        transform.SetParent(null);
        return false;
    }

    public void Run(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            isRun = true;
        }
        else if (callbackContext.phase == InputActionPhase.Canceled)
        {
            isRun = false;
        }
    }

    public void CheckInteractionObject(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            Vector3 dir = Camera.main.transform.position - pointTransform.position;
            Ray interactionRay = new Ray(Camera.main.transform.position, -dir.normalized);

            if (Physics.Raycast(interactionRay, out var hitInfo, interactionRange))
            {
                if (hitInfo.transform.TryGetComponent(out iInteraction interaction))
                {
                    interaction.InteractionAction();
                }
            }
        }
    }
    public void ChangeOfViewpoint(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            Debug.Log("sdafinop");
            cameraContainer.localPosition = cameraContainer.localPosition == threeView.localPosition ? oneView.localPosition : threeView.localPosition;
        }
    }

    public void HangingOnTheWall()
    {
        Vector3 forwardDir = (transform.position - forwardPosDown.position).normalized;
        Ray[] rays = new Ray[2]
        {
            new Ray(forwardPosUp.position,forwardDir),
            new Ray(forwardPosDown.position ,forwardDir)
        };

        Debug.DrawLine(rays[0].origin, rays[0].direction);
    }

    public void OnInventory(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        isLook = !toggle;
    }

    public void OnShootPlatform()
    {
        gameObject.SetActive(false);
    }

    public void OnShootPlayer(float power, Vector3 dir)
    {
        gameObject.SetActive(true);
        rigidbody.AddForce(dir * power, ForceMode.Impulse);
        isFlying = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isFlying = false;
    }

    public void SpeedUp()
    {
        StartCoroutine("SpeedUpBuff");
    }
    IEnumerator SpeedUpBuff()
    {
        moveSpeed += 3;
        runSpeed += 3;
        yield return new WaitForSeconds(10);
        moveSpeed -= 3;
        runSpeed -= 3;
    }
}
