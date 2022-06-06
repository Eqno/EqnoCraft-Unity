using System;
using UnityEngine;

public class Move: MonoBehaviour
{
    public float MoveSpeed = 4, JumpHeight = 15, Gravity = 45;
    private float jumping = 0;
    private float EPS = 0.001f;
    private Animator animator;
    private Vector3 direction;
    private GameObject _Camera;
    private CharacterController _Player;
    private int IDLE = 0, FORW = 1, LEFT = 2, BACK = 4, RIGH = 8;
    // 开始
    private void Start()
    {
        direction = Vector3.zero;
        animator = GetComponent<Animator>();
        _Camera = GameObject.Find("Camera");
        _Player = GetComponent<CharacterController>();
    }
    // 更新
    private void Update()
    {
        CharacterMove();
    }
    // 角色移动
    private void CharacterMove()
    {
        // 如果角色在地面
        if (_Player.isGrounded)
        {
            if (jumping > 0.5) animator.SetTrigger("Land");
            // 计数器清零
            jumping = 0;
            // 监听 WASD
            Vector3 dir = new Vector3(0, 0, 0);
            dir.z = Input.GetAxis("Vertical");
            dir.x = Input.GetAxis("Horizontal");
            // 移动动作
            if (dir.x<-EPS && dir.z>EPS)
                animator.SetInteger("RunDirection", LEFT | FORW); // 左前
            else if (dir.x>EPS && dir.z>EPS)
                animator.SetInteger("RunDirection", RIGH | FORW); // 右前
            else if (dir.x<-EPS && dir.z<-EPS)
                animator.SetInteger("RunDirection", LEFT | BACK); // 左后
            else if (dir.x>EPS && dir.z<-EPS)
                animator.SetInteger("RunDirection", RIGH | BACK); // 右后
            else if (dir.z > EPS)
                animator.SetInteger("RunDirection", FORW); // 正前
            else if (dir.x < -EPS)
                animator.SetInteger("RunDirection", LEFT); // 正左
            else if (dir.z < -EPS)
                animator.SetInteger("RunDirection", BACK); // 正后
            else if (dir.x > EPS)
                animator.SetInteger("RunDirection", RIGH); // 正右
            else
                animator.SetInteger("RunDirection", IDLE); // 不动
            // 向摄像机所指方向移动
            direction = transform.TransformDirection(dir * MoveSpeed);
            // 监听角色跳跃
            if (Input.GetButtonDown("Jump"))
            {
                direction.y = JumpHeight;
                animator.SetTrigger("Jump");
            }
        }
        // 计数器自增
        else jumping += Time.deltaTime;
        // 判断是否在空中
        if (jumping >= 0.1)
        {
            jumping = 0.1f;
            animator.SetBool("Jumping", true);
        }
        else animator.SetBool("Jumping", false);
        // 自然下落
        direction.y -= Gravity * Time.deltaTime;
        // 角色位移
        _Player.Move(direction * Time.deltaTime);
    }
}