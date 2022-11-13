using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

/// <summary>
///     This class makes player's movement work.
/// </summary>
[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("General")]
    [SerializeField]
    float playerSpeed = 3.5f;
    [SerializeField]
    float playerSprintSpeed = 4.5f;
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
    public bool isSprinting = false;

    [Header("Animators")]
    [SerializeField]
    private RuntimeAnimatorController gunAnim;
    [SerializeField]
    private RuntimeAnimatorController LongGunAnim;
    [SerializeField]
    private RuntimeAnimatorController noGunAnim;

    [Header("Guns")]
    [SerializeField]
    public GameObject pistol;
    [SerializeField]
    public GameObject uzi;
    [SerializeField]
    public GameObject rifle;
    public string gun = ""; //pistol, uzi, rifle

    [Header("Bullet")]
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform barrelTransform;
    [SerializeField]
    private Transform bulletParent;

    [Header("Components and Gravity")]
    private PlayerState state;
    private CharacterController controller;
    private Animator animator;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;
    public RewardsLoader rewards;
    public GameObject UI;

    [Header("Inventory")]
    public Item[] itemsToAdd;
    public Inventory myInventory = new Inventory(24);
    public Item currentPickedItem;

    [Header("Ammo")]
    public int pistolAmmo = 0;
    public int pistolAmmoInCharger = 0;
    public int maxPistolAmmo = 12;
    [Space(10)]
    public int uziAmmo = 0;
    public int uziAmmoInCharger = 0;
    public int maxUziAmmo = 20;
    [Space(10)]
    public int rifleAmmo = 0;
    public int rifleAmmoInCharger = 0;
    public int maxRifleAmmo = 30;
    [Space(2)]
    private bool isShootingAutomatic;

    [Header("Input")]
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction shootAction;
    private InputAction sprintAction;
    private InputAction reloadAction;
    private InputAction inventoryAction;

    private void Awake()
    {
        state = GetComponent<PlayerState>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];
        sprintAction = playerInput.actions["Sprint"];
        reloadAction = playerInput.actions["Reload"];
        inventoryAction = playerInput.actions["Inventory"];
    }

    private void OnEnable()
    {
        shootAction.Enable();
    }

    private void OnDisable()
    {
        shootAction.Disable();
    }

    private void Start()
    {
        bulletParent = GameObject.Find("BulletParent").transform;
        UI = GameObject.Find("UI");

        playerInput.camera = Camera.main;
        rewards = GameObject.FindWithTag("Settings").GetComponent<SettingsController>().rewards;
        rewards.zombiesKilled = 0;

        foreach (Item item in itemsToAdd)
        {
            if(item.type == Item.ItemType.Consumable)
            {
                myInventory.AddItem(new ItemStack(item, 2));
            }
            else if(item.name == "PistolAmmo")
            {
                pistolAmmo += 12;
                myInventory.AddItem(new ItemStack(item, 12));
            }
            else if (item.name == "UziAmmo")
            {
                uziAmmo += 20;
                myInventory.AddItem(new ItemStack(item, 20));
            }
            else if (item.name == "RifleAmmo")
            {
                rifleAmmo += 30;
                myInventory.AddItem(new ItemStack(item, 30));
            }
            else
            {
                myInventory.AddItem(new ItemStack(item, 1));
            }
        }

        pistolAmmoInCharger = maxPistolAmmo;
        uziAmmoInCharger = maxUziAmmo;
        rifleAmmoInCharger = maxRifleAmmo;

        pause = GameObject.Find("UI").GetComponent<PauseMenu>();
        InventoryManager.INSTANCE.OpenContainer(new ContainerPlayerHotbar(null, myInventory));
        isInventoryOpen = false;

        // Configure shooting input.
        shootAction.performed += context =>
        {
            if (context.interaction is HoldInteraction)
            {
                isShootingAutomatic = true;
            }
            else if (context.interaction is PressInteraction)
            {
                isShootingAutomatic = false;
            }

            // Decide action
            if(currentPickedItem != null)
            {
                // If the current item is a gun.
                if (currentPickedItem.type == Item.ItemType.Pistol || currentPickedItem.type == Item.ItemType.Uzi || currentPickedItem.type == Item.ItemType.Rifle)
                {
                    Shoot();
                }
                // If the current item is a consumable.
                if (currentPickedItem.type == Item.ItemType.Consumable)
                {
                    Consume();
                }
            }
        };

        shootAction.canceled += context =>
        {
            if (context.interaction is HoldInteraction)
            {
                isShootingAutomatic = false;
            }
        };
    }

    /// <summary>
    ///     This function checks if the player's got a gun and enough ammo. Then calls the bullet calculation function.
    /// </summary>
    private void Shoot()
    {
        if (CanGameUseMouseInput() && HasPickedAGun()) // Player has a gun and it's not in pause.
        {
            if(gun == "pistol" && pistolAmmoInCharger > 0) // Player's got a pistol and it has enough ammo.
            {
                CalculateBullet();
                pistolAmmo--;
                pistolAmmoInCharger--;

                foreach(var stack in myInventory.GetInventoryStacks())
                {
                    if(stack.GetItem() != null && stack.GetItem().name == "PistolAmmo")
                    {
                        stack.DecreaseAmount(1);
                        break;
                    }
                }
            }
            if (gun == "uzi" && uziAmmoInCharger > 0) // Player's got a uzi and it has enough ammo.
            {
                CalculateBullet();
                uziAmmo--;
                uziAmmoInCharger--;

                foreach (var stack in myInventory.GetInventoryStacks())
                {
                    if (stack.GetItem() != null && stack.GetItem().name == "UziAmmo")
                    {
                        stack.DecreaseAmount(1);
                        break;
                    }
                }
            }
            if (gun == "rifle" && rifleAmmoInCharger > 0) // Player's got a rifle and it has enough ammo.
            {
                CalculateBullet();
                rifleAmmo--;
                rifleAmmoInCharger--;

                foreach (var stack in myInventory.GetInventoryStacks())
                {
                    if (stack.GetItem() != null && stack.GetItem().name == "RifleAmmo")
                    {
                        stack.DecreaseAmount(1);
                        break;
                    }
                }
            }
            InventoryManager.INSTANCE.currentOpenContainer.updateSlots();
        }

        // If the gun is automatic and the player is still pressing the button, it will repeat the action.
        if (isShootingAutomatic && currentPickedItem.isSpameable)
        {
            if(gun == "uzi")
                Invoke("Shoot", 0.3f);
            if (gun == "rifle")
                Invoke("Shoot", 0.15f);
        }
    }

    /// <summary>
    ///     This function describes the calculation of the shot with a ray launched at the middle of the screen, where the player's reticle may be.
    /// </summary>
    public void CalculateBullet()
    {
        RaycastHit hit;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.Euler(90, 0, 0), bulletParent);
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

    public void Reload()
    {
        if(gun == "pistol")
        {
            if (pistolAmmo - pistolAmmoInCharger > 0) // We still have bullets that are not charged already and we can reload.
            {
                // How many bullets need to be reloaded
                int difAmmo = maxPistolAmmo - pistolAmmoInCharger;
                if (pistolAmmo - pistolAmmoInCharger < difAmmo) // We don't have enough ammo to reload the full difAmmo.
                    difAmmo = pistolAmmo - pistolAmmoInCharger;

                if (pistolAmmo < difAmmo) // Case we don't have enough ammo to do a full charger reload.
                    difAmmo = pistolAmmo;

                pistolAmmoInCharger += difAmmo;
            }
        }

        if (gun == "uzi")
        {
            if (uziAmmo - uziAmmoInCharger > 0) // We still have bullets that are not charged already and we can reload.
            {
                // How many bullets need to be reloaded
                int difAmmo = maxUziAmmo - uziAmmoInCharger;
                if (uziAmmo - uziAmmoInCharger < difAmmo) // We don't have enough ammo to reload the full difAmmo.
                    difAmmo = uziAmmo - uziAmmoInCharger;

                if (uziAmmo < difAmmo) // Case we don't have enough ammo to do a full charger reload.
                    difAmmo = uziAmmo;

                uziAmmoInCharger += difAmmo;
            }
        }

        if (gun == "rifle")
        {
            if (rifleAmmo - rifleAmmoInCharger > 0) // We still have bullets that are not charged already and we can reload.
            {
                // How many bullets need to be reloaded
                int difAmmo = maxRifleAmmo - rifleAmmoInCharger;
                if (rifleAmmo - rifleAmmoInCharger < difAmmo) // We don't have enough ammo to reload the full difAmmo.
                    difAmmo = rifleAmmo - rifleAmmoInCharger;
                
                if (rifleAmmo < difAmmo) // Case we don't have enough ammo to do a full charger reload.
                    difAmmo = rifleAmmo;

                rifleAmmoInCharger += difAmmo;
            }
        }
    }

    public void Consume()
    {
        if (!isInventoryOpen)
        {
            if (currentPickedItem.itemName == "Medkit")
                state.currentHealth += 12;
            if (currentPickedItem.itemName == "Pills")
                state.currentHealth += 15;
            if (currentPickedItem.itemName == "Water")
                state.currentThirst += 15;
            if (currentPickedItem.itemName == "Food Can")
                state.currentHunger += 6;

            myInventory.GetStackInSlot(selectedHotbarIndex).DecreaseAmount(1);
            InventoryManager.INSTANCE.currentOpenContainer.updateSlots();
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

    /// <summary>
    ///     Returns a boolean that will indicate if the player has selected a gun at the inventory's hotbar.
    ///     Also, if the player doesn't have a gun, it'll set it unactive and change the animator.
    /// </summary>
    private bool HasPickedAGun()
    {
        // Empty slot.
        if (currentPickedItem == null)
        {
            gun = "none";
            pistol.SetActive(false);
            uzi.SetActive(false);
            rifle.SetActive(false);
            animator.runtimeAnimatorController = noGunAnim;
            return false;
        }

        // Slot contains a hand gun.
        // Pistol
        if (currentPickedItem.type == Item.ItemType.Pistol)
        {
            gun = "pistol";
            pistol.SetActive(true);
            uzi.SetActive(false);
            rifle.SetActive(false);
            barrelTransform = pistol.transform.Find("Barrel").gameObject.transform;
            animator.runtimeAnimatorController = gunAnim;
            return true;
        }
        // Uzi
        if (currentPickedItem.type == Item.ItemType.Uzi)
        {
            gun = "uzi";
            pistol.SetActive(false);
            uzi.SetActive(true);
            rifle.SetActive(false);
            barrelTransform = uzi.transform.Find("Barrel").gameObject.transform;
            animator.runtimeAnimatorController = gunAnim;
            return true;
        }
        // Rifle
        if (currentPickedItem.type == Item.ItemType.Rifle)
        {
            gun = "rifle";
            pistol.SetActive(false);
            uzi.SetActive(false);
            rifle.SetActive(true);
            barrelTransform = rifle.transform.Find("Barrel").gameObject.transform;
            animator.runtimeAnimatorController = LongGunAnim;
            return true;
        }

        // Slot contains another type of object.
        gun = "none";
        pistol.SetActive(false);
        uzi.SetActive(false);
        rifle.SetActive(false);
        animator.runtimeAnimatorController = noGunAnim;
        return false;
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
            isSprinting = false;
        } else {
            isMoving = true;
        }
        // Move according to the camera's direction.
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f; // Let's avoid some weird and random jumps all along the movement.

        // Change movement's speed and animation if the player's sprinting or not.
        if (isMoving)
        {
            if (sprintAction.ReadValue<float>() > 0.1 && !state.isTired)
            {
                controller.Move(move * Time.deltaTime * playerSprintSpeed);
                animator.SetBool("Walk", false);
                animator.SetBool("Sprint", true);
                isSprinting = true;
            }
            else
            {
                controller.Move(move * Time.deltaTime * playerSpeed);
                animator.SetBool("Walk", true);
                animator.SetBool("Sprint", false);
                isSprinting = false;
            }
        }

        // Changes the height position of the player if it's standing at the floor.
        if (jumpAction.triggered && groundedPlayer)
        {
            animator.Play("Jump");
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        // Calls reload function.
        if (reloadAction.triggered && HasPickedAGun())
            Invoke("Reload", 1.2f);

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
                UI.SetActive(false);

                pause.cinemachineNormal.XYAxis.action.Disable();
                pause.cinemachineAim.XYAxis.action.Disable();
            }
            else
            {
                InventoryManager.INSTANCE.OpenContainer(new ContainerPlayerHotbar(null, myInventory));
                isInventoryOpen = false;

                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                UI.SetActive(true);

                pause.cinemachineNormal.XYAxis.action.Enable();
                pause.cinemachineAim.XYAxis.action.Enable();
            }
        }
        if(!isInventoryOpen && !pause.IsPaused())
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

        currentPickedItem = GetCurrentPickedItem(selectedHotbarIndex);
    }

    /// <summary>
    ///     Returns the current selected item in the hotbar.
    /// </summary>
    /// <param name="index"> Receives the current selected item's index in order to look for it. </param>
    /// <returns></returns>
    public Item GetCurrentPickedItem(int index)
    {
        if (myInventory.GetStackInSlot(selectedHotbarIndex).GetItem() != null)
        {
            HasPickedAGun();
            return myInventory.GetStackInSlot(selectedHotbarIndex).GetItem();
        }
        else
        {
            HasPickedAGun();
            return null;
        }
    }

    /// <summary>
    ///     Returns the current selected item's index in the hotbar.
    /// </summary>
    public int GetSelectedHotbarIndex()
    {
        return selectedHotbarIndex;
    }

    /// <summary>
    ///     Sets the walk and sprint speed according with the given parameters.
    /// </summary>
    /// <param name="playerAttribute"> Gets the speed value from the character's interface. </param>
    public void SetSpeed(int playerAttribute)
    {
        playerSpeed = (playerSpeed * (playerAttribute * 100 / 99)) / 100;
        playerSprintSpeed = (playerSprintSpeed * (playerAttribute * 100 / 99)) / 100;
    }
}