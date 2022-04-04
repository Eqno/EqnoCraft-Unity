using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Follow: MonoBehaviour
{ 
    [Header("������뾶")] public float radius = 5;
    [Header("�������ж�")] public float zoomSensitivity = 3;
    [Header("���������")] public float zoomMinDistance = 2;
    [Header("������Զ��")] public float zoomMaxDistance = 10;
    [Header("�������ж�")] public float horiSensitivity = 3;
    [Header("�������ж�")] public float vertSensitivity = 3;
    [Header("������ӽ�")] public float verticalMaxDegree = 90;
    [Header("����ӽ�")] public float verticalMinDegree = -90;

    private Cross _Cross;
    private bool firstPerson;
    private GameObject _Player;
    private float rotateX, rotateY;
    void Start()
    {
        firstPerson = false;
        rotateX = rotateY = 0;
        _Cross = GetComponent<Cross>();
        _Player = GameObject.Find("Player");

        _Cross.enabled = false;
        AdjustCrossLocked();
    }
    void LateUpdate()
    {
        {
            if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
            if (Input.GetKeyDown(KeyCode.V))
            {
                firstPerson = !firstPerson;
                if (!firstPerson)
                {
                    _Cross.enabled = false;
                    AdjustCrossLocked();
                }
            }
            if (firstPerson && Input.GetKeyDown(KeyCode.T))
            {
                _Cross.enabled = !_Cross.enabled;
                AdjustCrossLocked();
            }
        }

        if (_Cross.enabled || !_Cross.enabled && Input.GetKey(KeyCode.Mouse2))
        {
            rotateX += Input.GetAxis("Mouse X") * horiSensitivity;
            rotateY += Input.GetAxis("Mouse Y") * vertSensitivity;
            rotateY = rotateY > verticalMaxDegree ? verticalMaxDegree
                : rotateY < verticalMinDegree ? verticalMinDegree : rotateY;
        }

        transform.position = _Player.transform.position + new Vector3(0, 0.5f, 0);
        transform.localEulerAngles = new Vector3(-rotateY, rotateX, 0);

        if (firstPerson) return;
        radius -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
        if (radius > zoomMaxDistance) radius = zoomMaxDistance;
        if (radius < zoomMinDistance) radius = zoomMinDistance;
        transform.TransformDirection(transform.localEulerAngles);
        transform.Translate(new Vector3(0, 0, -radius));
    }
    private void AdjustCrossLocked()
    {
        if (_Cross.enabled)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
