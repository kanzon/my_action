using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region//�C���X�y�N�^�[�Őݒ肷��
    [Header("�ړ����x")] public float speed;
    [Header("�W�����v����")] public float jumpForce;
    [Header("���~���x")] public float downForce;
    [Header("�W�����v����")] public float jumpLimitTime;
    [Header("�ő卂��")] public float jumpHeight;
    [Header("���݂�����̍����̊���")]public float stepOnRate;
    [Header("�ڒn����")] public GroundCheck ground;
    [Header("���Ԃ�����")] public GroundCheck head;
    
    #endregion

    #region//�v���C�x�[�g�ϐ�
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
        //dC = transform.Find("ReflectCollinder").gameObject; //�q����Ȃ��ăv���C���[�ɂ��Ă��ǂ��H
    }

    private void Update()
    {
        //�W�����v�������ŃW�����v���J��Ԃ��Ȃ�����
        if (Input.GetKeyUp(KeyCode.Space))
        {
            pushSpace = false;
        }

    }
    
    void FixedUpdate()
    {

        if (!isDown) { 

        //�ڒn����𓾂�
        isGround = ground.IsGround();
        isHead = head.IsGround();

        //�e����W���̔���𓾂�
        GetXSpeed();
        Jump();
        //����
        //Deflect();

        SetAnimation();

        }       
    }

    /// <summary>
    /// �W�����v�̌v�Z
    /// </summary>
    /// <returns></returns>
    private void Jump()
    {
        float a = this.transform.position.y;


        //�G�𓥂񂾂Ƃ��̃W�����v
        if (isOtherJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, otherJumpHeight);
            jumpTime += Time.deltaTime;
            isOtherJump = false;   

        }
        //�n�ʂ̏�ŃW�����v���鎞
        else if (isGround)
        {
            if (Input.GetKey(KeyCode.Space) && !pushSpace)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);                
                jumpTime += Time.deltaTime;
                isJump = true;
                pushSpace = !pushSpace;
            }
        }//�������ɂ��W�����v�����̒��߂��ł���
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
    /// ���E�ړ��̌v�Z
    /// </summary>
    /// <returns>X�̑���</returns>
    private void GetXSpeed()
    {
        float x = Input.GetAxis("Horizontal");
        

        if (x != 0)
        {
            rb.velocity = new Vector2(speed * x, rb.velocity.y);
            isRun = true;
        }

        //�ړ������ɂ���ĕ����]��
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

    //�A�j���[�V�����������\�b�h
    private void SetAnimation()
    {
        anim.SetBool("jump", isJump || isOtherJump);
        anim.SetBool("ground", isGround);
        anim.SetBool("run", isRun);
    }
    #region//�G�ڐG����
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.collider.tag == enemyTag)
        {
            foreach(ContactPoint2D p in collision.contacts)
            {
                //���݂�����ɂȂ鍂��
                float stepOnHeight = (capsul.size.y * (stepOnRate / 80f));

                //���݂�����̃��[���h���W
                float judgePos = transform.position.y - (capsul.size.y / 2f) + stepOnHeight;
                { 

                  if(p.point.y < judgePos)
                  {
                        //�Ռ����肩��擾
                      ObjectCollision o = collision.gameObject.GetComponent<ObjectCollision>();
                        if(o != null)
                        {
                            //�Ռ����肩��o�E���h�������擾
                            otherJumpHeight = o.boundHeight;
                            o.playerStepOn = true;
                            isOtherJump = true;
                            jumpTime = 0.0f;
                            isJump = true;

                        }
                  }
                  else
                  {
                        //�ォ��ׂ��Ȃ��Ǝ���
                        anim.Play("player_down");                       
                        isDown = true;
                        break;
                  }

                }

            }

        }
    }
    #endregion
    
    /*//���˔���o��
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

    #region//���˂܂Ƃ�
    /// <summary>
    /// �g���K�[�ɓ���Ƃ��A�^�t�ɔ��˂���B
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            GameObject bullet = collision.gameObject;
            BulletManager bulletmanager =  bullet.GetComponent<BulletManager>();
            bulletmanager.shootSpeed = -bulletmanager.shootSpeed;
            Debug.Log("�쓮��");
        }
    }
    /// <summary>
    /// �g���K�[���ɑ��݂��鎞�A�^�t�ɔ��˂���B
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        { 
         GameObject bullet = collision.gameObject;
        BulletManager bulletmanager = bullet.GetComponent<BulletManager>();
        bulletmanager.shootSpeed = -bulletmanager.shootSpeed;
        Debug.Log("�쓮��");
        }
    }
    #endregion*/
    
}
