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

    private InventoryManager inventory;

    #region enable/disable InputActions
    private void OnEnable()
    {
        movementControls.Enable();

        lookControls.Enable();

        interactControls.Enable();
        interactControls.performed += OnInteract;
    }

    private void OnDisable()
    {
        movementControls.Disable();

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
        inventory = gameObject.GetComponent<InventoryManager>();
    }

    void Update()
    {
        if (!UIManager.uiManager.UIOpen)
        {
            lookInputValue = lookControls.ReadValue<Vector2>();
            HandleCameraLooking();
            HandleInteraction();
        }
    }

    void FixedUpdate()
    {        
        HandlePlayerMovement();
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
    

    private void OnInteract(InputAction.CallbackContext context) //called when interact key is clicked
    {
        if (UIManager.uiManager.UIOpen)
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
                rigidbody.gameObject.GetComponent<Interactable>()?.OnInteract(inventory.GetActiveItem());
            }else{
                collider.gameObject.GetComponent<Interactable>()?.OnInteract(inventory.GetActiveItem());
            }
        }
    }
    #endregion
}