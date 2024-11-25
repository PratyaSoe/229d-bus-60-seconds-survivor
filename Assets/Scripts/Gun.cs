    using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject muzzle;
    [SerializeField] Transform muzzlePosition;
    [SerializeField] GameObject projectile;

    [Header("Config")]
    [SerializeField] float fireDistance = 10;
    [SerializeField] float fireRate = 0.5f;

    Transform player;
    Vector2 offset;

    private float timeSinceLastShot = 0f;
    private int elementIndex = 0;
    Transform closestEnemy;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        timeSinceLastShot = fireRate;
        player = GameObject.Find("Player").transform;

        
    }

    private void Update()
    {
        transform.position = (Vector2)player.position + offset;

        FindClosestEnemy();
        AimAtEnemy();
        Shooting();
        SelectElement();
    }

    private void SelectElement()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            elementIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            elementIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            elementIndex = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            elementIndex = 3;
        }
    }

    void FindClosestEnemy()
    {
        closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance && distance <= fireDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
       
        
        }

    }
    void AimAtEnemy()
    {
        if(closestEnemy != null)
        {
            Vector3 direction = closestEnemy.position - transform.position;
            direction.Normalize();

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, angle);
            transform.position = (Vector2)player.position + offset;
        }
    }
    void Shooting()
    {
        if (closestEnemy == null) return;


        timeSinceLastShot += Time.deltaTime;
        if(timeSinceLastShot >= fireRate)
        {
            Shoot();
            timeSinceLastShot = 0;
        }
    }

    void Shoot()
    {
        anim.SetTrigger("Shoot");

        var muzzleGo = Instantiate(muzzle, muzzlePosition.position, transform.rotation);
        muzzleGo.transform.SetParent(transform);
        Destroy(muzzleGo, 0.05f);

     var projectileGo = Instantiate(projectile, muzzlePosition.position, transform.rotation);

        switch(elementIndex)
        {
            case 0:
                projectileGo.GetComponent<ProjectileGo>().elementType = ElementType.Fire;
                break;

            case 1:
                projectileGo.GetComponent<ProjectileGo>().elementType = ElementType.Water;
                break;

            case 2:
                projectileGo.GetComponent<ProjectileGo>().elementType = ElementType.Ice;
                break;

            case 3:
                projectileGo.GetComponent<ProjectileGo>().elementType = ElementType.Electric;
                break;
        }
        

     Destroy(projectileGo, 3);
    }



    public void SetOffset(Vector2 o)
    {
        offset = o;
    }


}
