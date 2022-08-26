using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     This class makes player's movement work.
/// </summary>
[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float playerSprintSpeed = 3.4f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 10f;
    [SerializeField]
    private float bulletHitMissDistance = 25f;
    private bool isMoving;
    private bool isInventoryOpen;
    private int selectedHotbarIndex = 0;
    private PauseMenu pause;

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform barrelTransform;
    [SerializeField]
    private Transform bulletParent;

    private CharacterController controller;
    private Animator animator;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;

    public GameObject reticlePanel;
    public Item[] itemsToAdd;
    private Inventory myInventory = new Inventory(24);

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction shootAction;
    private InputAction sprintAction;
    private InputAction inventoryAction;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];
        sprintAction = playerInput.actions["Sprint"];
        inventoryAction = playerInput.actions["Inventory"];
    }

    private void Start()
    {
        foreach (Item item in itemsToAdd)
        {
            myInventory.AddItem(new ItemStack(item, 1));
        }
        pause = GameObject.Find("UI").GetComponent<PauseMenu>();
        InventoryManager.INSTANCE.OpenContainer(new ContainerPlayerHotbar(null, myInventory));
        isInventoryOpen = false;
    }

    private void OnEnable()
    {
        shootAction.performed += _ => Shoot();
    }

    private void OnDisable()
    {
        shootAction.performed -= _ => Shoot();
    }

    /// <summary>
    ///     This function describes the calculation of the shot with a ray launched at the middle of the screen, where the player's reticle may be.
    /// </summary>
    private void Shoot()
    {
        if (CanGameUseMouseInput())
        {
            RaycastHit hit;
            GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            // Calculate if the bullet will collide with any object
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
            {
                bulletController.target = hit.point;
                bulletController.hit = true;
            }
            else
            {
                // If the bullet will not hit anything in certain distance, it'll send this information to the bullet's handler, so the bullet can be destroyed.
                bulletController.target = cameraTransform.position + cameraTransform.forward * bulletHitMissDistance;
                bulletController.hit = false;
            }
        }
    }

    /// <summary>
    ///     Returns a boolean that will indicate if the game can use the mouse input on gameplay or not.
    /// </summary>
    private bool CanGameUseMouseInput()
    {
        if (!isInventoryOpen)
        {
            if (pause.IsPaused())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        
        // Detect if the character is stopped, so we can set its animation.
        if(input.x == 0 && input.y == 0) {
            isMoving = false;
            animator.SetBool("Walk", false);
            animator.SetBool("Sprint", false);
        } else {
            isMoving = true;
        }
        // Move according to the camera's direction.
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f; // Let's avoid some weird and random jumps all along the movement.

        // Change movement's speed and animation if the player's sprinting or not.
        if (isMoving)
        {
            if (sprintAction.ReadValue<float>() > 0.1)
            {
                controller.Move(move * Time.deltaTime * playerSprintSpeed);
                animator.SetBool("Walk", false);
                animator.SetBool("Sprint", true);
            }
            else
            {
                controller.Move(move * Time.deltaTime * playerSpeed);
                animator.SetBool("Walk", true);
                animator.SetBool("Sprint", false);
            }
        }

        // Changes the height position of the player if it's standing at the floor.
        if (jumpAction.triggered && groundedPlayer)
        {
            animator.Play("Jump");
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        // Opens and close the inventory
        if (inventoryAction.triggered)
        {
            if (!isInventoryOpen)
            {
                InventoryManager.INSTANCE.OpenContainer(new ContainerPlayerInventory(null, myInventory));
                isInventoryOpen = true;

                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                reticlePanel.SetActive(false);
            }
            else
            {
                InventoryManager.INSTANCE.OpenContainer(new ContainerPlayerHotbar(null, myInventory));
                isInventoryOpen = false;

                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                reticlePanel.SetActive(true);
            }
        }
        UpdateSelectedHotbarIndex(Mouse.current.scroll.y.ReadValue());

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Rotate player towards camera's direction.
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    ///     Updates the selected item in the hotbar according with the mouse wheel's input.
    /// </summary>
    /// <param name="direction"> Direction in which the item selection will go. It's determined with the mouse wheel's input </param>
    private void UpdateSelectedHotbarIndex(float direction)
    {
        if (direction > 0)
            direction = 1;
        if (direction < 0)
            direction = -1;

        for (selectedHotbarIndex -= (int)direction; selectedHotbarIndex < 0; selectedHotbarIndex += 6);
        
        while(selectedHotbarIndex >= 6)
        {
            selectedHotbarIndex -= 6;
        }
    }

    /// <summary>
    ///     Returns the current selected item's index in the hotbar.
    /// </summary>
    public int GetSelectedHotbarIndex()
    {
        return selectedHotbarIndex;
    }
}