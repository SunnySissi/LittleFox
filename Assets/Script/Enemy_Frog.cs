using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private int direction = 1; //1表示向右，-1表示向左
    private float leftX,rightX;
    private Collider2D coll;

    public LayerMask ground;
    public Transform leftPoint,rightPoint;
    public float xSpeed,ySpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        GetEdgeLocation();
    }

    // Update is called once per frame
    void Update()
    {
        //Movement();
        AnimSwitcher();
    }

    void Movement()
    {
        if(coll.IsTouchingLayers(ground))
        {
            anim.SetBool("Jumping", true);
            //根据方向给敌人的刚体施加速度
            rb.velocity = new Vector2(direction * xSpeed, ySpeed);
            transform.localScale = new Vector3(-direction,1,1);
        }
        
        //检测是否到达左边界
        if (transform.position.x <= leftX)
        {
            //改变方向为向右
            direction = 1;
        }

        //检测是否到达右边界
        if (transform.position.x >= rightX)
        {
            //改变方向为向左
            direction = -1;
        }
    }

    //动画切换
    void AnimSwitcher()
    {
        if(anim.GetBool("Jumping"))
        {
            //根据刚体的垂直速度来设置Falling条件
            if(rb.velocity.y < 0.1f)
            {
                anim.SetBool("Jumping", false);
                anim.SetBool("Falling", true);
            }
        }
        if(coll.IsTouchingLayers(ground) && anim.GetBool("Falling"))
        {
            anim.SetBool("Falling", false);
        }
    }

    //获得运动边界的坐标值并销毁空物体
    void GetEdgeLocation()
    {
        leftX = leftPoint.position.x;
        rightX = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }
}
