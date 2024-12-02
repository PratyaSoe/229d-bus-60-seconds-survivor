using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healAmount = 50; // จำนวนเลือดที่เพิ่ม
    [SerializeField] private float lifetime = 10f; // เวลาที่ไอเท็มจะอยู่ก่อนหายไป

    private void Start()
    {
        // ทำลายไอเท็มหลังจากเวลาที่กำหนด
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ตรวจสอบว่าผู้เล่นชนกับไอเท็ม
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            // เพิ่มเลือดให้ผู้เล่น
            player.Heal(healAmount);

            // ลบไอเท็มฮีล
            Destroy(gameObject);
        }
    }

}