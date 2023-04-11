using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    // 弹开力度
    public float bounceForce; 
    
    // 这个常量用来控制角色可以跳跃的最大次数
    public const int maxJumpCount = 2;
    public LayerMask ground;
    // 定义一个UI组件，用来显示cherry的数量
    public Text cherryText;
    // 定义一个UI组件，用来显示gem的数量
    public Text gemText;

    public Collider2D playerColl;
    public AudioSource jumpAFX;
    public AudioSource collectAFX;
    public AudioSource hurtAFX;


    
    private Rigidbody2D rb;
    private Animator animator;
    // 是否受伤
    private bool isHurt; 
    //是否在地面
    private bool isGrounded;
    // // 这个变量用来获取角色的碰撞器组件
    // private Collider2D playerColl;
    // 定义一个静态变量，用来存储cherry的数量
    private static int cherryCount = 0;
    // 定义一个静态变量，用来存储gem的数量
    private static int gemCount = 0;

    // 这个属性用来存储角色可以跳跃的次数，并检查是否超过了最大次数
    private int jumpCount
    {
        get { return _jumpCount; }
        set { _jumpCount = Mathf.Clamp(value, 0, maxJumpCount); }
    }
    private int _jumpCount;

    // 定义一些常量，用来代替魔法字符串
    private const string IDLING = "Idling";
    private const string RUNNING = "Running";
    private const string JUMPING = "Jumping";
    private const string FALLING = "Falling";
    private const string HURTING = "Hurting";

    // Start is called before the first frame update
    void Start()
    {
        // 用GetComponent方法获取角色身上的刚体组件，并赋值给rb变量
        rb = GetComponent<Rigidbody2D>();
        // 用GetComponent方法获取角色身上的动画组件，并赋值给animator变量
        animator = GetComponent<Animator>();
        // // 获取角色的碰撞器组件并赋值给playerColl变量
        // playerColl = GetComponent<Collider2D>();
        // 把jumpCount变量初始化为maxJumpCount的值
        jumpCount = maxJumpCount;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        AnimationSwitcher();
    }
    
    void Update()
    {
        Jump();
        //OnGUI();
    }

    //角色移动
    void Movement()
    {
        if(!isHurt)
        {
            //水平移动
            // 获取水平方向的输入值，-1表示左，1表示右，0表示没有输入
            float horizontalInput = Input.GetAxis("Horizontal");
            // 获取水平方向的输入值，-1表示左，1表示右，0或者接近0表示没有输入
            float faceDirection = Input.GetAxisRaw("Horizontal");

            if(horizontalInput != 0)
            {
                // 根据输入值和速度计算出角色在水平方向上的速度向量，并乘以deltaTime来保证和帧率无关，并赋值给刚体组件的velocity属性，让角色移动
                rb.velocity = new Vector2(horizontalInput * speed * Time.deltaTime,rb.velocity.y);
                //设置跑动动画的判断条件
                SetAnimatorBool(RUNNING,true);
            }

            //转向
            if(faceDirection != 0)
            {
                transform.localScale = new Vector3(faceDirection,1,1);
            }
        }
    }

    //跳跃功能
    void Jump()
    {
        //跳跃
        if(Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpAFX.Play();
            //rb.AddForce(Vector2.up * jumpForce);
            rb.velocity = new Vector2(rb.velocity.x,jumpForce * Time.fixedDeltaTime);
            SetAnimatorBool(JUMPING,true);
            jumpCount--;
        }
    }
        
    // 定义一个封装的方法，用来设置动画组件的布尔值
    private void SetAnimatorBool(string name, bool value)
    {
        animator.SetBool(name, value);
    }

    //动画切换
    void AnimationSwitcher()
    {
        // SetAnimatorBool(IDLING,false);
        // SetAnimatorBool(FALLING,false);

        //空中判断
        //注意：需要在Animator窗口内设置AnyState -> Falling的动画转换
        if(rb.velocity.y < 0.1f && !isGrounded)
        {
            SetAnimatorBool(FALLING,true);
        }

        //水平移动判断
        if(animator.GetBool(RUNNING))
        {
            if(Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                SetAnimatorBool(RUNNING,false);
                SetAnimatorBool(IDLING,true);
            }
            else
            {
                SetAnimatorBool(IDLING,false);
                SetAnimatorBool(RUNNING,true);
            }
        }
        

        //垂直移动判断
        if(animator.GetBool(JUMPING))
        {
            if(rb.velocity.y < -0.1f)
            {
                SetAnimatorBool(FALLING,true);
                SetAnimatorBool(JUMPING,false);
            }
            else if(rb.velocity.y > 0.1f)
            {
                SetAnimatorBool(IDLING,false);
                SetAnimatorBool(JUMPING,true);
            } 
        }

        //落地判断
        isGrounded = playerColl.IsTouchingLayers(ground);
        if(isGrounded)
        {
            jumpCount = maxJumpCount;
            SetAnimatorBool(FALLING,false);
            //SetAnimatorBool(IDLING,true);
        }

        //受伤判断
        if(isHurt)
        {
            hurtAFX.Play();
            SetAnimatorBool(HURTING,true);
            if(Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                isHurt = false;
                SetAnimatorBool(HURTING,false);
            }
        }
    }

    // 定义一个OnTriggerEnter2D方法，用来处理触发事件
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 判断触发器的标签是否为Collection
        if (collision.tag == "Collection")
        {
            collectAFX.Play();
            // 判断收集物的名称是否为cherry
            if (collision.name == "cherry")
            {
            // 把cherryCount变量加一
            cherryCount++;
            // 把cherryText组件的文本设置成cherryCount变量的值
            cherryText.text = cherryCount.ToString();
            // 执行你想要的效果，例如播放音效等
            // 例如：
            // AudioManager.instance.PlaySound("Cherry");
            }
            // 判断收集物的名称是否为gem
            else if (collision.name == "gem")
            {
            // 把gemCount变量加一
            gemCount++;
            // 把gemText组件的文本设置成gemCount变量的值
            gemText.text = gemCount.ToString();
            // 执行你想要的效果，例如播放音效等
            // 例如：
            // AudioManager.instance.PlaySound("Gem");
            }
            
            // 销毁物体
            Destroy(collision.gameObject);
        }
    }

    //玩家跳跃可以消灭敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //判断碰撞对象的标签是否为Enemy
        if(collision.gameObject.tag == "Enemy")
        {
            //获取敌人组件
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            //检测是否处于下降状态
            if(animator.GetBool(FALLING))
            {
                // 销毁物体
                enemy.JumpOn();
                rb.velocity = new Vector2(rb.velocity.x,jumpForce * Time.fixedDeltaTime);
                SetAnimatorBool(JUMPING,true);
            }
            //如果不是下降状态
            //使用三目运算符简化代码
            else 
            {
                float x1 = transform.position.x;
                float x2 = collision.gameObject.transform.position.x;
                //给玩家的刚体施加一个反方向的速度
                rb.velocity = new Vector2((x1 < x2 ? -1 : 1) * bounceForce, rb.velocity.y);
                //设置一个标志，表示玩家正在弹开
                isHurt = true;
            }
        }
    }

    //测试用
    // void OnGUI()
    // {
    //     GUI.Label(new Rect(10, 10, 100, 20), animator.GetBool(FALLING).ToString());
    //     GUI.Label(new Rect(10, 30, 100, 20), animator.GetBool(IDLING).ToString());
    //     GUI.Label(new Rect(10, 50, 100, 20), rb.velocity.x.ToString());
    //     GUI.Label(new Rect(10, 70, 100, 20), rb.velocity.y.ToString());
    // }
    
}