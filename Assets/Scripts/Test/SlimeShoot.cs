using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeShoot : MonoBehaviour
{


    [Header("射撃間隔")] public float attackTime;

    public GameObject lazar = null;
    public Transform attackPos;
    private bool canAttack;
    private float currentAttackTime;


    // Start is called before the first frame update
    void Start()
    {
        currentAttackTime = attackTime;
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }


    public void Attack()
    {
        currentAttackTime += Time.deltaTime;

        if (attackTime < currentAttackTime)
        {
            canAttack = true;
        }
        //attackPosに弾丸を生成し、attackPosのローテーションで方向が決まる
        if (canAttack)
        {
            Instantiate(lazar, attackPos.position, attackPos.rotation);
            canAttack = false;
            currentAttackTime = 0f;

        }

    }


}

