using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Camera
{
    internal class ModifyBlock : MonoBehaviour
    {
        public static int HashKey = 1000;
        private static GameObject Inside = GameObject.Find("Inside");
        private static GameObject _Player = GameObject.Find("Player");
        private static GameObject Surface = GameObject.Find("Surface");
        // 对象池
        private static Dictionary<GameObject, ObjectPool> pool
            = new Dictionary<GameObject, ObjectPool>();
        // 方块位置
        public static Dictionary<long, GameObject> map = new Dictionary<long, GameObject>();
        // 删除方块
        public static void DelBlock(GameObject obj)
        {
            // 删除位置
            map[GetHash(obj.transform.position)] = null;
            // 更新周围状态
            UpdateState(obj.transform.position);
            // 销毁实例
            if (! pool.ContainsKey(obj)) pool[obj] = new ObjectPool(ref obj);
            pool[obj].Push(obj);
        }
        // 放置方块
        public static void PutBlockAt(ref GameObject obj, Vector3 pos)
        {
            // 实例化
            if (! pool.ContainsKey(obj)) pool[obj] = new ObjectPool(ref obj);
            GameObject block = pool[obj].Pop(pos);
            block.transform.SetParent(Surface.transform);
            // 记录位置
            map[GetHash(pos)] = block;
            // 更新周围状态
            UpdateState(pos);
        }
        // 更新周围状态
        private static void UpdateState(Vector3 pos)
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
            if (! CheckBlock(pos)) return;
            GameObject block = map[GetHash(pos)];
            if (CheckAround(pos))
            {
                block.transform.SetParent(Surface.transform);
                block.GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                block.transform.SetParent(Inside.transform);
                block.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        // 检查周围，是否暴露
        private static bool CheckAround(Vector3 pos)
        {
            if (! CheckBlock(pos + new Vector3(0, 0, 1))) return true;
            if (! CheckBlock(pos + new Vector3(0, 1, 0))) return true;
            if (! CheckBlock(pos + new Vector3(1, 0, 0))) return true;
            if (! CheckBlock(pos + new Vector3(0, 0, -1))) return true;
            if (! CheckBlock(pos + new Vector3(0, -1, 0))) return true;
            if (! CheckBlock(pos + new Vector3(-1, 0, 0))) return true;
            return false;
        }
        // 检查当前位置是否有方块
        public static bool CheckBlock(Vector3 pos)
        {
            long hash = GetHash(pos);
            return map.ContainsKey(hash) && map[hash]!=null;
        }
        // 获取哈希值
        public static long GetHash(Vector3 pos)
        { return ((long)pos.x)*HashKey*HashKey + ((long)pos.y)*HashKey + (long)pos.z; }
    }
}
