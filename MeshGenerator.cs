using UnityEngine;

public class MeshGenerator : MonoBehaviour {

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Color[] colors;
    

    public float heightMultiplier;

    public int xSize = 20;
    public int zSize = 20;

    public float scale;
    private float amplitude, frequency, noiseHeight;

    public int octaves;
    private int initialOctaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;
    private Vector2[] offsets;

    private float minHeight, maxHeight;

    public Gradient color;

    private void Start() {
        //sets initialOctaves to octaves, initializes offsets as a vector2 array of octaves length
        initialOctaves = octaves;
        offsets = new Vector2[octaves];
        for (int o = 0; o < octaves; o++) {
            //loops through octaves and gives a random value for each offset
            float offsetH = Random.Range(-10000, 10000);
            float offsetW = Random.Range(-10000, 10000);

            offsets[o] = new Vector2(offsetW, offsetH);
        }

        //initializes a mesh and equals the mesh filter of the object the scripts is attached to our
        //new mesh
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    void CreateShape() {
        //initializes vertices as a vector3 array of xSize + 1 multiplied by zSize + 1, which is the total
        //amount of vertices in the mesh
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];


        for (int i = 0, z = 0; z <= zSize; z++) {
            for (int x = 0; x <= xSize; x++, i++) {
                //gets the noise height value for each vertex and multiplies it by heightMultiplier since otherwise
                //the values would always be between 0 and 1
                float y = CalculateHeight(x, z) * heightMultiplier;
                //sets each element of the vertices array position 
                vertices[i] = new Vector3(x, y, z);

                //this is used to store the highest and lowest value of height of our mesh vertices 
                if(y > maxHeight) {
                    maxHeight = y;
                }
                if (y < minHeight) {
                    minHeight = y;
                }
            }
        }

        //sets triangles to an array of xSize * zSize * 6 being 6 the amount of vertex needed to create a quad
        triangles = new int[xSize * zSize * 6];
        

        for (int z = 0, vert = 0, tris = 0; z < zSize; z++) {
            for (int x = 0; x < xSize; x++) {
                //equals the points of the triangles on the quad to the mesh vertices
                triangles[tris] = vert;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                //increases vertex by one and adds 6 to tris
                vert++;
                tris += 6;
            }
            //increases vertex
            vert++;
        }

        //initializes colors array as a color array of vertices length
        colors = new Color[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++) {
            for (int x = 0; x <= xSize; x++, i++) {
                //gets the value of each vertices height between 0 and 1 and equals colors to
                //the corresponding color of in the gradient
                float height = Mathf.InverseLerp(minHeight, maxHeight, vertices[i].y);
                colors[i] = color.Evaluate(height);
            }
        }
    }

    void UpdateMesh() {
        //clears data of the previous mesh
        mesh.Clear();

        //sets mesh vertices, triangles and colors to our vertices, triangles and colors
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        //recalculates normals for light to work properly on our mesh
        mesh.RecalculateNormals();
    }

    private void Update() {
        //if initialOctaves is not equal to octaves generates new offsets looping through
        //the new octaves value and sets initialOctaves to octaves
        if (initialOctaves != octaves) {
            offsets = new Vector2[octaves];
            for (int o = 0; o < octaves; o++) {
                float offsetH = Random.Range(-10000, 10000);
                float offsetW = Random.Range(-10000, 10000);

                offsets[o] = new Vector2(offsetW, offsetH);
            }
            initialOctaves = octaves;
        }

        CreateShape();
        UpdateMesh();
    }

    float CalculateHeight(int w, int h) {

        //range where the result can be in
        amplitude = 1;
        //period at which data is sampled
        frequency = 1;

        noiseHeight = 0;

        for (int o = 0; o < octaves; o++) {
            //creates local variables to use on perlin noise method being both indexes passed to the function
            //minus half the width or height divided by our noise scale, multiplied by the frequency and
            //plus the corresponding offset
            float x = (w - (xSize / 2)) / scale * frequency + offsets[o].x;
            float y = (h - (zSize / 2)) / scale * frequency + offsets[o].y;

            //calls perlin noise using the local variables above and stores it in result
            float result = Mathf.PerlinNoise(x, y);

            //sets noise height as noise height plus result multiplied by amplitude
            noiseHeight += result * amplitude;

            //amplitude is decreased by multiplying the current amplitude by persistance
            amplitude *= persistance;

            //frequency is increased by multiplying the current frequency by lacunarity
            frequency *= lacunarity;

        }

        //returns noiseHeight
        return noiseHeight;
    }
}
