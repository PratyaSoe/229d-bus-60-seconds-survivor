using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    // ใช้สำหรับเก็บ prefab ของกระสุน
    [SerializeField] GameObject ProjectilePrefab;
    [SerializeField] GameObject lightningProjectilePrefab;

    private GameObject currentProjectilePrefab;
    private GunManager gunManager;

    private void Start()
    {
        // ตั้งค่ากระสุนเริ่มต้นเป็นกระสุนปกติ
        currentProjectilePrefab = ProjectilePrefab;

        // หา GunManager ที่เชื่อมต่อกับ Player
        gunManager = GetComponent<GunManager>();
    }

    private void Update()
    {
        // กดปุ่ม 1 เพื่อใช้กระสุนไฟฟ้า
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentProjectilePrefab = lightningProjectilePrefab;
            Debug.Log("Switched to LightningProjectile");
        }

        // กดปุ่ม 0 เพื่อใช้กระสุนปกติ
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentProjectilePrefab = ProjectilePrefab;
            Debug.Log("Switched to Projectile");
        }



       
    }
}