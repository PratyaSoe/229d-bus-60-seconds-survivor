using UnityEngine;
using System.Collections.Generic;
 
    public enum ElementType
    {
    Fire = 1,    // Mapped to key 1
    Water = 2,   // Mapped to key 2
    Ice = 3,     // Mapped to key 3
    Electric = 4 // Mapped to key 4
    }
 
    public class ProjectileGo : MonoBehaviour
    {
    [SerializeField] private SpriteRenderer sprite;
 
    [Header("Base Stats")]
    [SerializeField] private float speed = 18f;
    [SerializeField] private int baseDamage = 100;
    [Header("Element Settings")]
    [SerializeField] public ElementType elementType = ElementType.Fire;
    [SerializeField] private ParticleSystem elementalEffect;
    [Header("Status Effect Settings")]
    [SerializeField] private float burnDuration = 3f;
    [SerializeField] private float burnTickDamage = 20f;
    [SerializeField] private float freezeDuration = 2f;
    [SerializeField] private float slowPercent = 0.5f;
    [Header("Electric Chain Settings")]
    [SerializeField] private float chainRange = 5f;
    [SerializeField] private int maxChainTargets = 4;
    [SerializeField] private float chainDamageReduction = 0.7f;
    [SerializeField] private LineRenderer chainLightning;
    private List<Enemy> hitEnemies = new List<Enemy>();
    [SerializeField] private GameObject lightningEffectPrefab;

    private void Start()
    {
        SetupElementalEffects();
    }
 
    private void Update()
    {
        SetupElementalEffects();
        // Check for number key inputs (1-4)
        for (int i = 1; i <= 4; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i) || Input.GetKeyDown(KeyCode.Keypad0 + i))
            {
                ElementType newElement = (ElementType)i;
                if (elementType != newElement)
                {
                    elementType = newElement;
                    Debug.Log($"Switched to {elementType} element!");
                }
            }
        }
    }
 
 
    private void SetupElementalEffects()
    {
        switch (elementType)
        {
            case ElementType.Fire:
                sprite.color = new Color32(255, 0, 0, 255);
                break;
            case ElementType.Water:
                sprite.color = new Color32(0, 228, 255, 255);
                break;
            case ElementType.Ice:
                sprite.color = new Color32(218, 252, 255, 255);
                break;
            case ElementType.Electric: 
                sprite.color = new Color32(253, 255, 0, 255);
                break;
        }      
    }
 
    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
 
    private void OnTriggerEnter2D(Collider2D collision)
{
    Debug.Log($"Collision detected with: {collision.gameObject.name}");
    Enemy enemy = collision.gameObject.GetComponent<Enemy>();
    if (enemy != null)
    {
        Debug.Log($"Enemy found: {enemy.name}");
        ApplyElementalEffect(enemy);
        if (elementType != ElementType.Electric)
        {
            Destroy(gameObject);
        }
    }
}
 
    private void ApplyElementalEffect(Enemy primaryTarget)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                ApplyFireDamage(primaryTarget);
                break;
            case ElementType.Water:
                ApplyWaterDamage(primaryTarget);
                break;
            case ElementType.Ice:
                ApplyIceDamage(primaryTarget);
                break;
            case ElementType.Electric:
                ApplyElectricDamage(primaryTarget);
                break;
        }
    }
 
    private void ApplyFireDamage(Enemy enemy)
    {
        enemy.Hit(baseDamage);
        StartBurning(enemy);
        Debug.Log($"Applied fire damage: {baseDamage} initial + burn effect");
    }
 
 
    private void StartBurning(Enemy enemy)
    {
        // Add or refresh burn status effect
        BurnEffect burn = enemy.gameObject.GetComponent<BurnEffect>();
        if (burn == null)
        {
            burn = enemy.gameObject.AddComponent<BurnEffect>();
        }
        burn.StartBurn(burnDuration, burnTickDamage);
    }
 
    private void ApplyWaterDamage(Enemy enemy)
    {
        enemy.Hit(baseDamage);
        // Water makes enemies more vulnerable to electric damage
        WaterEffect wet = enemy.gameObject.GetComponent<WaterEffect>();
        if (wet == null)
        {
            wet = enemy.gameObject.AddComponent<WaterEffect>();
        }
        wet.ApplyWet(3f); // Wet for 3 seconds
    }
 
 
    private void ApplyIceDamage(Enemy enemy)
    {
        enemy.Hit(baseDamage);
        FreezeEffect freeze = enemy.gameObject.GetComponent<FreezeEffect>();
        if (freeze == null)
        {
            freeze = enemy.gameObject.AddComponent<FreezeEffect>();
        }
        freeze.Freeze(freezeDuration, slowPercent);
        Debug.Log($"Applied ice damage: {baseDamage} + {slowPercent * 100}% slow");
    }
 
    private void ApplyElectricDamage(Enemy primaryTarget)
    {
        hitEnemies.Add(primaryTarget);
        primaryTarget.Hit(baseDamage);
        Debug.Log($"Applied electric damage to primary target: {baseDamage}");
        ChainLightning(primaryTarget.transform.position, baseDamage * chainDamageReduction, maxChainTargets);
    }
 
    private void ChainLightning(Vector2 sourcePosition, float chainDamage, int remainingChains)
    {
        if (remainingChains <= 0) return;
 
        // Find all enemies in range
        Collider2D[] colliders = Physics2D.OverlapCircleAll(sourcePosition, chainRange);
        Enemy nearestEnemy = null;
        float nearestDistance = float.MaxValue;
 
        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null && !hitEnemies.Contains(enemy))
            {
                float distance = Vector2.Distance(sourcePosition, enemy.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }
 
        if (nearestEnemy != null)
        {

            // วาดเส้นสายฟ้า
            DrawChainLightning(sourcePosition, nearestEnemy.transform.position);

            // สร้างเอฟเฟกต์ที่เป้าหมาย
            CreateDamageEffect(nearestEnemy.transform.position);

            // ทำดาเมจ
            hitEnemies.Add(nearestEnemy);
            nearestEnemy.Hit(Mathf.RoundToInt(chainDamage));
            
            // Apply damage and continue chain
            hitEnemies.Add(nearestEnemy);
            nearestEnemy.Hit(Mathf.RoundToInt(chainDamage));
            // Check if target is wet for bonus damage
            WaterEffect wet = nearestEnemy.GetComponent<WaterEffect>();
            if (wet != null && wet.IsWet)
            {
                nearestEnemy.Hit(Mathf.RoundToInt(chainDamage * 0.5f)); // 50% bonus damage to wet targets
            }
 
            ChainLightning(nearestEnemy.transform.position, chainDamage * chainDamageReduction, remainingChains - 1);
        }

    }
 
    private void DrawChainLightning(Vector2 start, Vector2 end)
    {
        if (chainLightning == null) return;
 
        chainLightning.positionCount = 2;
        chainLightning.SetPosition(0, start);
        chainLightning.SetPosition(1, end);
        // Create a temporary visual effect
        //Destroy(chainLightning.gameObject, 0.1f);
        if (lightningEffectPrefab != null)
        {
            GameObject lightningEffect = Instantiate(lightningEffectPrefab, start, Quaternion.identity);
            LineRenderer lightningLine = lightningEffect.GetComponent<LineRenderer>();
            if (lightningLine != null)
            {
                lightningLine.SetPosition(0, start);
                lightningLine.SetPosition(1, end);
            }
            Destroy(lightningEffect, 0.5f); // Destroy effect after animation
        }
    }

    private void CreateDamageEffect(Vector2 position)
    {
        if (lightningEffectPrefab != null)
        {
            GameObject effect = Instantiate(lightningEffectPrefab, position, Quaternion.identity);
            ParticleSystem particles = effect.GetComponent<ParticleSystem>();
            if (particles != null)
            {
                particles.Play();
            }
            Destroy(effect, 0.3f); // ทำลายหลัง  วินาที
        }
    }

}


