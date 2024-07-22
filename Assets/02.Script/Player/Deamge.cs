using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Codice.Client.Common.WebApi.WebApiEndpoints;

public class Deamge : MonoBehaviour
{
    private readonly string E_BulletTag = "E_Bullet";
    public GameObject Blood;
    private int hp = 0;
    private int maxhp = 100;
    private bool isDie = false;
    private Rigidbody rb;
    private CapsuleCollider cp;


    void Start()
    {
        hp = maxhp;
        Blood = Resources.Load("Effects/BulletImpactFleshSmallEffect") as GameObject;
        rb = GetComponent<Rigidbody>();
        cp = rb.GetComponent<CapsuleCollider>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(E_BulletTag))
        {
            collision.gameObject.SetActive(false);

            // 맞은 위치 Collision 구조체안에 Contacts라는 배열이 있다.
            GameObject blood = ShowBloodEffect(collision);

            hp -= 10;
            if(hp <= 0)
            {
                Debug.Log("으앙죽음");
                PlayerDie();
            }
        }

    }

    private GameObject ShowBloodEffect(Collision collision)
    {
        Vector3 pos = collision.contacts[0].point; //  위치
        Vector3 _nomal = collision.contacts[0].normal; // 방향
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _nomal);
        GameObject blood = Instantiate(Blood, pos, rot);
        Destroy(blood, 1.0f);
        return blood;
    }

    public void PlayerDie()
    {
        isDie = true;
      
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i< enemies.Length; i++)
        {
            enemies[i].gameObject.SendMessage("PlayerDie",SendMessageOptions.DontRequireReceiver);
        }
    }
}
