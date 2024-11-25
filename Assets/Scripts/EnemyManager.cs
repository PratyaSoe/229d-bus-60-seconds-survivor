using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float timeBetweenSpawns = 0.2f;
    float currentTimeBetweenSpawns;


    Transform enemiesParent;

    public static EnemyManager Instance;
    // NEW: ��駤������� (radius) ����Ѻ����Դ�ͧ�ѵ��
    [SerializeField] float spawnRadius = 35f; // ������ҧ�ҡ������
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
        // �ҵ��˹觢ͧ������
        Transform playerTransform = GameObject.Find("Player").transform;

        // �������˹��������ͺ� ������
        Vector2 randomDirection = Random.insideUnitCircle.normalized; // ��ȷҧ����
        float randomDistance = Random.Range(35f, spawnRadius); // ���зҧ���������շ���˹�
        Vector2 spawnPosition = (Vector2)playerTransform.position + randomDirection * randomDistance; // �ӹǳ���˹�

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

        //NEW ��Ѻ���ʶҹ��ѵ�ٵ����Ǥٳ�����
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
    //NEW �ѧ��ѹ����Ѻ�ѻവ��Ǥٳ��������觢ͧ�ѵ��
    public void UpdateEnemyStats(float newHealthMultiplier, float newSpeedMultiplier, float newDamageMultiplier)
    {
        healthMultiplier = newHealthMultiplier;
        speedMultiplier = newSpeedMultiplier;

    }
    //NEW
}
