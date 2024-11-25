using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    bool AddGunReceived = false; // ʶҹТͧ����Ѻ�����
    int AddGunCount = 0;

    [SerializeField] GameObject gunPrefab;
    

    Transform player;
    List<Vector2> gunPositions = new List<Vector2>();

    int spawnedGuns = 0;

    //public float timeLimit = 5f; // ���ҷ���˹����

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
        //AddGun();
        
    }
    private void Update()
    {
        

        if (!AddGunReceived)
        {
            //timer += Time.deltaTime; // �������ҷ���ҹ��������

            //if (timer >= timeLimit)
            //{
            //if (AddGunCount != 4)
            //{
            // ������������
            //AddGun();
            //AddGunReceived = true; // ����¹ʶҹТͧ����Ѻ�����
            //AddGunCount += 1;
            //timer = 0f; // ���絵�ǹѺ����
            //AddGunReceived = false;
            //}
            //if (AddGunCount == 4)
            //{
            //AddGunReceived = false;
            //}
            //}

        }

        //else
        //   {
        // ����������ö�Ѻ�����������
        //  if (Input.GetKeyDown(KeyCode.G))
        //   {
        //      AddGunReceived = false; // ����¹ʶҹ�����������������ö�Ѻ�����������
        //  }
        // }

        //timer += Time.deltaTime;

        //if (timer >= timeLimit)
        //{
        //    AddGun();
        // }





        //For test
        if (Input.GetKeyDown(KeyCode.G))
            AddGun();



    }

    
    void AddGun()
    {
        var pos = gunPositions[spawnedGuns];
        var newGun = Instantiate(gunPrefab, pos, Quaternion.identity);

        newGun.GetComponent<Gun>().SetOffset(pos);
        spawnedGuns++;
    }




}