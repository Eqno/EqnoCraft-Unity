using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using System.Collections;
public class Expand
{
    public int BedrockDepth, StoneDepth, DirtDepth;
    private GameObject Grass, Dirt, Stone, Bedrock, Wood, Leaf, Inside, Surface;
    public float seedX, seedZ, BedrockRelief, StoneRelief, DirtRelief;
    public int PutSpeed, TreeRandMax, TreeRandMod, TreeHeightMin, TreeHeightMax;
    // 构造传参
    public Expand(
        GameObject Grass,
        GameObject Dirt,
        GameObject Stone,
        GameObject Bedrock,
        GameObject Wood,
        GameObject Leaf,
        GameObject Inside,
        GameObject Surface,
        float seedX,
        float seedZ,
        int BedrockDepth,
        int StoneDepth,
        int DirtDepth,
        float BedrockRelief,
        float StoneRelief,
        float DirtRelief,
        int PutSpeed,
        int TreeRandMax,
        int TreeRandMod,
        int TreeHeightMin,
        int TreeHeightMax)
    {
        this.Grass = Grass;
        this.Dirt = Dirt;
        this.Stone = Stone;
        this.Bedrock = Bedrock;
        this.Wood = Wood;
        this.Leaf = Leaf;
        this.Inside = Inside;
        this.Surface = Surface;
        this.seedX = seedX;
        this.seedZ = seedZ;
        this.BedrockDepth = BedrockDepth;
        this.StoneDepth = StoneDepth;
        this.DirtDepth = DirtDepth;
        this.BedrockRelief = BedrockRelief;
        this.StoneRelief = StoneRelief;
        this.DirtRelief = DirtRelief;
        this.PutSpeed = PutSpeed;
        this.TreeRandMax = TreeRandMax;
        this.TreeRandMod = TreeRandMod;
        this.TreeHeightMin = TreeHeightMin;
        this.TreeHeightMax = TreeHeightMax;
    }
    // 生成区块
    public IEnumerator GenerateSection(int x, int z, int dir)
    {
        // 如果此处没有方块
        if (! ModifyBlock.map.ContainsKey(ModifyBlock.GetHash(x, z)))
        {
            // 计算基岩层高度
            int bedrockY = GetY(
                x, z, 
                BedrockDepth,
                BedrockRelief,
                seedX, seedZ
            );
            // 计算石头层高度
            int stoneY = BedrockDepth + StoneDepth + GetY(
                x, z,
                StoneDepth,
                StoneRelief,
                seedX, seedZ
            );
            // 计算泥土层高度
            int dirtY = stoneY + DirtDepth + GetY(
                x, z,
                DirtDepth,
                DirtRelief,
                seedX, seedZ
            );
            // 种树
            int t = GetT(x, z, seedX, seedZ, TreeRandMax);
            if (t % TreeRandMod == 0)
                EqnoTree.PlantTree(
                    new Vector3Int(x, dirtY + 1, z),
                    (t % (TreeHeightMax-TreeHeightMin) + TreeHeightMin),
                    Wood, Leaf
                );
            // 生成泥土层
            for (int y=dirtY-1; y>=stoneY; y--)
            {
                PutBlockAt(
                    Dirt,
                    new Vector3(x, y, z),
                    dir
                );
            }
            // 生成草地层
            if (dir == -1) PutBlockAt(Grass, new Vector3(x, dirtY, z), -2);
            else PutBlockAt(Grass, new Vector3(x, dirtY, z), dir);
            // 生成石头层
            for (int y=stoneY-1; y>=bedrockY; y--)
            {
                PutBlockAt(
                    Stone,
                    new Vector3(x, y, z),
                    dir
                );
                if (dir != -1 && y % PutSpeed == 0)
                    yield return null;
            }
            // 生成基岩层
            for (int y=bedrockY-1; y>=-1; y--)
            {
                PutBlockAt(
                    Bedrock,
                    new Vector3(x, y, z),
                    dir
                );
            }
        }
    }
    // 放置方块
    private void PutBlockAt(GameObject obj, Vector3 pos, int dir)
    {
        GameObject block = ModifyBlock.GetFromPool(obj, pos);
        ModifyBlock.AddIntoMap(pos, block);
        if (dir == -1)
        {
            block.transform.SetParent(Inside.transform);
            block.SetActive(false);
        }
        block.transform.SetParent(Surface.transform);
        ModifyBlock.UpdateAround(pos);
    }
    // 柏林函数获取层高
    private int GetY(float x, float z,
        float depth, float relief, float seedX, float seedZ)
    {
        float xSample = (x + seedX) / relief;
        float zSample = (z + seedZ) / relief;
        float noise = Mathf.PerlinNoise(xSample, zSample);
        return (int)(depth * noise);
    }
    // 柏林函数获取树种子
    private int GetT(float x, float z, float seedX, float seedZ, int TreeRandMax)
    {
        float xSample = (x + seedX);
        float zSample = (z + seedZ);
        float noise = Mathf.PerlinNoise(xSample, zSample);
        return (int)(TreeRandMax * noise);
    }
}
