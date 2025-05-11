using UnityEngine;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] tilePrefabs; // Assign in Inspector: [plains, forest, hills, mountains]
    private List<GameObject> tileGroups;

    public const int MAP_WIDTH = 120;

    public Vector2 offset = new Vector2(-MAP_WIDTH / 2, -MAP_WIDTH / 2);

    private int baseOffset = 0;
   

    public int[,] noiseID; 
    public bool[,] isBusy;
    //private GameObject[,] tileGrid; 


    [SerializeField] private float spawnRadius = 2;
    [SerializeField] private float spawnFalloff = 9;
    [SerializeField] private int spawnTileIndex = 3;



    void Start()
    {
        InitializeArrays();
        CreateTileGroups();
        GenerateMap();
        
    }

    void InitializeArrays()
    {
        noiseID = new int[MAP_WIDTH, MAP_WIDTH];
        isBusy = new bool[MAP_WIDTH, MAP_WIDTH];
        
        //tileGrid = new GameObject[MAP_WIDTH, MAP_HEIGHT];
        tileGroups = new List<GameObject>();
    }

    void CreateTileGroups()
    {
        for (int i = 0; i < tilePrefabs.Length; i++)
        {
            tileGroups.Add(new GameObject(tilePrefabs[i].name));
            tileGroups[i].transform.parent = transform;
            tileGroups[i].transform.localPosition = Vector3.zero;
        }
        //GameObject g = new GameObject("Mountains");
        //g.transform.parent = transform;
        //g.transform.localPosition = Vector3.zero;
        //tileGroups.Add(g);
    }

    void GenerateMap()
    {
        baseOffset = Random.Range(-10000, 10000);
        rockOffset = Random.Range(-10000, 10000);
        biomeOffset = Random.Range(-10000, 10000);
        treeOffset = baseOffset;
        //mountainOffset = Random.Range(-10000, 10000);
        for (int x = 0; x < MAP_WIDTH; x++)
        {
            for (int y = 0; y < MAP_WIDTH; y++)
            {
                int tileID = GetIdUsingPerlin(x, y);
                noiseID[x, y] = tileID;
                CreateTile(x, y);
                AddRockTile(x, y);
                AddTreeTile(x, y);
                if (tileID == 3)
                    AddMountainTile(x, y);
            }
        }
        
    }
    
    [Header("Base map Settings")]
    [SerializeField] private float scale = 15;
    [SerializeField] private int octaveCount = 3;
    [SerializeField] private float lacunarity = 2;
    [SerializeField] private float persistence = 0.3f;


    


 
    private int GetIdUsingPerlin(int x, int y)
    {
        float rawPerlin = MathStuff.perlin(x - baseOffset, y - baseOffset, scale, octaveCount, lacunarity, persistence);



        float dist2 = (offset + new Vector2(x, y)).magnitude;//Vector2.SqrMagnitude(offset + new Vector2(x,y));
        float flattenMultiplier = Mathf.Clamp01((dist2 - spawnRadius) / spawnFalloff);
        float flattenedPerlin = Mathf.Clamp01(rawPerlin * flattenMultiplier); //flatten area around tower 


        if (flattenMultiplier < 0.5f)
            isBusy[x, y] = true;   

        float tiledPerlin = flattenedPerlin * tilePrefabs.Length;

        if (tiledPerlin == tilePrefabs.Length)
            tiledPerlin = tilePrefabs.Length - 1;

        int id = Mathf.FloorToInt(tiledPerlin);

        if (id == 0 && flattenMultiplier < 1)
            id = spawnTileIndex;

        return id;
    }

    private void CreateTile(int x, int y)
    {
        GameObject tile = Instantiate(tilePrefabs[noiseID[x, y]], offset + new Vector2(x, y), Quaternion.identity, tileGroups[noiseID[x, y]].transform);
        tile.name = $"tile_x{x}_y{y}";
        tile.GetComponent<SpriteRenderer>().sortingLayerName = "Background";
        //tileGrid[x, y] = tile;
        
    }


    [Header("Rock map Settings")]
    [SerializeField] private int rockOctaveCount = 1;          // Single octave = no smoothing
    [SerializeField] private float rockLacunarity = 4f;       // Extreme frequency jumps
    [SerializeField] private float rockPersistence = 0.2f;     // Weak amplitude decay
    [SerializeField] private float rockScale = 3f;            // Very small scale = high freq
    [SerializeField] private int rockOffset;          
    [SerializeField] private float rockThreshold = 0.9f;       // Only the top 10% spawn rocks
    [SerializeField] private GameObject rock;
   

    void AddRockTile(int x, int y)
    {
        int id = noiseID[x, y];
        if (id != 1 && id != 2)
            return;
        float multiplier = 1;
        if (id == 1) multiplier = Random.Range(0,2);
        float rawPerlin = multiplier * MathStuff.perlin(x - rockOffset, y - rockOffset, rockScale, rockOctaveCount, rockLacunarity, rockPersistence);

        if (rawPerlin > rockThreshold && !isBusy[x,y])
        {
            isBusy[x, y] = true;
            GameObject tile = Instantiate(rock, offset + new Vector2(x, y), Quaternion.identity, transform.Find("Rocks"));
            tile.name = $"tile_x{x}_y{y}";
            tile.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground1";
        }
    }

    [Header("Tree Map Settings")]
    [SerializeField] private int treeOctaveCount = 2;          // Slight smoothing for small clusters
    [SerializeField] private float treeLacunarity = 3f;       // Balanced frequency jumps
    [SerializeField] private float treePersistence = 0.4f;    // Moderate amplitude decay
    [SerializeField] private float treeScale = 12f;           // Medium scale for natural spacing
    private int treeOffset;
    [SerializeField] private float treeThreshold = 0.78f;     // ~22% spawn chance
    [SerializeField] private GameObject[] treePrefabs;        // Array of tree variants

    void AddTreeTile(int x, int y)
    {
        int id = noiseID[x, y];
        if (!IsInTreeBiome(x, y)) return;

        float multiplier = (id == 2) ? Mathf.Clamp01(Random.Range(0, 5)) : 1f;

        float rawPerlin = multiplier * MathStuff.perlin(
            x - treeOffset,
            y - treeOffset,
            treeScale,
            treeOctaveCount,
            treeLacunarity,
            treePersistence
        );
        if (id == 1 && multiplier != 0) rawPerlin = 1 - rawPerlin; //invert 
        if (rawPerlin > treeThreshold && !isBusy[x, y])
        {
            isBusy[x, y] = true;
            GameObject treePrefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
            GameObject tile = Instantiate(
                treePrefab,
                offset + new Vector2(x, y),
                Quaternion.identity,
                transform.Find("Trees")
            );
            tile.name = $"tree_x{x}_y{y}";
            tile.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground2";
        }
    }

    [Header("Tree Biome Mask Settings")]
    [SerializeField] private float biomeScale = 80f;          // Large scale = broad areas
    [SerializeField] private float biomeThreshold = 0.4f;    // Lower = bigger forests
    private int biomeOffset = 2000;

    public bool IsInTreeBiome(int x, int y)
    {
        int id = noiseID[x, y];
        if (id < 1 || id > 2)
            return false;
        float biomeValue = Mathf.PerlinNoise(
            (x + biomeOffset) / biomeScale,
            (y + biomeOffset) / biomeScale
        );
        return biomeValue > biomeThreshold; // Binary true/false
    }
    public bool IsInHillBiome(int x, int y) { return !IsInTreeBiome(x, y) && noiseID[x, y] != 0 && noiseID[x, y] != 3; }
    public bool IsInMountains(int x, int y) { return noiseID[x, y] == 3; }

    //[Header("Mountain Map Settings")]
    //[SerializeField] private int mountainOctaveCount = 4;          // More octaves = smoother ridges
    //[SerializeField] private float mountainLacunarity = 2.1f;     // Moderate frequency jumps
    //[SerializeField] private float mountainPersistence = 0.65f;   // Stronger amplitude decay
    //[SerializeField] private float mountainScale = 25f;           // Larger scale = broader clusters
    //[SerializeField] private int mountainOffset; 
    //[SerializeField] private float mountainThreshold = 0.55f;     // Lower threshold = more coverage
    [SerializeField] private GameObject mountainPrefab;

    public void AddMountainTile(int x, int y)
    {
        if (x < 0 || x > MAP_WIDTH || y < 0 || y > MAP_WIDTH || isBusy[x,y])
            return;
        isBusy[x, y] = true;
        GameObject tile = Instantiate(
            mountainPrefab,
            offset + new Vector2(x, y),
            Quaternion.identity,
            transform.Find("Mountains")
        );
        tile.name = $"tile_x{x}_y{y}";
        tile.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground1";
        //float rawPerlin = MathStuff.perlin(
        //    x - mountainOffset,
        //    y - mountainOffset,
        //    mountainScale,
        //    mountainOctaveCount,
        //    mountainLacunarity,
        //    mountainPersistence
        //);

        //if (rawPerlin > mountainThreshold && !isBusy[x, y])
        //{
        //    isBusy[x, y] = true;
        //    GameObject tile = Instantiate(
        //        mountainPrefab,
        //        offset + new Vector2(x, y),
        //        Quaternion.identity,
        //        transform.Find("Mountains")
        //    );
        //    tile.name = $"tile_x{x}_y{y}";
        //    tile.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground1";
        //}
    }
    //[SerializeField] private GameObject[] mountainPrefabs;
    //int mountainID = 3;
    //private void GenerateMountains()
    //{
    //    Dictionary<Vector2Int, int> clock = new Dictionary<Vector2Int, int>
    //    {
    //        [new Vector2Int(0, 1)] = 0,
    //        [new Vector2Int(1, 1)] = 1,
    //        [new Vector2Int(1, 0)] = 2,
    //        [new Vector2Int(1, -1)] = 3,
    //        [new Vector2Int(0, -1)] = 4,
    //        [new Vector2Int(-1, -1)] = 5,
    //        [new Vector2Int(-1, 0)] = 6,
    //        [new Vector2Int(-1, 1)] = 7,
    //        [new Vector2Int(0, 0)] = 8,
    //        [new Vector2Int(0, 0)] = 9
    //    };

    //    Vector2Int[] directions = {Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
    //    for(int i = 3; i < MAP_WIDTH - 3; ++i)
    //    {
    //        for (int j = 3; j < MAP_HEIGHT - 3; ++j)
    //        {
    //            if (noiseGrid[i, j] != mountainID)
    //                continue;
    //            isBusy[i, j] = true;
    //            foreach (Vector2Int d in directions)
    //                if (F(d, i, j)) continue;


    //            //InitMountain(8, i, j);

    //        }
    //    }
    //    bool F(Vector2Int direction, int x, int y)
    //    {
    //        //x + direction.x < MAP_WIDTH && x + direction.x > 0 && y + direction.y < MAP_HEIGHT && y + direction.y > 0 &&
    //        int x1 = x + direction.x;
    //        int y1 = y + direction.y;
    //        if (noiseGrid[x1, y1] != mountainID)
    //        {
    //            Vector2Int perp = new Vector2Int(direction.y, direction.x);
    //            Vector2Int arrow1 = new Vector2Int(perp.x, perp.y);
    //            Vector2Int arrow2 = new Vector2Int(- perp.x, - perp.y);
    //            if (noiseGrid[x1 + arrow1.x, y1 + arrow1.y] != mountainID)
    //                InitMountain(clock[arrow1], x, y);
    //            else if (noiseGrid[x1 + arrow2.x, y1 + arrow2.y] != mountainID)
    //                InitMountain(clock[arrow2], x, y);
    //            else
    //                InitMountain(clock[direction], x, y);
    //            return true;
    //        }


    //        return false;
    //    }
    //    void InitMountain(int index, int x, int y)
    //    {
    //        GameObject tile = Instantiate(mountainPrefabs[index], offset + new Vector2(x, y), Quaternion.identity, transform.Find("Mountains"));
    //        tile.name = $"tile_x{x}_y{y}";
    //        tile.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground2";

    //    }
    //}
}