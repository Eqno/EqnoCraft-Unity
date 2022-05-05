using UnityEngine;
using System.Threading;
using Assets.Scripts.Camera;

public class Perlin : MonoBehaviour
{
    private Transform _Player;
    private float seedX, seedZ;
    public int length = 30, width = 30;
    public GameObject Grass, Dirt, Stone, Bedrock;
    public int bedrockDepth = 3, stoneDepth = 20, dirtDepth = 8;
    public float bedrockRelief = 3, stoneRelief = 30, dirtRelief = 30;
    class Pair
    {
        public int x, z;
        public Pair(int x, int z) { this.x = x; this.z = z; }
    };
    private void GenerateSection(object obj)
    {
        int x = (obj as Pair).x, z = (obj as Pair).z; 

        int bedrockY = GetY(x, z, bedrockDepth, bedrockRelief);
        for (int y=-1; y<bedrockY; y++) ModifyBlock.PutBlockAt(ref Bedrock, new Vector3(x, y, z));
        
        int stoneY = bedrockDepth + stoneDepth + GetY(x, z, stoneDepth, stoneRelief);
        for (int y = bedrockY; y<stoneY; y++) ModifyBlock.PutBlockAt(ref Stone, new Vector3(x, y, z));

        int dirtY = stoneY + dirtDepth + GetY(x, z, dirtDepth, dirtRelief);
        for (int y = stoneY; y<dirtY; y++) ModifyBlock.PutBlockAt(ref Dirt, new Vector3(x, y, z));

        ModifyBlock.PutBlockAt(ref Grass, new Vector3(x, dirtY, z));
    }
    private void Awake()
    {
        seedX = Random.value * 100f;
        seedZ = Random.value * 100f;
        for (int x = -length; x < length; x++)
            for (int z = -width; z < width; z++)
                GenerateSection(new Pair(x, z));
        _Player = GameObject.Find("Player").GetComponent<Transform>();
    }
    private void LateUpdate()
    {
        for (int x=(int)_Player.position.x-length; x<(int)_Player.position.x+length; x++)
        {
            Vector3Int vert1 = new Vector3Int(x, 0, (int)_Player.position.z-width);
            if (! ModifyBlock.map.ContainsKey(ModifyBlock.GetHash(vert1)))
            {
                // Thread thread = new Thread(new ParameterizedThreadStart(GenerateSection));
                // thread.Start(new Pair(vert1.x, vert1.z));
                GenerateSection(new Pair(vert1.x, vert1.z));
            }
            Vector3Int vert2 = new Vector3Int(x, 0, (int)_Player.position.z+width);
            if (! ModifyBlock.map.ContainsKey(ModifyBlock.GetHash(vert2)))
            {
                // Thread thread = new Thread(new ParameterizedThreadStart(GenerateSection));
                // thread.Start(new Pair(vert2.x, vert2.z));
                GenerateSection(new Pair(vert2.x, vert2.z));
            }
        }
        for (int z=(int)_Player.position.z-width; z<(int)_Player.position.z+width; z++)
        {
            Vector3Int vert1 = new Vector3Int((int)_Player.position.x-length, 0, z);
            if (! ModifyBlock.map.ContainsKey(ModifyBlock.GetHash(vert1)))
            {
                // Thread thread = new Thread(new ParameterizedThreadStart(GenerateSection));
                // thread.Start(new Pair(vert1.x, vert1.z));
                GenerateSection(new Pair(vert1.x, vert1.z));
            }
            Vector3Int vert2 = new Vector3Int((int)_Player.position.x+length, 0, z);
            if (! ModifyBlock.map.ContainsKey(ModifyBlock.GetHash(vert2)))
            {
                // Thread thread = new Thread(new ParameterizedThreadStart(GenerateSection));
                // thread.Start(new Pair(vert2.x, vert2.z));
                GenerateSection(new Pair(vert2.x, vert2.z));
            }
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