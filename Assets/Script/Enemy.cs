using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator anim;
    protected Collider2D coll;
    protected int direction; //1表示向右或向上，-1表示向左或向下
    public AudioSource deathAFX;

    public LayerMask ground;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        GetEdgeLocation();
    }

    // Update is called once per frame
    // protected virtual void Update()
    // {
    //     Movement();
    //     AnimSwitcher();
    // }

    //控制敌人的运动逻辑，根据方向和速度给刚体施加力，并检测是否到达边界
    protected virtual void Movement()
    {

    }

    //控制敌人的动画切换逻辑，根据刚体的速度和碰撞状态设置动画参数
    protected virtual void AnimSwitcher()
    {

    }

    //获得运动边界的坐标值并销毁空物体
    protected virtual void GetEdgeLocation()
    {

    }

    //执行Death动画
    public void JumpOn()
    {
        anim.SetTrigger("Death");
        deathAFX.Play();
        //把刚体的速度设置为零
        rb.velocity = Vector2.zero;
        //把刚体的类型设置为Kinematic
        rb.bodyType = RigidbodyType2D.Static;
        //把碰撞体设置为不激活
        coll.enabled = false;
    }

    //敌人死亡的逻辑，销毁该敌人对象
    public void Death()
    {
        Destroy(gameObject);
    }
}