using System;
using UnityEngine;

public class Move: MonoBehaviour
{
    public float MoveSpeed = 4, JumpHeight = 7, Gravity = 20;

    private float pi;
    private Vector3 direction;
    private GameObject _Camera;
    private CharacterController _Player;
    // ��ʼ
    private void Start()
    {
        pi = Mathf.Atan(1);
        direction = Vector3.zero;
        _Camera = GameObject.Find("Camera");
        _Player = GetComponent<CharacterController>();
    }
    // ����
    private void Update()
    {
        // �����ɫ�ڵ���
        if (_Player.isGrounded)
        {
            // ���������ָ�����ƶ�
            float rotateX = _Camera.transform.eulerAngles.y;
            direction = transform.TransformDirection(
                new Vector3(
                    Input.GetAxis("Horizontal") * Mathf.Cos(toRad(rotateX)) 
                    + Input.GetAxis("Vertical") * Mathf.Sin(toRad(rotateX)), 
                    0, 
                    Input.GetAxis("Vertical") * Mathf.Cos(toRad(rotateX)) 
                    - Input.GetAxis("Horizontal") * Mathf.Sin(toRad(rotateX)))
                ) * MoveSpeed;
            // ������ɫ��Ծ
            if (Input.GetButtonDown("Jump")) direction.y = JumpHeight;
        }
        // ��Ȼ����
        direction.y -= Gravity * Time.deltaTime;
        // ��ɫλ��
        _Player.Move(direction * Time.deltaTime);
    }
    // �Ƕ�ת����
    private float toRad(float deg) { return deg / 45 * pi; }
}