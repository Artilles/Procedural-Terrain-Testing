using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode
    {
        NoiseMap,
        ColorMap
    };

    public DrawMode drawMode = DrawMode.ColorMap;

    public int seed;
    public bool autoUpdate = false;
    public int mapWidth = 100;
    public int mapHeight = 100;
    public float noiseScale = 1f;

    public int octaves = 4;
    [Range(0, 1)]
    public float persistance = 0.5f;
    public float lacunarity = 2f;
    public Vector2 offset;

    public TerrainType[] regions;

    public void GenerateMap()
    {
        float[,] noiseMap = NoiseGenerator.GenerateNoiseMap(mapWidth, mapHeight, noiseScale, seed, octaves, persistance, lacunarity, offset);

        Color[] colorMap = new Color[mapWidth * mapHeight];
        for(int y = 0; y < mapHeight; y++) {
            for(int x = 0; x < mapWidth; x++) {
                float currentHeight = noiseMap[x, y];
                for(int i = 0; i < regions.Length; i++) {
                    if (currentHeight <= regions[i].height) {
                        colorMap[(y * mapWidth) + x] = regions[i].terrainColor;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();

        if (drawMode == DrawMode.NoiseMap)
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        else if (drawMode == DrawMode.ColorMap)
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));

    }

    private void OnValidate()
    {
        if(mapWidth < 1)
            mapWidth = 1;

        if (mapHeight < 1)
            mapHeight = 1;

        if (lacunarity < 1f)
            lacunarity = 1f;

        if (octaves < 0)
            octaves = 0;
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color terrainColor;
}
