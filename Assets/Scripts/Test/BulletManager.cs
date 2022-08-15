using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{

    [Header("�ˌ����x")] public float shootSpeed;
    [Header("��������")] public float destroyTime;
    [Header("�󒆂�")] public bool isAir = true;

    private Rigidbody2D rb = null;
    private float currentTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (destroyTime < currentTime)
        {
            Destroy(gameObject);
        }
        //�Ȃɂ��ɂԂ���Ɖ^�������߂�
        if (isAir)
        {
            Move();
        }
    }

    //velocity�Œe�ۂ̉^�������߂�
    public void Move()
    {
        rb.velocity = transform.TransformDirection(new Vector2(shootSpeed, 0f));
    }

    //�J�����O�ɏo��Ƃ��̃I�u�W�F�N�g��j��
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    //�Ԃ���Ɖ^�����~
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isAir = false;
        Debug.Log("�쓮");
    }



}
