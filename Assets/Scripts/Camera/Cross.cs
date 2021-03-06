using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cross : MonoBehaviour
{
    [Header("准星的长度")] public float width = 20;
    [Header("准星的高度")] public float height = 3;
    [Header("准星的间距")] public float distance = 0;
    [Header("准星背景图")] public Texture2D texture;
    
    // 开始函数
    private void Start() { Initialize(); }
    // 绘制 UI
    private void OnGUI() { DrawCross(); }
    // 定义引用
    private Texture tex;
    private GUIStyle lineStyle;
    // 获取实例
    private void Initialize()
    {
        lineStyle = new GUIStyle();
        lineStyle.normal.background = texture;
    }
    // 绘制准星
    private void DrawCross()
    {
        GUI.Box(new Rect(Screen.width / 2 - distance / 2 - width,
            Screen.height / 2 - height / 2, width, height), tex, lineStyle);
        GUI.Box(new Rect(Screen.width / 2 + distance / 2,
            Screen.height / 2 - height / 2, width, height), tex, lineStyle);
        GUI.Box(new Rect(Screen.width / 2 - height / 2,
            Screen.height / 2 - distance / 2 - width, height, width), tex, lineStyle);
        GUI.Box(new Rect(Screen.width / 2 - height / 2,
            Screen.height / 2 + distance / 2, height, width), tex, lineStyle);
    }
}