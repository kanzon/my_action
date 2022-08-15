using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region//インスペクターで設定する
    [Header("移動速度")] public float speed;
    [Header("ジャンプ高さ")] public float jumpForce;
    [Header("下降速度")] public float downForce;
    [Header("ジャンプ時間")] public float jumpLimitTime;
    [Header("最大高さ")] public float jumpHeight;
    [Header("踏みつけ判定の高さの割合")]public float stepOnRate;
    [Header("接地判定")] public GroundCheck ground;
    [Header("頭ぶつけ判定")] public GroundCheck head;
    
    #endregion

    #region//プライベート変数
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private CapsuleCollider2D capsul = null;
    //private GameObject dC= null;
    private bool pushSpace = false;
    private bool isGround = false;
    private bool isHead = false;
    private bool isJump = false;
    private bool isRun = false;
    private bool isDown = false;
    private bool decide = false;
    private bool isOtherJump = false;
    private float otherJumpHeight = 0.0f;
    private float jumpTime = 0.0f;
    private string enemyTag = "Enemy";
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        capsul = GetComponent<CapsuleCollider2D>();
        //dC = transform.Find("ReflectCollinder").gameObject; //子じゃなくてプレイヤーにつけても良き？
    }

    private void Update()
    {
        //ジャンプ長押しでジャンプを繰り返さないため
        if (Input.GetKeyUp(KeyCode.Space))
        {
            pushSpace = false;
        }

    }
    
    void FixedUpdate()
    {

        if (!isDown) { 

        //接地判定を得る
        isGround = ground.IsGround();
        isHead = head.IsGround();

        //各種座標軸の判定を得る
        GetXSpeed();
        Jump();
        //反射
        //Deflect();

        SetAnimation();

        }       
    }

    /// <summary>
    /// ジャンプの計算
    /// </summary>
    /// <returns></returns>
    private void Jump()
    {
        float a = this.transform.position.y;


        //敵を踏んだときのジャンプ
        if (isOtherJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, otherJumpHeight);
            jumpTime += Time.deltaTime;
            isOtherJump = false;   

        }
        //地面の上でジャンプする時
        else if (isGround)
        {
            if (Input.GetKey(KeyCode.Space) && !pushSpace)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);                
                jumpTime += Time.deltaTime;
                isJump = true;
                pushSpace = !pushSpace;
            }
        }//長押しによるジャンプ高さの調節ができる
        else if(isJump)
        {
            if (Input.GetKey(KeyCode.Space) && jumpLimitTime > jumpTime)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTime += Time.deltaTime;
            }
            else
            {
                isJump = false;
                jumpTime = 0f;
            }
        }
    }

    /// <summary>
    /// 左右移動の計算
    /// </summary>
    /// <returns>Xの速さ</returns>
    private void GetXSpeed()
    {
        float x = Input.GetAxis("Horizontal");
        

        if (x != 0)
        {
            rb.velocity = new Vector2(speed * x, rb.velocity.y);
            isRun = true;
        }

        //移動向きによって方向転換
        if ( x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            
        }
        else if(x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            isRun = false;
        }
    }

    //アニメーション条件メソッド
    private void SetAnimation()
    {
        anim.SetBool("jump", isJump || isOtherJump);
        anim.SetBool("ground", isGround);
        anim.SetBool("run", isRun);
    }
    #region//敵接触判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.collider.tag == enemyTag)
        {
            foreach(ContactPoint2D p in collision.contacts)
            {
                //踏みつけ判定になる高さ
                float stepOnHeight = (capsul.size.y * (stepOnRate / 80f));

                //踏みつけ判定のワールド座標
                float judgePos = transform.position.y - (capsul.size.y / 2f) + stepOnHeight;
                { 

                  if(p.point.y < judgePos)
                  {
                        //衝撃相手から取得
                      ObjectCollision o = collision.gameObject.GetComponent<ObjectCollision>();
                        if(o != null)
                        {
                            //衝撃相手からバウンド高さを取得
                            otherJumpHeight = o.boundHeight;
                            o.playerStepOn = true;
                            isOtherJump = true;
                            jumpTime = 0.0f;
                            isJump = true;

                        }
                  }
                  else
                  {
                        //上から潰さないと死ぬ
                        anim.Play("player_down");                       
                        isDown = true;
                        break;
                  }

                }

            }

        }
    }
    #endregion
    
    /*//反射判定出現
    private void Deflect()
    {
        if(Input.GetMouseButton(1))
        {
            dC.SetActive(true);
        }
        else
        {
            dC.SetActive(false);
        }

    }

    #region//反射まとめ
    /// <summary>
    /// トリガーに入るとき、真逆に反射する。
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            GameObject bullet = collision.gameObject;
            BulletManager bulletmanager =  bullet.GetComponent<BulletManager>();
            bulletmanager.shootSpeed = -bulletmanager.shootSpeed;
            Debug.Log("作動中");
        }
    }
    /// <summary>
    /// トリガー中に存在する時、真逆に反射する。
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        { 
         GameObject bullet = collision.gameObject;
        BulletManager bulletmanager = bullet.GetComponent<BulletManager>();
        bulletmanager.shootSpeed = -bulletmanager.shootSpeed;
        Debug.Log("作動中");
        }
    }
    #endregion*/
    
}
