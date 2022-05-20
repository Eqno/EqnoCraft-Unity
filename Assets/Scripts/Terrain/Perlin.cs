using System.Collections;
using System.Threading;
using UnityEngine;
using System;
using System.Collections.Generic;
public class Perlin : MonoBehaviour
{
    public GameObject Grass, Dirt, Stone, Bedrock, Wood, Leaf, Inside, Surface;
    public int length = 30, width = 30;
    public int PutSpeed = 3, DelRadius = 3, LoadingTime = 3;
    public int BedrockDepth = 3, StoneDepth = 15, DirtDepth = 5;
    public float BedrockRelief = 3, StoneRelief = 15, DirtRelief = 20;
    public int TreeRandMax = 10000, TreeRandMod = 444, TreeHeightMin = 5, TreeHeightMax = 8;
    
    private Expand expand;
    private Contract contract;
    private Transform _Player;
    private float seedX, seedZ;
    // 开始函数
    private void Start()
    {
        // 生成随机种子
        seedX = UnityEngine.Random.value * 100;
        seedZ = UnityEngine.Random.value * 100;
        // 视野拓展
        expand = new Expand(
            Grass, Dirt, Stone, Bedrock, Wood, Leaf, Inside, Surface,
            seedX, seedZ, BedrockDepth, StoneDepth, DirtDepth,
            BedrockRelief, StoneRelief, DirtRelief, PutSpeed,
            TreeRandMax, TreeRandMod, TreeHeightMin, TreeHeightMax
        );
        // 视野收缩
        contract = new Contract();
        // 固定角色位置（固定在天空，等待地面加载）
        GameObject.Find("Camera").GetComponent<Cross>().enabled = false;
        _Player = GameObject.Find("Player").GetComponent<Transform>();
        _Player.GetComponent<Move>().enabled = false;
        // 生成地形
        GenerateTerrain();
        // 激活角色移动
        StartCoroutine(ActiveActor());
    }
    // 生成地形
    private void GenerateTerrain()
    {
        for (int x=-length; x<length; x++)
            for (int z=-width; z<width; z++)
                StartCoroutine(expand.GenerateSection(x, z, -1));
    }
    // 激活角色
    IEnumerator ActiveActor()
    {
        yield return new WaitForSeconds(LoadingTime);
        GameObject.Find("Camera").GetComponent<Cross>().enabled = true;
        GameObject.Find("LoadingPanel").SetActive(false);
        _Player.GetComponent<Move>().enabled = true;
    }
    // 更新函数
    private void Update()
    {
        int x = (int)_Player.position.x, z = (int)_Player.position.z;
        ExpandSection(x, z);
        ContractSection(x, z);
    }
    // 拓展视野区块
    private void ExpandSection(int x, int z)
    {
        for (int i=x-length-1; i<x+length+1; i++)
            StartCoroutine(expand.GenerateSection(i, z+width, 0));
        for (int i=x-length-1; i<x+length+1; i++)
            StartCoroutine(expand.GenerateSection(i, z-width, 1));
        for (int i=z-width-1; i<z+width+1; i++)
            StartCoroutine(expand.GenerateSection(x+length, i, 2));
        for (int i=z-width-1; i<z+width+1; i++)
            StartCoroutine(expand.GenerateSection(x-length, i, 3));
    }
    // 收缩视野区块
    private void ContractSection(int x, int z)
    {
        for (int r=1; r<=DelRadius; r++)
        {
            for (int i=x-length-r-2; i<x+length+r+2; i++)
                contract.DeleteSection(i, z+width+r);
            for (int i=x-length-r-2; i<x+length+r+2; i++)
                contract.DeleteSection(i, z-width-r);
            for (int i=z-width-r-2; i<z+width+r+2; i++)
                contract.DeleteSection(x+length+r, i);
            for (int i=z-width-r-2; i<z+width+r+2; i++)
                contract.DeleteSection(x-length-r, i);
        }
    }
}