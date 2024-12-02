using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healAmount = 50; // �ӹǹ���ʹ�������
    [SerializeField] private float lifetime = 10f; // ���ҷ��������������͹����

    private void Start()
    {
        // ������������ѧ�ҡ���ҷ���˹�
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ��Ǩ�ͺ��Ҽ����蹪��Ѻ�����
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            // �������ʹ��������
            player.Heal(healAmount);

            // ź��������
            Destroy(gameObject);
        }
    }

}