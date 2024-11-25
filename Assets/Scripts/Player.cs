using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI expText; // Text แสดง EXP
    [SerializeField] TextMeshProUGUI levelText; // Text แสดงระดับเลเวล
    [SerializeField] float moveSpeed = 6;
    Animator anim;
    Rigidbody2D rb;

    int maxHealth = 100;
    int currenthealth;
    public int experiencePoints = 0;
    public int maxExp = 120; // EXP สูงสุดที่ต้องการเพื่อเพิ่มเลเวล
    public int level = 1; // ระดับเลเวลของผู้เล่น
    
    bool dead = false;

    float moveHorizontal, moveVertical;
    Vector2 movement;

    int facingDirection = 1; // 1 = right,-1 = left
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        currenthealth = maxHealth;
        healthText.text = maxHealth.ToString();

        // แสดงค่าเริ่มต้นของ level
        levelText.text = level.ToString(); // แสดงระดับเลเวลเริ่มต้น

    }
    public void AddExp(int amount)
    {
        experiencePoints += amount;
        
        // ตรวจสอบว่า EXP ถึง maxExp หรือยัง
        if (experiencePoints >= maxExp)
        {
            LevelUp(); // เรียกฟังก์ชันเพิ่มเลเวล
        }

    }
    void LevelUp()
    {
        level++; // เพิ่มระดับเลเวล
        experiencePoints -= maxExp; // หัก EXP ที่เหลือหลังจากเพิ่มเลเวล
        maxExp = Mathf.RoundToInt(maxExp * 1.6f); // เพิ่ม maxExp เป็น 120% ของเดิมเพื่อให้การเก็บ EXP ยากขึ้นในเลเวลถัดไป
        Debug.Log("Level Up! Current Level: " + level);

        // อัปเดตข้อความใน levelText
        levelText.text = level.ToString();
        
        
    }
    private void Update()
    {
        //Only for testing
        //if(Input.GetKeyDown(KeyCode.Space))
            //Hit(10);





        if (dead)
        {
            movement = Vector2.zero;
            anim.SetFloat("velocity", 0);
            return;
        }

        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveHorizontal, moveVertical).normalized;

        anim.SetFloat("velocity", movement.magnitude);

        if (movement.x != 0)
            facingDirection = movement.x > 0 ? 1 : -1;

        transform.localScale = new Vector2(facingDirection, 1);

    }
    private void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
            Hit(20);
    }

    void Hit(int damage)
    {
        anim.SetTrigger("hit");
        currenthealth -= damage;
        healthText.text = Mathf.Clamp(currenthealth, 0, maxHealth).ToString();

        if (currenthealth <= 0)
        {
            SceneManager.LoadScene("Lose");
        }
            //Die();
            

        
        
    }


    void Die()
    {
        dead = true;
        // Call GameOver
    }
}
