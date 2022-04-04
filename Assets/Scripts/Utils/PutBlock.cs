using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    internal class PutBlock : MonoBehaviour
    {
        public static float EPS = 0.1f;
        private static GameObject _Player = GameObject.Find("Player");
        public static void PutBlockAt(ref GameObject rot, ref GameObject obj, Vector3 pos)
        {
            Vector3 playerPos = _Player.transform.position,
                pp1 = playerPos, pp2 = playerPos;
            pp1.y -= 0.5f;
            pp2.y += 0.5f; 
            ProcessCoord(ref pp1);
            ProcessCoord(ref pp2);
            ProcessCoord(ref pos);
            if ((pos - pp1).magnitude > EPS && (pos - pp2).magnitude > EPS)
            {
                pos.z += 0.5f;
                Instantiate(obj, pos, Quaternion.identity)
                    .transform.SetParent(rot.transform);
            }
        }
        private static void ProcessCoord(ref Vector3 pos)
        {
            if (pos.x >= 0) pos.x = (int)(pos.x + 0.5);
            else pos.x = (int)(pos.x - 0.5);
            if (pos.y >= 0) pos.y = (int)(pos.y + 0.5);
            else pos.y = (int)(pos.y - 0.5);
            if (pos.z >= 0) pos.z = (int)(pos.z + 0.5);
            else pos.z = (int)(pos.z - 0.5);
        }
    }
}
