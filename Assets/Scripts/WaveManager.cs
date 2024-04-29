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

    private void Awake()
    {
        if (Instance == null) Instance = this;
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
        if (timer >= timeLimit)
        {
            SceneManager.LoadScene("Win"); 
        }
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
                WaveComplete();
        }
        yield return null;
    }
    private void WaveComplete()
    {
        StopAllCoroutines();
        EnemyManager.Instance.DestroyAllEnemies();
        waveRunning = false;
        currentWaveTime = 60;
        timeText.text = currentWaveTime.ToString();
        timeText.color = Color.red;
    }

}
