using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private float upX, downX;
    private Collider2D coll;
    private int direction = 1; //1表示向上，-1表示向下

    public Transform upPoint, downPoint;
    public float ySpeed;

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
        Movement();
        //AnimSwitcher();
    }

    void Movement()
    {


        //根据方向给敌人的刚体施加速度
        rb.velocity = new Vector2(rb.velocity.x, direction * ySpeed);

        //检测是否到达左边界
        if (transform.position.y <= downX)
        {
            //改变方向为向上
            direction = 1;
        }

        //检测是否到达右边界
        if (transform.position.y >= upX)
        {
            //改变方向为向下
            direction = -1;
        }
    }

    //动画切换
    void AnimSwitcher()
    {

    }

    //获得运动边界的坐标值并销毁空物体
    void GetEdgeLocation()
    {
        upX = upPoint.position.y;
        downX = downPoint.position.y;
        Destroy(upPoint.gameObject);
        Destroy(downPoint.gameObject);
    }
}
