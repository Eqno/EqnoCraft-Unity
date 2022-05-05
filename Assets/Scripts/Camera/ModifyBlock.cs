using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Camera
{
    internal class ModifyBlock : MonoBehaviour
    {
        public static int HashKey = 1000;
        public static float EPS = 0.05f;
        private static GameObject Inside = GameObject.Find("Inside");
        private static GameObject _Player = GameObject.Find("Player");
        private static GameObject Surface = GameObject.Find("Surface");
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
            Destroy(obj);
        }
        // 放置方块
        public static void PutBlockAt(ref GameObject obj, Vector3 pos)
        {
            // 获取角色位置
            Vector3 playerPos = _Player.transform.position;
            Vector3 pp1 = playerPos + new Vector3(0, -0.5f, 0);
            Vector3 pp2 = playerPos + new Vector3(0, 0.5f, 0);
            // 整数化
            ProcessCoord(ref pp1);
            ProcessCoord(ref pp2);
            ProcessCoord(ref pos);
            // 如果不在角色身上
            if ((pos - pp1).magnitude > EPS && (pos - pp2).magnitude > EPS)
            {
                // 实例化
                GameObject block = Instantiate(obj, pos, Quaternion.identity);
                block.transform.SetParent(Surface.transform);
                // 记录位置
                map[GetHash(pos)] = block;
                // 更新周围状态
                UpdateState(pos);
            }
        }
        // 坐标整数化
        private static void ProcessCoord(ref Vector3 pos)
        {
            if (pos.x >= 0) pos.x = (int)(pos.x + 0.5);
            else pos.x = (int)(pos.x - 0.5);
            if (pos.y >= 0) pos.y = (int)(pos.y + 0.5);
            else pos.y = (int)(pos.y - 0.5);
            if (pos.z >= 0) pos.z = (int)(pos.z + 0.5);
            else pos.z = (int)(pos.z - 0.5);
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
        // 检查当前位置是否有方块
        private static bool CheckBlock(Vector3 pos)
        {
            long hash = GetHash(pos);
            return map.ContainsKey(hash) && map[hash]!=null;
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
        // 获取哈希值
        public static long GetHash(Vector3 pos)
        { return ((long)pos.x)*HashKey*HashKey + ((long)pos.y)*HashKey + (long)pos.z; }
    }
}
