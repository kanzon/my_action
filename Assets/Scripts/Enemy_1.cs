using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : MonoBehaviour
{

    #region//インスペクター
    [Header("移動速度")] public float speed;
    [Header("画面外でも行動するか")] public bool nonVisible;
    [Header("接触判定")] public EnemyCollisionCheck cheakCollision;
    #endregion

    #region//プライベート
    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private ObjectCollision oc = null;
    private BoxCollider2D col = null;
    private bool rightRleftF = false;
    private bool isDead = false;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        oc = GetComponent<ObjectCollision>();
        col = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!oc.playerStepOn)
        {


            if (sr.isVisible || nonVisible)
            {
                if(cheakCollision.isOn)
                {
                    rightRleftF = !rightRleftF;
                }
                int xVevtor = -1;
                if (rightRleftF)
                {
                    xVevtor = 1;
                    transform.localScale = new Vector3(-1, 1, 1);

                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                rb.velocity = new Vector2(xVevtor * speed, rb.velocity.y);
            }
            else
            {
                rb.Sleep();
            }
        }
        else
        {
            if(!isDead)
            {
                anim.Play("enemy_dead");
                rb.velocity = new Vector2(0, rb.velocity.y);
                isDead = true;
                col.enabled = false;
                Destroy(gameObject, 3f);
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, 5));
            }
        }
    }
}
