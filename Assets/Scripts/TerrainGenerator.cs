
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 20f;
    public int xOffSet;
    public int yOffSet;

    // Start is called before the first frame update
    void Start()
    {
        xOffSet = Random.Range(0, 99999);
        yOffSet = Random.Range(0, 99999);
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();
    }

    private Texture GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Color color = CalculateColor(x,y);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }

    private Color CalculateColor(int x,int y)
    {
        float xCoord = (float)x / width *scale + xOffSet;
        float yCoord = (float)y / height *scale+ yOffSet;
        float sample = Mathf.PerlinNoise(xCoord, yCoord);
         return new Color(sample, sample, sample);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
