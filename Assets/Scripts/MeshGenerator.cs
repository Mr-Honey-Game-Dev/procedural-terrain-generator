
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] verticies;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

    public int width = 256;
    public int height = 256;

    public float scale = 0.2f;
    public float zoomScale = 0.3f;

    public float xOffSet;
    public float zOffSet;

   float minTerrainHeight=10000;
   float maxTerrainHeight=-10000;

    Color[] VertexColors;
    public Gradient gradient;

   
    // Start is called before the first frame update
    void Start()
    {
        xOffSet = Random.Range(1.0f,10000);
        zOffSet = Random.Range(1.0f,10000);
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        StartCoroutine( CreateShape());
         
    }
    IEnumerator CreateShape()
    {
        verticies = new Vector3[(xSize + 1) * (zSize + 1)];
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {

                // y = Mathf.PerlinNoise(x * 0.3f + xOffSet, z * 0.3f + zOffSet) *2f ;
                float y = 0;
                y = Mathf.PerlinNoise(x * zoomScale + xOffSet, z * zoomScale + zOffSet) * scale + 
                    Mathf.PerlinNoise(x * zoomScale + xOffSet+0.5f, z * zoomScale + zOffSet+0.5f) * scale/2 +
                    Mathf.PerlinNoise(x * zoomScale + xOffSet + 0.5f+0.25f, z * zoomScale + zOffSet + 0.5f+0.25f) * scale / 4;

                verticies[i]=new Vector3(x, y, z);
                if (y > maxTerrainHeight) { maxTerrainHeight = y; }
                if (y < minTerrainHeight) { minTerrainHeight = y; }
                i++;
            }
        }


        Debug.Log(maxTerrainHeight+" "+ minTerrainHeight);
        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;
                vert++;
                tris += 6;
            }
            vert++;

            yield return new WaitForSeconds(0.01f);
        }
     

        mesh.RecalculateBounds();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        yield return new WaitForSeconds(0.01f);
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        VertexColors = new Color[verticies.Length];
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, verticies[i].y);
                VertexColors[i] = gradient.Evaluate(height);
                i++;
            }
        }
         mesh.colors = VertexColors;

        transform.localScale = new Vector3(20,4, 20);

    }

    private void OnDrawGizmos()
    {
       /* if (verticies == null) {
            return;
        }
        for (int i = 0; i < verticies.Length; i++)
        {
            Gizmos.DrawSphere(verticies[i], 0.1f);
        }*/
    }

    private void CreateQuad()
    {
        verticies = new Vector3[] {
        new Vector3(0,0,0),
        new Vector3(0,0,1),
        new Vector3(1,0,0),
        new Vector3(1,0,1)
        };

        triangles = new int[]{
          0,1,2,
          1,3,2
        };
    }

    private void Update()
    {
        UpdateMesh();
    }
}
