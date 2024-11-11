using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] float speed = 2f;
    [SerializeField] int expDropAmount = 50;

    private int currentHealth;

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
        anim.SetTrigger("hit");

        if (currentHealth <= 0)
        {
            DropExp();
            Destroy(gameObject);
        }
    }

    //NEW �ѧ��ѹ�ѻവ���ʶҹ��ѵ�ٵ����Ǥٳ������Ѻ
    public void UpdateStats(float healthMultiplier, float speedMultiplier)
    {
        maxHealth = Mathf.RoundToInt(maxHealth * healthMultiplier);
        currentHealth = maxHealth; // ��駤�� currentHealth ���������ҡѺ maxHealth ����������
        speed *= speedMultiplier;
    }
    //NEW
    private void DropExp()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.AddExp(expDropAmount); // เพิ่ม EXP ให้ผู้เล่น
            Debug.Log("Player gained " + expDropAmount + " EXP");
        }
    }
}
