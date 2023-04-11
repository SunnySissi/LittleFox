using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : Enemy
{
    private float upX, downX;

    public Transform upPoint, downPoint;
    public float ySpeed;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        direction = 1; //1表示向上，-1表示向下
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        //AnimSwitcher();
    }

    protected override void Movement()
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
    protected override void AnimSwitcher()
    {

    }

    //获得运动边界的坐标值并销毁空物体
    protected override void GetEdgeLocation()
    {
        upX = upPoint.position.y;
        downX = downPoint.position.y;
        Destroy(upPoint.gameObject);
        Destroy(downPoint.gameObject);
    }
}
