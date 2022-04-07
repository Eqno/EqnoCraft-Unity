using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;

public class RayTest : MonoBehaviour
{
    [Header("���÷�������")] public int index;
    [Header("���÷�������")] public GameObject[] Obj;
    [Header("���ÿ鸸����")] public GameObject Ground;
    [Header("���߼�����")] public float maxDistance = 10;

    private Camera _camera;
    private GameObject _Player;
    void Start()
    {
        _camera = GetComponent<Camera>();
        _Player = GameObject.Find("Player");
    }
    void Update()
    {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !hit.collider.gameObject.Equals(_Player))
                Destroy(hit.collider.gameObject);
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector3 dir = transform.position - hit.transform.position;
                Vector3 pos = hit.point + dir / dir.magnitude * PutBlock.EPS;
                if (index < Obj.Length) PutBlock.PutBlockAt(ref Ground, ref Obj[index], pos);
            }
        }
    }
}
