using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Camera;

public class Perlin : MonoBehaviour
{
    public int length = 30, width = 30;
    public GameObject Grass, Dirt, Stone, Bedrock;
    public int bedrockDepth = 3, stoneDepth = 20, dirtDepth = 8;
    public float bedrockRelief = 3, stoneRelief = 30, dirtRelief = 30;

    private Transform _Player;
    private float seedX, seedZ;
    private GameObject Surface, Inside;
    // 放置方块
    IEnumerator PutBlockAt(GameObject obj, Vector3 pos, int state)
    {
        // 实例化
        GameObject block = ModifyBlock.GetFromPool(obj, pos);
        // 记录位置
        ModifyBlock.AddIntoMap(pos, block);
        // 未定
        if (state == 0)
        {
            // 设置父亲
            block.transform.SetParent(Surface.transform);
            // 更新周围状态
            ModifyBlock.UpdateState(pos);
        }
        // 内部
        else if (state == 1)
        {
            block.transform.SetParent(Inside.transform);
            block.GetComponent<MeshRenderer>().enabled = false;
        }
        // 外部
        else if (state == 2)
        {
            block.transform.SetParent(Surface.transform);
            block.GetComponent<MeshRenderer>().enabled = true;
        }
        yield return null;
    }
    IEnumerator GenerateSection(int x, int z, bool init)
    {
        if (! ModifyBlock.map.ContainsKey(ModifyBlock.GetHash(x, z)))
        {
            int bedrockY = GetY(x, z, bedrockDepth, bedrockRelief);
            int stoneY = bedrockDepth + stoneDepth + GetY(x, z, stoneDepth, stoneRelief);
            int dirtY = stoneY + dirtDepth + GetY(x, z, dirtDepth, dirtRelief);
            for (int y=-1; y<bedrockY; y++)
            {
                StartCoroutine(PutBlockAt(Bedrock, new Vector3(x, y, z), init?1:0));
                if (! init) yield return null;
            }
            for (int y = bedrockY; y<stoneY; y++)
            {
                StartCoroutine(PutBlockAt(Stone, new Vector3(x, y, z), init?1:0));
                if (! init) yield return null;
            }
            for (int y = stoneY; y<dirtY; y++)
            {
                StartCoroutine(PutBlockAt(Dirt, new Vector3(x, y, z), 0));
                if (! init) yield return null;
            }
            StartCoroutine(PutBlockAt(Grass, new Vector3(x, dirtY, z), init?2:0));
        }
    }
    IEnumerator DeleteSection(int x, int z)
    {
        long hash = ModifyBlock.GetHash(x, z);
        if (ModifyBlock.map.ContainsKey(hash))
        {
            foreach (var item in ModifyBlock.map[hash])
                ModifyBlock.DelIntoPool(item.Value);
            ModifyBlock.map.Remove(hash);
            yield return new WaitForEndOfFrame();
        }
    }
    private void Awake()
    {
        Inside = GameObject.Find("Inside");
        Surface = GameObject.Find("Surface");
        seedX = Random.value * 100f;
        seedZ = Random.value * 100f;
        for (int x=-length; x<length; x++)
            for (int z=-width; z<width; z++)
                StartCoroutine(GenerateSection(x, z, true));
        _Player = GameObject.Find("Player").GetComponent<Transform>();
    }
    private void LateUpdate()
    {
        int x = (int)_Player.position.x, z = (int)_Player.position.z;
        if (! ModifyBlock.map.ContainsKey(ModifyBlock.GetHash(x, z+width)))
        {
            for (int i=x-length; i<x+length; i++)
                StartCoroutine(GenerateSection(i, z+width, false));
        }
        if (! ModifyBlock.map.ContainsKey(ModifyBlock.GetHash(x, z-width)))
        {
            for (int i=x-length; i<x+length; i++)
                StartCoroutine(GenerateSection(i, z-width, false));
        }
        if (! ModifyBlock.map.ContainsKey(ModifyBlock.GetHash(x+length, z)))
        {
            for (int i=z-width; i<z+width; i++)
                StartCoroutine(GenerateSection(x+length, i, false));
        }
        if (! ModifyBlock.map.ContainsKey(ModifyBlock.GetHash(x-length, z)))
        {
            for (int i=z-width; i<z+width; i++)
                StartCoroutine(GenerateSection(x-length, i, false));
        }
        
        if (ModifyBlock.map.ContainsKey(ModifyBlock.GetHash(x, z+width+1)))
        {
            for (int i=x-length-1; i<x+length+1; i++)
                StartCoroutine(DeleteSection(i, z+width+1));
        }
        if (ModifyBlock.map.ContainsKey(ModifyBlock.GetHash(x, z-width-1)))
        {
            for (int i=x-length-1; i<x+length+1; i++)
                StartCoroutine(DeleteSection(i, z-width-1));
        }
        if (ModifyBlock.map.ContainsKey(ModifyBlock.GetHash(x+length+1, z)))
        {
            for (int i=z-width-1; i<z+width+1; i++)
                StartCoroutine(DeleteSection(x+length+1, i));
        }
        if (ModifyBlock.map.ContainsKey(ModifyBlock.GetHash(x-length-1, z)))
        {
            for (int i=z-width-1; i<z+width+1; i++)
                StartCoroutine(DeleteSection(x-length-1, i));
        }
    }
    private int GetY(float x, float z, float depth, float relief)
    {
        float xSample = (x + seedX) / relief;
        float zSample = (z + seedZ) / relief;
        float noise = Mathf.PerlinNoise(xSample, zSample);
        return (int)(depth * noise);
    }
}