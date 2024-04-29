using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    bool AddGunReceived = false; // สถานะของการรับไอเท็ม
    int AddGunCount = 0;

    [SerializeField] GameObject gunPrefab;

    Transform player;
    List<Vector2> gunPositions = new List<Vector2>();

    int spawnedGuns = 0;

    public float timeLimit = 5f; // เวลาที่กำหนดให้

    private float timer = 0f;
    private void Start()
    {
        player = GameObject.Find("Player").transform;
        gunPositions.Add(new Vector2(-1.2f, 1f));
        gunPositions.Add(new Vector2(1.2f, 1f));

        gunPositions.Add(new Vector2(-1.4f, 0.2f));
        gunPositions.Add(new Vector2(1.4f, 0.2f));

        gunPositions.Add(new Vector2(-1f, -0.5f));
        gunPositions.Add(new Vector2(1f, -0.5f));

        AddGun();
        AddGun();

    }
    private void Update()
    {
        if (!AddGunReceived)
        {
            timer += Time.deltaTime; // เพิ่มเวลาที่ผ่านไปเรื่อยๆ

            if (timer >= timeLimit)
            {
                if (AddGunCount != 4)
                {
                    // เมื่อหมดเวลา
                    AddGun();
                    AddGunReceived = true; // เปลี่ยนสถานะของการรับไอเท็ม
                    AddGunCount += 1;
                    timer = 0f; // รีเซ็ตตัวนับเวลา
                    AddGunReceived = false;
                }
                if (AddGunCount == 4)
                {
                    AddGunReceived = false;
                }
            }

        }
        
        //  else
        //   {
        // ผู้เล่นสามารถรับไอเท็มใหม่ได้
        //  if (Input.GetKeyDown(KeyCode.G))
        //   {
        //      AddGunReceived = false; // เปลี่ยนสถานะเพื่อให้ผู้เล่นสามารถรับไอเท็มใหม่ได้
        //  }
        // }

        //timer += Time.deltaTime;

        //if (timer >= timeLimit)
        //{
        //    AddGun();
        // }





        //For test
        //  if (Input.GetKeyDown(KeyCode.G))
        // AddGun();
    }


    void AddGun()
    {
        var pos = gunPositions[spawnedGuns];
        var newGun = Instantiate(gunPrefab, pos, Quaternion.identity);

        newGun.GetComponent<Gun>().SetOffset(pos);
        spawnedGuns++;
    }





}
