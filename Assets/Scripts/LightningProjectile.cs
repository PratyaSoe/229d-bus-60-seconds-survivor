using System;
using UnityEngine;

public class LightningProjectile : MonoBehaviour
{
    [SerializeField] float speed = 18f;                 // �������Ǣͧ����ع
    [SerializeField] int directDamage = 100;           // ����������µ����������µç
    [SerializeField] int splashDamage = 50;            // ����������µ��������·���������
    [SerializeField] float splashRadius = 3f;          // �����㹡����������������§
    [SerializeField] int maxSharedTargets = 4;         // �ӹǹ������·������������٧�ش
    [SerializeField] GameObject lightningEffect;       // �Ϳ࿡����¿����ѧⴹ�ѵ��

    private void FixedUpdate()
    {
        // �������͹���ͧ����ع
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ��Ǩ�ͺ��ҡ���ع���ѵ��
        var hitEnemy = collision.gameObject.GetComponent<Enemy>();
        if (hitEnemy != null)
        {
            // �Ӵ�������ѵ�ٷ��ⴹ����ع�µç
            hitEnemy.Hit(directDamage);

            // ���ҧ�Ϳ࿡����¿�� (�����)
            if (lightningEffect != null)
            {
                Instantiate(lightningEffect, transform.position, Quaternion.identity);
            }

            // ����������Ѻ�ѵ�ٷ���������
            ShareDamage(hitEnemy);

            // ����¡���ع
            Destroy(gameObject);
        }
    }

    private void ShareDamage(Enemy mainTarget)
    {
        // ���� Collider �������ͺ� ���˹觢ͧ�ѵ�ٷ��ⴹ����ع
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(mainTarget.transform.position, splashRadius);

        // ��ǹѺ������·�����Ѻ�������
        int sharedCount = 0;

        foreach (var collider in nearbyColliders)
        {
            if (sharedCount >= maxSharedTargets) break; // �ӡѴ�ӹǹ������·�����Ѻ�����

            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null && enemy != mainTarget) // ����������������ѡ
            {
                enemy.Hit(splashDamage);
                sharedCount++;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // �ʴ�������������� Editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }

    internal void SetOffset(Vector2 pos)
    {
        throw new NotImplementedException();
    }
}
