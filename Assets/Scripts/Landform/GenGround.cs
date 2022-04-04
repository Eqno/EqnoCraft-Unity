using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;
using Assets.Scripts.Utils;

public class GenGround: MonoBehaviour
{
    [Header("����")] public GameObject Ground;
    [Header("�ݿ�")] public GameObject Grass;
    [Header("����")] public GameObject Bedrock;
    [Header("����")] public int length = 10;
    [Header("���")] public int width = 10;
    [Header("�߶�")] public int height = 0;
    [Header("���")] public int depth = 1;
    [Header("�߳�")] public int thread = 4;
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
