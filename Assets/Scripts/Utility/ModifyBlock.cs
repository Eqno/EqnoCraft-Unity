using System.Collections.Generic;
using UnityEngine;
public static class ModifyBlock
{
    public static int HashKey = 1000;
    private static GameObject Inside = GameObject.Find("Inside");
    private static GameObject Surface = GameObject.Find("Surface");
    // 对象池
    public static Dictionary<GameObject, ObjectPool> pool
        = new Dictionary<GameObject, ObjectPool>();
    // 方块位置
    public static Dictionary<long, Dictionary<int, GameObject>> map
        = new Dictionary<long, Dictionary<int, GameObject>>();
    // 获取实例
    public static GameObject GetFromPool(GameObject obj, Vector3 pos)
    {
        if (! pool.ContainsKey(obj))
            pool[obj] = new ObjectPool(ref obj);
        return pool[obj].Pop(pos);
    }
    // 销毁实例
    public static void DelIntoPool(GameObject obj)
    {
        if (! pool.ContainsKey(obj))
            pool[obj] = new ObjectPool(ref obj);
        pool[obj].Push(obj);
    }
    // 获取哈希值
    public static long GetHash(float x, float z)
    { return ((long)x) * HashKey + (long)z; }
    // 从 Map 中获取实例
    public static GameObject GetFromMap(Vector3 pos)
    {
        long hash = GetHash(pos.x, pos.z);
        if (! map.ContainsKey(hash)) return null;
        var dic = map[hash];
        if (! dic.ContainsKey((int)pos.y)) return null;
        return dic[(int)pos.y];
    }
    // 向 Map 中添加引用
    public static void AddIntoMap(Vector3 pos, GameObject obj)
    {
        long hash = GetHash(pos.x, pos.z);
        if (! map.ContainsKey(hash))
            map.Add(hash, new Dictionary<int, GameObject>());
        var dic = map[hash];
        if (! dic.ContainsKey((int)pos.y))
            dic.Add((int)pos.y, obj);
    }
    // 从 Map 中删除引用
    private static void DelFromMap(Vector3 pos)
    {
        long hash = GetHash(pos.x, pos.z);
        if (map.ContainsKey(hash))
        {
            var dic = map[hash];
            if (dic.ContainsKey((int)pos.y))
            {
                dic.Remove((int)pos.y);
                if (dic.Count <= 0) map.Remove(hash);
            }
        }
    }
    // 删除方块
    public static void DelBlock(GameObject obj)
    {
        // 清空位置
        DelFromMap(obj.transform.position);
        // 更新周围状态
        UpdateAround(obj.transform.position);
        // 销毁实例
        DelIntoPool(obj);
    }
    // 放置方块
    public static void PutBlockAt(ref GameObject obj, Vector3 pos)
    {
        // 实例化
        GameObject block = GetFromPool(obj, pos);
        block.transform.SetParent(Surface.transform);
        // 记录位置
        AddIntoMap(pos, block);
        // 更新周围状态
        UpdateAround(pos);
    }
    // 更新周围状态
    public static void UpdateAround(Vector3 pos)
    {
        UpdateBlock(pos + new Vector3(0, 0, 1));
        UpdateBlock(pos + new Vector3(0, 1, 0));
        UpdateBlock(pos + new Vector3(1, 0, 0));
        UpdateBlock(pos + new Vector3(0, 0, -1));
        UpdateBlock(pos + new Vector3(0, -1, 0));
        UpdateBlock(pos + new Vector3(-1, 0, 0));
    }
    // 更新当前位置方块状态
    private static void UpdateBlock(Vector3 pos)
    {
        GameObject obj = GetFromMap(pos);
        if (obj == null) return;
        if (CheckAround(pos))
        {
            obj.SetActive(true);
            obj.transform.SetParent(Surface.transform);
        }
        else
        {
            obj.SetActive(false);
            obj.transform.SetParent(Inside.transform);
        }
    }
    // 检查周围，是否暴露
    private static bool CheckAround(Vector3 pos)
    {
        if (GetFromMap(pos + new Vector3(0, 0, 1)) == null) return true;
        if (GetFromMap(pos + new Vector3(0, 1, 0)) == null) return true;
        if (GetFromMap(pos + new Vector3(1, 0, 0)) == null) return true;
        if (GetFromMap(pos + new Vector3(0, 0, -1)) == null) return true;
        if (GetFromMap(pos + new Vector3(0, -1, 0)) == null) return true;
        if (GetFromMap(pos + new Vector3(-1, 0, 0)) == null) return true;
        return false;
    }
}