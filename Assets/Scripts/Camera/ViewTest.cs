using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewTest : MonoBehaviour
{
    public float EPS = 0.01f;
    public float CameraRadius = 5;
    public float ZoomSensitivity = 3;
    public float MinZoomDistance = 2;
    public float MaxZoomDistance = 10;
    public float MinVerticalDegree = -90;
    public float MaxVerticalDegree = 90;
    public float VerticalSensitivity = 3;
    public float HorizentalSensitivity = 3;
    private Camera _Camera;
    private GameObject _Player;
    private static float pi = 4 * Mathf.Atan(1);
    private Vector3 cameraRotate, _cameraRadius, playerHeight;
    void Start()
    {
        Initialize();
    }
    void LateUpdate()
    {
        ListenInput();
        UpdateView();
    }
    private void Initialize()
    {
        _Camera = GetComponent<Camera>();
        _Player = GameObject.Find("Player");
        cameraRotate = new Vector3(0, 0, 0);
        playerHeight = new Vector3(0, 0.8f, 0);
        _cameraRadius = new Vector3(0, 0, -CameraRadius);
    }
    private void LimiteValue(ref float value, float max, float min)
    {
        if (value > max) value = max;
        if (value < min) value = min;
    }
    private void ListenInput()
    {
        // 监听摄像机角度
        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))
        {
            cameraRotate.x -= Input.GetAxis("Mouse Y") * VerticalSensitivity;
            cameraRotate.y += Input.GetAxis("Mouse X") * HorizentalSensitivity;
            LimiteValue(ref cameraRotate.x, MaxVerticalDegree, MinVerticalDegree);
        }
        if (Input.GetKey(KeyCode.Mouse1))
            _Player.transform.eulerAngles = new Vector3(0, cameraRotate.y, 0);
        // 监听摄像机距离
        _cameraRadius.z += Input.GetAxis("Mouse ScrollWheel") * ZoomSensitivity;
        LimiteValue(ref _cameraRadius.z, -MinZoomDistance, -MaxZoomDistance);
    }
    private void UpdateView()
    {
        // 调整摄像机高度和角度
        transform.position = _Player.transform.position + playerHeight;
        transform.localEulerAngles = cameraRotate;
        transform.TransformDirection(cameraRotate);
        transform.Translate(_cameraRadius);
    }
    private float toDeg(float rad)
    { return rad * 180 / pi; }
}
