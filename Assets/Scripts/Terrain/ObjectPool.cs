using System;
using System.Collections.Generic;
using UnityEngine;
public class ObjectPool
{
    public GameObject Object;
    public int MaxNum = 1000000;
    public Queue<GameObject> queue = new Queue<GameObject>();
    // 构造函数
    public ObjectPool(ref GameObject obj) { Object = obj; }
    // 对象入池
    public void Push(GameObject obj)
    {        
        // 未满则塞
        if (queue.Count < MaxNum)
        {
            queue.Enqueue(obj);
            obj.SetActive(false);
        }
        else MonoBehaviour.Destroy(obj);
    }
    // 对象出池
    public GameObject Pop(Vector3 pos)
    {
        // 队列不空
        if (queue.Count > 0)
        {
            GameObject obj = queue.Dequeue();
            obj.transform.position = pos;
            obj.SetActive(true);
            return obj;
        }
        return MonoBehaviour.Instantiate(Object, pos, Quaternion.identity);
    }
    // 清空池子
    public void Clear() { queue.Clear(); }
}