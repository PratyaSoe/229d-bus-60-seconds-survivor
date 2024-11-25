using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpgradePanels : Player
{
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI expText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] float moveSpeed = 6;

    [Header("Upgrade System")]
    [SerializeField] GameObject upgradePanel;
    [SerializeField] Button[] upgradeButtons;

    Animator anim;
    Rigidbody2D rb;

    int maxHealth = 100;
    int currenthealth;
    public int experiencePoints = 0;
    public int maxExp = 120;
    public int level = 1;

    bool dead = false;
    float moveHorizontal, moveVertical;
    Vector2 movement;
    int facingDirection = 1;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        currenthealth = maxHealth;
        healthText.text = maxHealth.ToString();
        levelText.text = level.ToString();

        // Disable upgrade panel at start
        upgradePanel.SetActive(false);

        // Setup upgrade buttons
        upgradeButtons[0].onClick.AddListener(() => UpgradeHealth());
        upgradeButtons[1].onClick.AddListener(() => UpgradeSpeed());
        upgradeButtons[2].onClick.AddListener(() => UpgradeDamage());
    }

    public void AddExp(int amount)
    {
        experiencePoints += amount;
        expText.text = $"{experiencePoints}/{maxExp}";

        if (experiencePoints >= maxExp)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        experiencePoints -= maxExp;
        maxExp = Mathf.RoundToInt(maxExp * 1.6f);

        // Pause game and show upgrade options
        Time.timeScale = 0;
        upgradePanel.SetActive(true);

        levelText.text = level.ToString();
        expText.text = $"0/{maxExp}";
    }

    void UpgradeHealth()
    {
        maxHealth += 20;
        currenthealth = maxHealth;
        healthText.text = maxHealth.ToString();
        CloseUpgradePanel();
    }

    void UpgradeSpeed()
    {
        moveSpeed += 1f;
        CloseUpgradePanel();
    }

    void UpgradeDamage()
    {
        // Placeholder for damage upgrade
        // You'd implement this based on your combat system
        Debug.Log("Damage upgrade selected");
        CloseUpgradePanel();
    }

    void CloseUpgradePanel()
    {
        upgradePanel.SetActive(false);
        Time.timeScale = 1;
    }

    // ... [Rest of the existing Player script remains the same]
}