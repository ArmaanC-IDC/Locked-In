using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float verticalMouseSensitivity = 2f;
    public float horizontalMouseSensitivity = 5f;
    public float interactMaxDistance = 50f;

    public InputAction movementControls;
    public InputAction lookControls;
    public InputAction interactControls;

    private Rigidbody rb;
    private float verticalLookRotation = 0;
    private new Transform camera;
    private Vector2 lookInputValue = Vector2.zero;
    private Transform interactIndicator;
    private GameObject currentHoverObject; 

    #region enable/disable InputActions
    private void OnEnable()
    {
        movementControls.Enable();

        lookControls.Enable();
        lookControls.performed += OnLookPerformed;
        lookControls.canceled += OnLookCanceled;

        interactControls.Enable();
        interactControls.performed += OnInteract;
    }

    private void OnDisable()
    {
        movementControls.Disable();

        lookControls.performed -= OnLookPerformed;
        lookControls.canceled -= OnLookCanceled;
        lookControls.Disable();

        interactControls.performed -= OnInteract;
        interactControls.Disable();
    }
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        camera = transform.Find("Camera");
        interactIndicator = transform.Find("InteractIndicator");
    }

    void FixedUpdate()
    {
        HandleCameraLooking();
        
        HandlePlayerMovement();

        //interaction based on events
    }

    #region handle player movement
    private void HandlePlayerMovement()
    {
        Vector2 moveDir = movementControls.ReadValue<Vector2>();
        Vector3 move = transform.right * moveDir.x + transform.forward * moveDir.y;
        Vector3 newVel = new Vector3(move.x * moveSpeed, rb.velocity.y, move.z * moveSpeed);
        rb.velocity = newVel;
    }
    #endregion

    #region handle camera looking

    private void HandleCameraLooking()
    {
        #region horizontal looking
        float mouseX = lookInputValue.x * horizontalMouseSensitivity;
        transform.rotation = transform.rotation * Quaternion.Euler(0, mouseX, 0);
        #endregion

        #region vertical looking
        verticalLookRotation -= lookInputValue.y * verticalMouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        camera.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
        #endregion

    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        if (GameManager.gameManager.UIOpen)
        {
            return ;
        }

        lookInputValue = context.ReadValue<Vector2>();

        HandleInteraction();
    }

    private void OnLookCanceled(InputAction.CallbackContext context)
    {
        if (GameManager.gameManager.UIOpen)
        {
            return ;
        }
        lookInputValue = Vector2.zero;
    }
    #endregion

    #region handle interaction

    private void HandleInteraction()
    {
        if (camera == null)
        {
            Debug.LogError("Camera is not assigned!");
            return;
        }
        if (interactIndicator == null)
        {
            Debug.LogError("InteractIndicator is not assigned!");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, interactMaxDistance))
        {
            interactIndicator.transform.position = hit.point;
            hit.transform.gameObject.GetComponent<Interactable>()?.OnHover();

            //if hovering over something different, clear the hover on the old thing
            if (currentHoverObject != null && hit.transform.gameObject != currentHoverObject)
            {
                currentHoverObject?.GetComponent<Interactable>()?.ClearHover();
                currentHoverObject = hit.transform.gameObject;
            }else if (currentHoverObject==null)
            {
                currentHoverObject = hit.transform.gameObject;
            }
        }else if (currentHoverObject != null){
            currentHoverObject.GetComponent<Interactable>()?.ClearHover();
            currentHoverObject = null;
        }
    }
    

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (GameManager.gameManager.UIOpen)
        {
            return ;
        }
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, interactMaxDistance))
        {
            Collider collider = hit.collider;
            Rigidbody rigidbody = collider.attachedRigidbody;
            if (rigidbody != null)
            {
                rigidbody.gameObject.GetComponent<Interactable>()?.OnInteract();
            }else{
                collider.gameObject.GetComponent<Interactable>()?.OnInteract();
            }
        }
    }
    #endregion
}