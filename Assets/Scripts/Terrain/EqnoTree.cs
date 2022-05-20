using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqnoTree
{
    private static GameObject Surface = GameObject.Find("Surface");
    // 存储树木位置 -> 树木方块
    public static Dictionary<long, Dictionary<int, List<GameObject>>> tree
        = new Dictionary<long, Dictionary<int, List<GameObject>>>();
    public static Dictionary<long, Dictionary<int, GameObject>> map
        = new Dictionary<long, Dictionary<int, GameObject>>();
    // 种树
    public static void PlantTree(Vector3Int treePos, int height, GameObject Trunk, GameObject Leaf)
    {
        // 获取哈希值
        long hash = ModifyBlock.GetHash(treePos.x, treePos.z);
        // 如果这个位置没有树
        if (! tree.ContainsKey(hash))
            tree.Add(hash, new Dictionary<int, List<GameObject>>());
        if (! tree[hash].ContainsKey(treePos.z))
        {
            // 生成一棵树
            List<GameObject> list = new List<GameObject>();
            tree[hash].Add(treePos.z, list);
            GenerateTree(list, treePos, height, Trunk, Leaf);
        }
    }
    // 生成树
    private static void GenerateTree(
        List<GameObject> list,
        Vector3Int treePos, int height,
        GameObject Trunk, GameObject Leaf)
    {
        // 生成树干
        for (int y=treePos.y; y<treePos.y+height; y++)
        {
            Vector3 pos = new Vector3(treePos.x, y, treePos.z);
            PutBlockAt(list, ref Trunk, pos);
        }
        // 生成树叶
        for (int r=height/3; r>=0; r--)
            for (int x=treePos.x-r; x<=treePos.x+r; x++)
                for (int z=treePos.z-r; z<=treePos.z+r; z++)
                {
                    Vector3 pos = new Vector3(x, treePos.y+height-r, z);
                    PutBlockAt(list, ref Leaf, pos);
                }
    }
    // 放置树木方块
    private static void PutBlockAt(List<GameObject> list, ref GameObject obj, Vector3 pos)
    {
        if (GetFromMap(pos) != null) return;
        // 实例化
        GameObject block = ModifyBlock.GetFromPool(obj, pos);
        block.transform.SetParent(Surface.transform);
        AddIntoMap(pos, block);
        list.Add(block);
    }
    // 从 Map 中获取实例
    public static GameObject GetFromMap(Vector3 pos)
    {
        long hash = ModifyBlock.GetHash(pos.x, pos.z);
        if (! map.ContainsKey(hash)) return null;
        var dic = map[hash];
        if (! dic.ContainsKey((int)pos.y)) return null;
        return dic[(int)pos.y];
    }
    // 向 Map 中添加引用
    public static void AddIntoMap(Vector3 pos, GameObject obj)
    {
        long hash = ModifyBlock.GetHash(pos.x, pos.z);
        if (! map.ContainsKey(hash))
            map.Add(hash, new Dictionary<int, GameObject>());
        var dic = map[hash];
        if (! dic.ContainsKey((int)pos.y))
            dic.Add((int)pos.y, obj);
    }
}