using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI waveText;

    public float timeLimit = 60f; // เวลาที่กำหนดให้

    private float timer = 0f;
    public static WaveManager Instance;

    bool waveRunning = true;
    int currentWave = 0;
    int currentWaveTime;

    float enemyHealthMultiplier = 1.2f;
    float enemySpeedMultiplier = 1.1f;
    float enemyDamageMultiplier = 1.1f;
    private void Awake()
    {
        //NEW
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //NEW
        //if (Instance == null) Instance = this;
    }

    private void Start()
    {
        StartNewWave();
        waveText.text = "60";
        waveText.text = "Wave: 1";
    }

    private void Update()
    {
        timer += Time.deltaTime;
        //For testing
        if (Input.GetKeyDown(KeyCode.Space))
            StartNewWave();
        //NEW ตรวจสอบว่าครบ timeLimit เพื่อไป Scene Win หลังจากถึง wave สุดท้าย
        if (timer >= timeLimit && currentWave >= 5) // ตัวอย่างให้จบที่ wave 5
        {
            SceneManager.LoadScene("Win");
        }
        //NEW
        //if (timer >= timeLimit)
        //{
        //SceneManager.LoadScene("Win"); 
        //}
    }



    public bool WaveRunning() => waveRunning;

    private void StartNewWave()
    {
        StopAllCoroutines();
        timeText.color = Color.white;
        currentWave++;
        waveRunning = true;
        currentWaveTime = 60;
        waveText.text = "Wave:" + currentWave;

        //NEW
        float healthMultiplier = Mathf.Pow(enemyHealthMultiplier, currentWave - 1);
        float speedMultiplier = Mathf.Pow(enemySpeedMultiplier, currentWave - 1);
        float damageMultiplier = Mathf.Pow(enemyDamageMultiplier, currentWave - 1);

        EnemyManager.Instance?.UpdateEnemyStats(healthMultiplier, speedMultiplier, damageMultiplier);
        //NEW
        StartCoroutine(WaveTimer());
    }

    IEnumerator WaveTimer()
    {
       while(waveRunning)
        {
            yield return new WaitForSeconds(1f);
            currentWaveTime--;

            timeText.text = currentWaveTime.ToString();

            if (currentWaveTime <= 0)
            {
                WaveComplete();
                break; // ออกจาก loop เพื่อหยุด Coroutine นี้
            }
            //if (currentWaveTime <= 0)
            //WaveComplete();
        }
        //yield return null;
    }
    private void WaveComplete()
    {
        StopAllCoroutines();
        EnemyManager.Instance.DestroyAllEnemies();
        waveRunning = false;
        currentWaveTime = 60;
        timeText.text = currentWaveTime.ToString();
        timeText.color = Color.red;

        // เรียก StartNewWave เพื่อเริ่ม wave ถัดไป ถ้าไม่ใช่ wave สุดท้าย
        if (currentWave < 5) // สมมติว่าจบเกมเมื่อถึง wave 5
        {
            StartNewWave();
        }
    }

}
