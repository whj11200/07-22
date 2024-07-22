using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
// �÷��̰� �Ǿ����� �跲 ������ �����ϰ� �����
// 5�� �跲�� �Ѿ˿� ���� �������� �����
public class BarrelCtrl : MonoBehaviour
{
    [SerializeField]
    private Texture[] textures;
    [SerializeField]
    private MeshRenderer mesh;
    [SerializeField]
    private int HitCount = 0;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private string BulletTag = "Bullet";
    private string E_BulletTag = "E_Bullet";
    [SerializeField]
    private GameObject ExplosionPrefad;
    private AudioClip clip;
    private AudioClip BoomCilp;
    private string CameraTag = "MainCamera";
    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private Mesh[] meshes;

    
 
   

    void Start()
    {
        ExplosionPrefad = Resources.Load("EffectBoom") as GameObject;
        rb = GetComponent<Rigidbody>();
        textures = Resources.LoadAll<Texture>("BarrelTextures");
        // textrues = Resources.LoadAll("")as <Texture>;
        mesh = GetComponent<MeshRenderer>();
        mesh.material.mainTexture = textures[Random.Range(0, textures.Length)];
        clip = Resources.Load("Sound/bullet_hit_metal_enemy_1") as AudioClip;
        BoomCilp = Resources.Load("Sound/grenade_exp2") as AudioClip;
        meshFilter = GetComponent<MeshFilter>();
        meshes = Resources.LoadAll<Mesh>("Meshes");
        
        

    }
    #region ������Ÿ���� ����� �浹����
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(BulletTag) || other.gameObject.CompareTag(E_BulletTag))
        {
            other.gameObject.SetActive(false);
            SoundManger.S_Instance.PlaySound(transform.position, clip);
            if (++HitCount == 5)
            {
                ExplosionBarrel();
                CamerSek.instance.TurnOn();

            }


        }

    }
    #endregion

    void OnDamage(object[] _params)
    {
        SoundManger.S_Instance.PlaySound(transform.position, clip);
        Vector3 firepos = (Vector3)_params[2];
        Vector3 hitpos = (Vector3)_params[0];
        // ������ġ�� ����ġ�� �Ÿ��� ����
        Vector3 incomeVector = hitpos - firepos;
        // �������� �Ի� ���Ͷ�� ��
        incomeVector = incomeVector.normalized; // �Ի纤�͸� ����ȭ ���ͷ� ����
        // Ray �� hit ��ǥ�� �Ի纤���� ������ ���� ����
        GetComponent<Rigidbody>().AddForceAtPosition(incomeVector * 1500f, hitpos);
        // � ������ ���� ��Ƽ�  ���İ� �����ǰ� �Ҷ� ȣ�� �Ǵ� �޼��夤
        if (++HitCount == 5)
        {
            ExplosionBarrel();
            CamerSek.instance.TurnOn();
            

        }
    }
    void ExplosionBarrel()
    {
        
        GameObject Effect = Instantiate(ExplosionPrefad,transform.position, Quaternion.identity);
        Destroy(Effect, 2f);
        Collider[] colls = Physics.OverlapSphere(transform.position, 20f);
        // �跲 �ڱ��ڽ� ��ġ���� 20���濡 �跲 ���̾ Cols ��� �迭�� ��´�.
      
        foreach (Collider coll in colls)
        {
            Rigidbody rigidbody = coll.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
               
                SoundManger.S_Instance.PlaySound(transform.position, BoomCilp);
                rigidbody.mass = 1.0f;
                rigidbody.AddExplosionForce(500, transform.position, 10f, 1000f);
                Destroy(gameObject,2.0f);
               
                
            }
            Invoke("BerralMassOrginal",1f);
         
            
          
            // ������ٵ� Ŭ���� ���� �Լ��� AddExplosionForce(���ķ�,��ġ, �ݰ�, ���� �ڱ�ġ�� ��)�� �ǹ��Ѵ�.
        }
        int ids = Random.Range(0, meshes.Length);
        // �͠����� �޽��ʴ��� sharedMesh�Ͽ� ���������� �޽��ȿ��ִ� mesh���� ����ǰ� ��
        meshFilter.sharedMesh = meshes[ids];
        GetComponent<MeshCollider>().sharedMesh = meshes[ids];
    }
    void BerralMassOrginal()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, 100.0f);
        // �跲 �ڱ��ڽ� ��ġ���� 20���濡 �跲 ���̾ Cols ��� �迭�� ��´�.

        foreach (Collider coll in colls)
        {
            Rigidbody rigidbody = coll.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                
                rigidbody.mass = 60f;
                

            }
           


           
        }

    }
  
}
