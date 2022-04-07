using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SwitchBlock : MonoBehaviour
{
    [Header("方框水平位置")] public float left = -400;
    [Header("方框垂直位置")] public float down = -640;
    [Header("方框右移距离")] public float move = 110;

    private RectTransform Tab;
    private RayTest BlockIndex;
    void Start()
    {
        BlockIndex = GameObject.Find("Camera").GetComponent<RayTest>();
        Tab = GameObject.Find("TabPartTwo").GetComponent<RectTransform>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            BlockIndex.index = 0;
            Tab.anchoredPosition = new Vector2(left + 0 * move, down);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BlockIndex.index = 1;
            Tab.anchoredPosition = new Vector2(left + 1 * move, down);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            BlockIndex.index = 2;
            Tab.anchoredPosition = new Vector2(left + 2 * move, down);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            BlockIndex.index = 3;
            Tab.anchoredPosition = new Vector2(left + 3 * move, down);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            BlockIndex.index = 4;
            Tab.anchoredPosition = new Vector2(left + 4 * move, down);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            BlockIndex.index = 5;
            Tab.anchoredPosition = new Vector2(left + 5 * move, down);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            BlockIndex.index = 6;
            Tab.anchoredPosition = new Vector2(left + 6 * move, down);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            BlockIndex.index = 7;
            Tab.anchoredPosition = new Vector2(left + 7 * move, down);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            BlockIndex.index = 8;
            Tab.anchoredPosition = new Vector2(left + 8 * move, down);
        }
            
    }
}
