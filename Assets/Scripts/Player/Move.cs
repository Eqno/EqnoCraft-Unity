using System;
using UnityEngine;

public class Move: MonoBehaviour
{
    public float MoveSpeed = 4, JumpHeight = 7, Gravity = 20;

    private float pi;
    private Vector3 direction;
    private GameObject _Camera;
    private CharacterController _Player;
    // 开始
    private void Start()
    {
        pi = Mathf.Atan(1);
        direction = Vector3.zero;
        _Camera = GameObject.Find("Camera");
        _Player = GetComponent<CharacterController>();
    }
    // 更新
    private void Update()
    {
        // 如果角色在地面
        if (_Player.isGrounded)
        {
            // 向摄像机所指方向移动
            float rotateX = _Camera.transform.eulerAngles.y;
            direction = transform.TransformDirection(
                new Vector3(
                    Input.GetAxis("Horizontal") * Mathf.Cos(toRad(rotateX)) 
                    + Input.GetAxis("Vertical") * Mathf.Sin(toRad(rotateX)), 
                    0, 
                    Input.GetAxis("Vertical") * Mathf.Cos(toRad(rotateX)) 
                    - Input.GetAxis("Horizontal") * Mathf.Sin(toRad(rotateX)))
                ) * MoveSpeed;
            // 监听角色跳跃
            if (Input.GetButtonDown("Jump")) direction.y = JumpHeight;
        }
        // 自然下落
        direction.y -= Gravity * Time.deltaTime;
        // 角色位移
        _Player.Move(direction * Time.deltaTime);
    }
    // 角度转弧度
    private float toRad(float deg) { return deg / 45 * pi; }
}