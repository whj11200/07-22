using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Codice.Client.Common.EventTracking.TrackFeatureUseEvent.Features.DesktopGUI.Filters;
// ��Ʃ����Ʈ public ����� ��� �ʵ带
[System.Serializable]
public class Playeranimation // �ν��Ͻ� â�� ���� �ش�.
{
    public AnimationClip idle;
    public AnimationClip runForward;
    public AnimationClip runBackward;
    public AnimationClip runLeft;
    public AnimationClip runRight;
    public AnimationClip Sprint;

}

public class Player : MonoBehaviour
{
    public Playeranimation Playeranimation;
    [SerializeField]
    private float movespeed = 5f;
    [SerializeField]
    private float rotspeed = 250f;
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    CapsuleCollider capsule;
    [SerializeField]
    Transform tr;
    [SerializeField]
    Animation _animation;
    float h, v, x;
    [SerializeField]
    private Transform firepos;
    public ParticleSystem muzzle;
    private AudioSource source;
    public AudioClip clip;
    [SerializeField]
    private GameObject A4A1;
    [SerializeField]
    private GameObject ShotGun;
    [SerializeField]
    private bool DontFire=false;
    private string E_Bullet = "E_Bullet";
    private readonly string EnemyTag = "Enemy";
    private readonly string BarrelTag = "Barrel";
    private readonly string WallTag = "Wall";
   

    void Start()
    {
        // ���۳�Ʈ ĳ�� ó��
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();  
        _animation = GetComponent<Animation>();
        tr = GetComponent<Transform>();
        _animation.Play(Playeranimation.idle.name);
        firepos = GameObject.Find("fire").transform;

        clip = Resources.Load("Sound/p_m4_1") as AudioClip;   
        source = GetComponent<AudioSource>();
        muzzle.Stop();
      
    }


    void Update()
    {
        Debug.DrawLine(firepos.position, firepos.forward * 100f, Color.red);
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        x = Input.GetAxisRaw("Mouse X");
        Vector3 moveDir = (h * Vector3.right) + (v * Vector3.forward);
        tr.Translate(moveDir.normalized * movespeed * Time.deltaTime, Space.Self);
        MoveAnimation();
        Runing();
        tr.Rotate(Vector3.up * x * Time.deltaTime * rotspeed);
        if (DontFire == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                muzzle.Play();
                Invoke("turnOFF",2f);
                Fire();
                
            }
        }
      
        

    }

    private void Fire()
    {
        #region projectctile movement ���

        //var _bullet = ObjectPullingManger.pullingManger.GetBulletPool();
        //if (_bullet != null)
        //{
        //    _bullet.transform.position = firepos.position;
        //    _bullet.transform.rotation = firepos.rotation; 
        //    _bullet.SetActive(true);
        //}
        //

        ////var firebullet = Instantiate(bullet, firepos.position, firepos.rotation);
        #endregion
        RaycastHit hit;// ������ ������Ʈ�� �浹�����̳�
                       // �Ÿ����� �˷��ִ� ���� ����ü
                       // ������ �i���� �ش���ġ  �ش� ���� ����� 
        if (Physics.Raycast(firepos.position, firepos.forward, out hit, 15f))
        {
            if (hit.collider.CompareTag(EnemyTag)|| hit.collider.CompareTag(BarrelTag)||hit.collider.CompareTag(WallTag))
            {
                Debug.Log("hit");
                object[] _parms = new object[3];
                _parms[0] = hit.point; // ù��° �迭�� ���� ��ġ�� ����,������ġ
                _parms[1] = 25f;//��������
                _parms[2] = firepos.position;// �߻���ġ
               

                // ������ ���� ������Ʈ�� �Լ��� ȣ���ϸ鼭 �Ű����� ���� ����
                hit.collider.gameObject.SendMessage("OnDamage", _parms,SendMessageOptions.DontRequireReceiver);
            }

    
            
          
        }
        source.PlayOneShot(clip, 1.0f);

    }

    private void Runing()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) )
        {
            movespeed = 10f;
            _animation.CrossFade(Playeranimation.Sprint.name, 0.3f);
            DontFire = true;
            muzzle.Stop();
        }
        else if (Input.GetKey(KeyCode.D)&& Input.GetKey(KeyCode.A)&& Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))
        {
            movespeed = 10f;
            muzzle.Stop();
            DontFire = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            movespeed = 5f;
            DontFire = false;
            muzzle.Stop();
        }
    }

    private void MoveAnimation()
    {
        if (h > 0.1f)
        {
            _animation.CrossFade(Playeranimation.runRight.name, 0.3f);
            // ���� ���� Ŭ���� �� ���� ����Ŭ�� �ִϸ��̼� 0.3�ʰ� ȥ���ϸ� �ε巯�� �ִϸ��̼��� ���´�.

        }
        else if (h < -0.1f)
        {
            _animation.CrossFade(Playeranimation.runLeft.name, 0.3f);
        }
        else if (v > 0.1)
        {
            _animation.CrossFade(Playeranimation.runForward.name, 0.3f);
        }
        else if (v < -0.1)
        {
            _animation.CrossFade(Playeranimation.runBackward.name, 0.3f);
        }
        else if (h == 0 && v == 0)
        {
            _animation.CrossFade(Playeranimation.idle.name);
        }
    }
    void turnOFF()
    {
        muzzle.Stop();
    }
    
}
