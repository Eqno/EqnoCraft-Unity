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
    // ��ʼ
    private void Start()
    {
        direction = Vector3.zero;
        animator = GetComponent<Animator>();
        _Camera = GameObject.Find("Camera");
        _Player = GetComponent<CharacterController>();
    }
    // ����
    private void Update()
    {
        CharacterMove();
    }
    // ��ɫ�ƶ�
    private void CharacterMove()
    {
        // �����ɫ�ڵ���
        if (_Player.isGrounded)
        {
            if (jumping > 0.5) animator.SetTrigger("Land");
            // ����������
            jumping = 0;
            // ���� WASD
            Vector3 dir = new Vector3(0, 0, 0);
            dir.z = Input.GetAxis("Vertical");
            dir.x = Input.GetAxis("Horizontal");
            // �ƶ�����
            if (dir.x<-EPS && dir.z>EPS)
                animator.SetInteger("RunDirection", LEFT | FORW); // ��ǰ
            else if (dir.x>EPS && dir.z>EPS)
                animator.SetInteger("RunDirection", RIGH | FORW); // ��ǰ
            else if (dir.x<-EPS && dir.z<-EPS)
                animator.SetInteger("RunDirection", LEFT | BACK); // ���
            else if (dir.x>EPS && dir.z<-EPS)
                animator.SetInteger("RunDirection", RIGH | BACK); // �Һ�
            else if (dir.z > EPS)
                animator.SetInteger("RunDirection", FORW); // ��ǰ
            else if (dir.x < -EPS)
                animator.SetInteger("RunDirection", LEFT); // ����
            else if (dir.z < -EPS)
                animator.SetInteger("RunDirection", BACK); // ����
            else if (dir.x > EPS)
                animator.SetInteger("RunDirection", RIGH); // ����
            else
                animator.SetInteger("RunDirection", IDLE); // ����
            // ���������ָ�����ƶ�
            direction = transform.TransformDirection(dir * MoveSpeed);
            // ������ɫ��Ծ
            if (Input.GetButtonDown("Jump"))
            {
                direction.y = JumpHeight;
                animator.SetTrigger("Jump");
            }
        }
        // ����������
        else jumping += Time.deltaTime;
        // �ж��Ƿ��ڿ���
        if (jumping >= 0.1)
        {
            jumping = 0.1f;
            animator.SetBool("Jumping", true);
        }
        else animator.SetBool("Jumping", false);
        // ��Ȼ����
        direction.y -= Gravity * Time.deltaTime;
        // ��ɫλ��
        _Player.Move(direction * Time.deltaTime);
    }
}