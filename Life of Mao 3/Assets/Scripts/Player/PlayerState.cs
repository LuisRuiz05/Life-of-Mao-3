using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
///     This script is in charge of calculating current player's statistics
/// </summary>
public class PlayerState : MonoBehaviour
{
    public Character character;
    public PlayerController controller;

    [Header("Player Info")]
    public string name;
    public Sprite icon;
    public int strength;
    public int speed;
    public int maxStamina;
    public int maxHealth;

    [Header("UI")]
    public Image characterImageDisplay;
    public Image healthBar;
    public Image staminaBar;
    public Image bulletImage;

    public Text healtText;
    public Text thirstText;
    public Text hungerText;
    public Text bulletText;

    public Timer timer;

    public Image bleedingImage;
    public Image bleedingPanel;

    [Header("Actual Statistics")]
    public float currentHealth;
    public float currentStamina;
    public float currentThirst;
    public float currentHunger;

    // Stamina
    public bool isTired = false;
    private bool needToRecover = false;

    void Start()
    {
        // Get player info from character interface.
        name = character.name;
        icon = character.icon;
        strength = character.strength;
        speed = character.speed;
        maxStamina = character.stamina;
        maxHealth = character.health;

        // Set Data
        controller = GetComponent<PlayerController>();
        controller.SetSpeed(speed);

        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentHunger = 100;
        currentThirst = 100;

        timer = GameObject.Find("UI/Timer").GetComponent<Timer>();

        // Get UI componentes
        characterImageDisplay = GameObject.Find("UI/PlayerData/Background/Character/Image").GetComponent<Image>();
        healthBar = GameObject.Find("UI/PlayerData/Background/Bars/Health").GetComponent<Image>();
        staminaBar = GameObject.Find("UI/PlayerData/Background/Bars/Stamina").GetComponent<Image>();
        healtText = GameObject.Find("UI/PlayerData/Background/Text/Health").GetComponent<Text>();
        thirstText = GameObject.Find("UI/PlayerData/Water/Text").GetComponent<Text>();
        hungerText = GameObject.Find("UI/PlayerData/Food/Text").GetComponent<Text>();
        bulletText = GameObject.Find("UI/PlayerData/Ammo/Quantity").GetComponent<Text>();
        bleedingImage = GameObject.Find("UI/Bleeding").GetComponent<Image>();
        bleedingPanel = GameObject.Find("UI/Bleeding/BleedingBg").GetComponent<Image>();

        // Set UI Values
        characterImageDisplay.sprite = icon;
        healtText.text = currentHealth + "/" + maxHealth;
        thirstText.text = currentThirst + "/100";
        hungerText.text = currentHunger + "/100";

        // Initializing hunger and thirst
        Invoke("GetHungry", 45f);
        Invoke("GetThirsty", 45f);

    }

    void Update()
    {
        GetStaminaDown();
        UpdateBars();
        UpdateAmmo();
        Die();
    }

    /// <summary>
    ///     Reduces the player's stamina while it is sprinting.
    /// </summary>
    public void GetStaminaDown()
    {
        // Check if player is able to sprint.
        if(currentStamina <= 0)
        {
            // Player can't have a negative stamina, so it will be set in 0.
            if(currentStamina <= 0)
            {
                currentStamina = 0;
            }
            needToRecover = true;
            isTired = true;
        } else
        {
            isTired = false;
        }

        // Sprint
        if (controller.isSprinting && !isTired)
        {
            currentStamina -= 2.5f * Time.deltaTime;
            needToRecover = true;
        }

        // Recovering
        if (needToRecover && !controller.isSprinting)
        {
            Invoke("RecoverStamina", 2f);
        }
    }

    /// <summary>
    ///     Updates the ammo counter in order to the remaining bullets.
    /// </summary>
    public void UpdateAmmo()
    {
        if (controller.pistol.activeSelf)
            bulletText.text = controller.pistolAmmoInCharger + "/" + controller.maxPistolAmmo;
        else if (controller.uzi.activeSelf)
            bulletText.text = controller.uziAmmoInCharger + "/" + controller.maxUziAmmo;
        else if (controller.rifle.activeSelf)
            bulletText.text = controller.rifleAmmoInCharger + "/" + controller.maxRifleAmmo;
        else
            bulletText.text = "-";
    }

    /// <summary>
    ///     Updates the hunger statistic.
    /// </summary>
    public void GetHungry()
    {
        currentHunger -= 1f;
        Invoke("GetHungry", 1f);
    }

    /// <summary>
    ///     Updates the thirst statistic.
    /// </summary>
    public void GetThirsty()
    {
        currentThirst -= 1f;
        Invoke("GetThirsty", 1f);
    }

    /// <summary>
    ///     Ends the game when the player gets health to 0 or below.
    /// </summary>
    public void Die()
    {
        if(currentHealth <= 0)
        {
            currentHealth = 0;

            controller.rewards.missionState = false;
            controller.rewards.time = timer.maxTime - timer.time;

            SceneManager.LoadScene(3);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    /// <summary>
    ///     Recovers player's stamina if the player's tired and it hasn't sprinted in the past 2 seconds.
    /// </summary>
    public void RecoverStamina()
    {
        // If player sprints before the recuperation time, it'll not recover stamina.
        // If player is recovering stamina and it sprints again, stamina will stop recovering.
        if (controller.isSprinting)
            return;

        // Check if stamina needs to stop recovering.
        if(currentStamina >= maxStamina)
        {
            needToRecover = false;
            // Player can't exceed max stamina, so it will be set in the maximum.
            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
            return;
        }

        // Recover.
        currentStamina += 4f * Time.deltaTime;
    }

    /// <summary>
    ///     Updates the player's statistics i the UI.
    /// </summary>
    public void UpdateBars()
    {
        // Fix exceding values.
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        if (currentHunger > 100)
            currentHunger = 100;
        if (currentThirst > 100)
            currentThirst = 100;

        // Update bars.
        staminaBar.fillAmount = currentStamina / maxStamina;
        healthBar.fillAmount = currentHealth / maxHealth;
        healtText.text = currentHealth + "/" + maxHealth;
        hungerText.text = currentHunger + "/" + 100;
        thirstText.text = currentThirst + "/" + 100;

        // Update damage screen.
        // More than 3/4 health
        if (currentHealth > 3 * maxHealth / 4)
        {
            bleedingImage.color = new Color(255, 0, 0, 0);
            bleedingPanel.color = new Color(255, 0, 0, 0);
        }
        // Between 3/4 and 1/2 health
        else if (currentHealth > maxHealth / 2)
        {
            bleedingImage.color = new Color(255, 0, 0, 0.5f);
        }
        // Between 1/2 and 1/4 health
        else if (currentHealth > maxHealth / 4)
        {
            bleedingImage.color = new Color(255, 0, 0, 1);
            bleedingPanel.color = new Color(255, 0, 0, 0.125f);
        }
        // Less than 1/4 health
        else 
            bleedingPanel.color = new Color(255, 0, 0, 0.25f);
    }
}
