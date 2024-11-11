using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] float moveSpeed = 6;
    Animator anim;
    Rigidbody2D rb;

    int maxHealth = 100;
    int currenthealth;

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
