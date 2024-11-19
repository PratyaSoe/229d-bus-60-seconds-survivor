using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    // ������Ѻ�� prefab �ͧ����ع
    [SerializeField] GameObject ProjectilePrefab;
    [SerializeField] GameObject lightningProjectilePrefab;

    private GameObject currentProjectilePrefab;
    private GunManager gunManager;

    private void Start()
    {
        // ��駤�ҡ���ع��������繡���ع����
        currentProjectilePrefab = ProjectilePrefab;

        // �� GunManager ����������͡Ѻ Player
        gunManager = GetComponent<GunManager>();
    }

    private void Update()
    {
        // ������ 1 ���������ع俿��
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentProjectilePrefab = lightningProjectilePrefab;
            Debug.Log("Switched to LightningProjectile");
        }

        // ������ 0 ���������ع����
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentProjectilePrefab = ProjectilePrefab;
            Debug.Log("Switched to Projectile");
        }



       
    }
}