using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;
using Assets.Scripts.Utils;

public class GenGround: MonoBehaviour
{
    [Header("地面")] public GameObject Ground;
    [Header("草块")] public GameObject Grass;
    [Header("基岩")] public GameObject Bedrock;
    [Header("长度")] public int length = 10;
    [Header("宽度")] public int width = 10;
    [Header("高度")] public int height = 0;
    [Header("深度")] public int depth = 1;
    [Header("线程")] public int thread = 4;
    void Awake()
    {
        for (int i = 0; i < depth; i++)
            for (int j = -length; j < length; j++)
                for (int k = -width; k < width; k++)
                    PutBlock.PutBlockAt(
                        ref Ground, 
                        ref Grass, 
                        new Vector3(j, height - i, k)
                        );
        for (int i = -length; i < length; i++)
        {
            for (int j = -width; j < width; j++)
                PutBlock.PutBlockAt(
                    ref Ground, 
                    ref Bedrock, 
                    new Vector3(i, height - depth, j)
                    );
        }
        PutBlock.PutBlockAt(ref Ground, ref Grass, new Vector3(1, 1, 1));
        PutBlock.PutBlockAt(ref Ground, ref Grass, new Vector3(0, 1, 3));
        PutBlock.PutBlockAt(ref Ground, ref Grass, new Vector3(1, 1, 3));
        PutBlock.PutBlockAt(ref Ground, ref Grass, new Vector3(1, 2, 3));
        PutBlock.PutBlockAt(ref Ground, ref Grass, new Vector3(2, 1, 3));
        PutBlock.PutBlockAt(ref Ground, ref Grass, new Vector3(2, 2, 3));
        PutBlock.PutBlockAt(ref Ground, ref Grass, new Vector3(2, 3, 3));
        PutBlock.PutBlockAt(ref Ground, ref Grass, new Vector3(3, 1, 3));
        PutBlock.PutBlockAt(ref Ground, ref Grass, new Vector3(3, 2, 3));
        PutBlock.PutBlockAt(ref Ground, ref Grass, new Vector3(3, 3, 3));
        PutBlock.PutBlockAt(ref Ground, ref Grass, new Vector3(3, 1, 2));
        PutBlock.PutBlockAt(ref Ground, ref Grass, new Vector3(3, 2, 2));
        PutBlock.PutBlockAt(ref Ground, ref Grass, new Vector3(3, 3, 2));
        PutBlock.PutBlockAt(ref Ground, ref Grass, new Vector3(3, 1, 1));
        PutBlock.PutBlockAt(ref Ground, ref Grass, new Vector3(3, 2, 1));
        PutBlock.PutBlockAt(ref Ground, ref Grass, new Vector3(3, 3, 1));
    }
}
