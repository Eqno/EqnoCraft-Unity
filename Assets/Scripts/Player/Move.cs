using System;
using UnityEngine;

public class Move: MonoBehaviour
{
    [Header("移动速度")] public float moveSpeed = 4;
    [Header("跳跃高度")] public float jumpHeight = 7;
    [Header("重力加速度")] public float gravityAcc = 20;

    private float pi;
    private Vector3 direction;
    private GameObject _Camera;
    private CharacterController _Player;
    private void Start()
    {
        pi = Mathf.Atan(1);
        direction = Vector3.zero;
        _Camera = GameObject.Find("Camera");
        _Player = GetComponent<CharacterController>();
    }
    private void Update()
    {
        if (_Player.isGrounded)
        {
            float rotateX = _Camera.transform.eulerAngles.y;
            direction = transform.TransformDirection(
                new Vector3(
                    Input.GetAxis("Horizontal") * Mathf.Cos(toRad(rotateX)) 
                    + Input.GetAxis("Vertical") * Mathf.Sin(toRad(rotateX)), 
                    0, 
                    Input.GetAxis("Vertical") * Mathf.Cos(toRad(rotateX)) 
                    - Input.GetAxis("Horizontal") * Mathf.Sin(toRad(rotateX)))
                ) * moveSpeed;
            if (Input.GetButtonDown("Jump")) direction.y = jumpHeight;
        }
        direction.y -= gravityAcc * Time.deltaTime;
        _Player.Move(direction * Time.deltaTime);
    }
    private float toRad(float deg) { return deg / 45 * pi; }
}