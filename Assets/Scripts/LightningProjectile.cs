using System;
using UnityEngine;

public class LightningProjectile : MonoBehaviour
{
    [SerializeField] float speed = 18f;                 // ความเร็วของกระสุน
    [SerializeField] int directDamage = 100;           // ความเสียหายต่อเป้าหมายโดยตรง
    [SerializeField] int splashDamage = 50;            // ความเสียหายต่อเป้าหมายที่แชร์ดาเมจ
    [SerializeField] float splashRadius = 3f;          // รัศมีในการหาเป้าหมายใกล้เคียง
    [SerializeField] int maxSharedTargets = 4;         // จำนวนเป้าหมายที่แชร์ดาเมจได้สูงสุด
    [SerializeField] GameObject lightningEffect;       // เอฟเฟกต์สายฟ้าหลังโดนศัตรู

    private void FixedUpdate()
    {
        // การเคลื่อนที่ของกระสุน
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ตรวจสอบว่ากระสุนชนศัตรู
        var hitEnemy = collision.gameObject.GetComponent<Enemy>();
        if (hitEnemy != null)
        {
            // ทำดาเมจแก่ศัตรูที่โดนกระสุนโดยตรง
            hitEnemy.Hit(directDamage);

            // สร้างเอฟเฟกต์สายฟ้า (ถ้ามี)
            if (lightningEffect != null)
            {
                Instantiate(lightningEffect, transform.position, Quaternion.identity);
            }

            // แชร์ดาเมจให้กับศัตรูที่อยู่ใกล้
            ShareDamage(hitEnemy);

            // ทำลายกระสุน
            Destroy(gameObject);
        }
    }

    private void ShareDamage(Enemy mainTarget)
    {
        // ค้นหา Collider ในรัศมีรอบๆ ตำแหน่งของศัตรูที่โดนกระสุน
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(mainTarget.transform.position, splashRadius);

        // ตัวนับเป้าหมายที่ได้รับแชร์ดาเมจ
        int sharedCount = 0;

        foreach (var collider in nearbyColliders)
        {
            if (sharedCount >= maxSharedTargets) break; // จำกัดจำนวนเป้าหมายที่ได้รับดาเมจ

            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null && enemy != mainTarget) // ห้ามซ้ำเป้าหมายหลัก
            {
                enemy.Hit(splashDamage);
                sharedCount++;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // แสดงรัศมีแชร์ดาเมจใน Editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }

    internal void SetOffset(Vector2 pos)
    {
        throw new NotImplementedException();
    }
}
