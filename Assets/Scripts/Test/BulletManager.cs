using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{

    [Header("射撃速度")] public float shootSpeed;
    [Header("消失時間")] public float destroyTime;
    [Header("空中か")] public bool isAir = true;

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
        //なにかにぶつかると運動を辞める
        if (isAir)
        {
            Move();
        }
    }

    //velocityで弾丸の運動を決める
    public void Move()
    {
        rb.velocity = transform.TransformDirection(new Vector2(shootSpeed, 0f));
    }

    //カメラ外に出るとこのオブジェクトを破壊
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    //ぶつかると運動を停止
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isAir = false;
        Debug.Log("作動");
    }



}