// Status Effect Components
public class BurnEffect : MonoBehaviour
{
    private float duration;
    private float tickDamage;
    private float nextTickTime;
    private float endTime;
 
    public void StartBurn(float burnDuration, float damage)
    {
        duration = burnDuration;
        tickDamage = damage;
        endTime = Time.time + duration;
        nextTickTime = Time.time;
    }
 
    private void Update()
    {
        if (Time.time > endTime)
        {
            Destroy(this);
            return;
        }
 
        if (Time.time >= nextTickTime)
        {
            GetComponent<Enemy>()?.Hit(Mathf.RoundToInt(tickDamage));
            nextTickTime = Time.time + 0.5f; // Damage every 0.5 seconds
        }
    }
}
 
public class WaterEffect : MonoBehaviour
{
    public bool IsWet { get; private set; }
    private float wetEndTime;
 
    public void ApplyWet(float duration)
    {
        IsWet = true;
        wetEndTime = Time.time + duration;
    }
 
    private void Update()
    {
        if (Time.time > wetEndTime)
        {
            IsWet = false;
            Destroy(this);
        }
    }
}
 
public class FreezeEffect : MonoBehaviour
{
    private Enemy enemy;
    private float originalSpeed;
    private float endTime;
 
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
 
    public void Freeze(float duration, float slowPercent)
    {
        if (enemy != null)
        {
            originalSpeed = enemy.GetMoveSpeed();
            enemy.SetMoveSpeed(originalSpeed * (1 - slowPercent));
            endTime = Time.time + duration;
        }
    }
 
    private void Update()
    {
        if (Time.time > endTime)
        {
            if (enemy != null)
            {
                enemy.SetMoveSpeed(originalSpeed);
            }
            Destroy(this);
        }
    }
}