using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectManager : MonoBehaviour
{

    private GameObject dC = null;

    [System.Serializable]
    public class Bounds
    {
        public float xMin, xMax, yMin, yMax;
    }
    [SerializeField] Bounds bounds;
    [SerializeField, Range(0f, 1f)] private float followStrength; 

    // Start is called before the first frame update
    void Start()
    {
        dC = transform.Find("ReflectTrigger").gameObject;   //triggerstay�̕������C������ bool�����A���˔�������߂�
    }

    // Update is called once per frame
    void Update()
    {
        Deflect();
        CollinderMove();

    }

    private void Deflect()
    {
        if (Input.GetMouseButton(1))
        {
            dC.SetActive(true);
        }
        else
        {
            dC.SetActive(false);
        }

    }

    #region//���˂܂Ƃ�
    /// <summary>
    /// �g���K�[�ɓ���Ƃ��A�^�t�ɔ��˂���B
    /// </summary>
    /// <param ></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            GameObject bullet = collision.gameObject;
            int age = Random.Range(90, 270);
            bullet.transform.Rotate(0,0,age);
        }
    }
    /// <summary>
    /// �g���K�[���ɑ��݂��鎞�A�^�t�ɔ��˂���B
    /// </summary>
    /// <param name="collision"></param>
    /* private void OnTriggerStay2D(Collider2D collision)
     {
         if (collision.tag == "Bullet")
         {
             GameObject bullet = collision.gameObject;
             int age = Random.Range(100,250);
             bullet.transform.Rotate(0, 0, age);
             /*BulletManager bulletmanager = bullet.GetComponent<BulletManager>();
             bulletmanager.shootSpeed = -bulletmanager.shootSpeed;
             Debug.Log("�쓮��");
}
    }*/
    #endregion

    //���˃R�����_�[�̈ړ�
    private void CollinderMove()
    {
        var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        targetPos.x = Mathf.Clamp(targetPos.x, bounds.xMin, bounds.xMax);
        targetPos.y = Mathf.Clamp(targetPos.y, bounds.yMin, bounds.yMax);

        targetPos.z = 0f;

        dC.transform.position = Vector3.Lerp(transform.position, targetPos, followStrength);
    }
}