using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{

    [Header("ËŒ‚‘¬“x")] public float shootSpeed;
    [Header("Á¸ŠÔ")] public float destroyTime;
    [Header("‹ó’†‚©")] public bool isAir = true;

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
        //‚È‚É‚©‚É‚Ô‚Â‚©‚é‚Æ‰^“®‚ğ«‚ß‚é
        if (isAir)
        {
            Move();
        }
    }

    //velocity‚Å’eŠÛ‚Ì‰^“®‚ğŒˆ‚ß‚é
    public void Move()
    {
        rb.velocity = transform.TransformDirection(new Vector2(shootSpeed, 0f));
    }

    //ƒJƒƒ‰ŠO‚Éo‚é‚Æ‚±‚ÌƒIƒuƒWƒFƒNƒg‚ğ”j‰ó
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    //‚Ô‚Â‚©‚é‚Æ‰^“®‚ğ’â~
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isAir = false;
        Debug.Log("ì“®");
    }



}
