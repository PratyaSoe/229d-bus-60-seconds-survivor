using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public GameObject FloatingTextPrefab;
    [SerializeField] int maxHealth = 200;
    [SerializeField] float speed = 2f;
    [SerializeField] float currentMoveSpeed;
    [SerializeField] int expDropAmount = 50;
    [SerializeField] private GameObject healthPickupPrefab; // Prefab ‰Õ‡∑Á¡Œ’≈
    [SerializeField] private float itemDropChance = 0.2f; // ‚Õ°“ ¥√Õª (20%)

    private void Initialize()
    {
        currentMoveSpeed = speed;
    }

    private void OnEnable()
    {
        Initialize();
    }

    [SerializeField] private int currentHealth;

    Animator anim;
    Transform target;

    private void Start()
    {
        currentHealth = maxHealth;
        target = GameObject.Find("Player").transform;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            direction.Normalize();

            transform.position += direction * speed * Time.deltaTime;

            var playerToTheRight = target.position.x > transform.position.x;
            transform.localScale = new Vector2(playerToTheRight ? -1 : 1, 1);
        }
    }

    public void Hit(int damage)
    {
        currentHealth -= damage;
        
        // Show popup damage
        if (FloatingTextPrefab != null)
        {
            ShowFloatingText(damage);
        }

        anim.SetTrigger("hit");
        
        if (currentHealth <= 0)
        {
            DropExp();
            Destroy(gameObject);
        }
    }

    private void ShowFloatingText(int damage)
{
    // Instantiate the floating text at the enemy's position
    GameObject floatingText = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity);

    // Get the TextMesh or TextMeshPro component and set the damage text
    TextMesh textMesh = floatingText.GetComponent<TextMesh>();
    if (textMesh != null)
    {
        textMesh.text = damage.ToString();
    }

    // Add fade-out script to the floating text
    FloatingText floatingTextScript = floatingText.AddComponent<FloatingText>();
    floatingTextScript.StartFadeOut(1.5f, floatSpeed: 1f); // 1.5 seconds to fade out
}

    public void UpdateStats(float healthMultiplier, float speedMultiplier)
    {
        maxHealth = Mathf.RoundToInt(maxHealth * healthMultiplier);
        currentHealth = maxHealth;
        speed *= speedMultiplier;
    }

    private void DropExp()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.AddExp(expDropAmount);
            Debug.Log("Player gained " + expDropAmount + " EXP");
        }
        //  ÿË¡¥√Õª‰Õ‡∑Á¡Œ’≈
        if (healthPickupPrefab != null && Random.value <= itemDropChance)
        {
            Instantiate(healthPickupPrefab, transform.position, Quaternion.identity);
            Debug.Log("Dropped Health Pickup!");
        }
    }

    public float GetMoveSpeed()
    {
        return currentMoveSpeed;
    }

    public void SetMoveSpeed(float newSpeed)
    {
        currentMoveSpeed = Mathf.Max(0, newSpeed);

        if (anim != null)
        {
            anim.SetFloat("MoveSpeed", speed / currentMoveSpeed);
        }
    }

    public void ResetMoveSpeed()
    {
        SetMoveSpeed(speed);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnDrawGizmosSelected()
    {
        if (TryGetComponent(out Collider2D collider))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
        }
    }
}