using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;

public class RayTest : MonoBehaviour
{
    [Header("放置方块索引")] public int index;
    [Header("放置方块种类")] public GameObject[] Obj;
    [Header("放置块父物体")] public GameObject Ground;
    [Header("射线检测距离")] public float maxDistance = 10;

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
