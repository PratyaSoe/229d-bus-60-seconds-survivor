using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float timeBetweenSpawns = 0.5f;
    float currentTimeBetweenSpawns;


    Transform enemiesParent;

    public static EnemyManager Instance;
    // NEW: ตั้งค่ารัศมี (radius) สำหรับการเกิดของศัตรู
    [SerializeField] float spawnRadius = 35f; // ระยะห่างจากผู้เล่น
    //NEW
    private float healthMultiplier = 1f;
    private float speedMultiplier = 1f;

    //NEW
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }


    private void Start()
    {
        enemiesParent = GameObject.Find("Enemies").transform;
    }

    private void Update()
    {
        if (!WaveManager.Instance.WaveRunning()) return;
        currentTimeBetweenSpawns -= Time.deltaTime;

        if (currentTimeBetweenSpawns <= 0)
        {
            SpawnEnemy();
            currentTimeBetweenSpawns = timeBetweenSpawns;
        }
    }
    Vector2 RandomPositionAroundPlayer()
    {
        // หาตำแหน่งของผู้เล่น
        Transform playerTransform = GameObject.Find("Player").transform;

        // สุ่มตำแหน่งในรัศมีรอบๆ ผู้เล่น
        Vector2 randomDirection = Random.insideUnitCircle.normalized; // ทิศทางสุ่ม
        float randomDistance = Random.Range(35f, spawnRadius); // ระยะทางสุ่มในรัศมีที่กำหนด
        Vector2 spawnPosition = (Vector2)playerTransform.position + randomDirection * randomDistance; // คำนวณตำแหน่ง

        return spawnPosition;
    }
    //Vector2 RandomPosition()
    //{
    //return new Vector2(Random.Range(-16, 16), Random.Range(-8, 8));
    //}

    void SpawnEnemy()
    {
        var e = Instantiate(enemyPrefab, RandomPositionAroundPlayer(), Quaternion.identity);
        e.transform.SetParent(enemiesParent);

        //NEW ปรับค่าสถานะศัตรูตามตัวคูณที่ได้
        var enemy = e.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.UpdateStats(healthMultiplier, speedMultiplier);
        }
        //NEW
    }
    public void DestroyAllEnemies()
    {
        foreach (Transform e in enemiesParent)
            Destroy(e.gameObject);
    }
    //NEW ฟังก์ชันสำหรับอัปเดตตัวคูณความแข็งแกร่งของศัตรู
    public void UpdateEnemyStats(float newHealthMultiplier, float newSpeedMultiplier, float newDamageMultiplier)
    {
        healthMultiplier = newHealthMultiplier;
        speedMultiplier = newSpeedMultiplier;

    }
    //NEW
}
