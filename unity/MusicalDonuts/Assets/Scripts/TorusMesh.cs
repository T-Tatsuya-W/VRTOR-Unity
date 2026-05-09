using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TorusMesh : MonoBehaviour
{
    public float majRadius = 1f;
    public float minorRadius = 0.5f;
    public int numSegments = 1;

    void Start()
    {
        GenerateTorus();
    }

    void GenerateTorus()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int radialSegments = numSegments;
        int tubularSegments = numSegments;

        int numVertices = (radialSegments + 1) * (tubularSegments + 1);
        Vector3[] vertices = new Vector3[numVertices];
        Vector3[] normals = new Vector3[numVertices];
        Vector2[] uvs = new Vector2[numVertices];
        int[] triangles = new int[radialSegments * tubularSegments * 6];

        int vertexIndex = 0;
        for (int i = 0; i <= radialSegments; i++)
        {
            float theta = (float)i / radialSegments * 2f * Mathf.PI;
            for (int j = 0; j <= tubularSegments; j++)
            {
                float phi = (float)j / tubularSegments * 2f * Mathf.PI;

                float x = (majRadius + minorRadius * Mathf.Cos(theta)) * Mathf.Cos(phi);
                float z = (majRadius + minorRadius * Mathf.Cos(theta)) * Mathf.Sin(phi);
                float y = minorRadius * Mathf.Sin(theta);

                vertices[vertexIndex] = new Vector3(x, y, z);

                // Normal calculation
                Vector3 center = new Vector3(majRadius * Mathf.Cos(phi), majRadius * Mathf.Sin(phi), 0f);
                normals[vertexIndex] = (vertices[vertexIndex] - center).normalized;

                uvs[vertexIndex] = new Vector2((float)j / tubularSegments, (float)i / radialSegments);

                vertexIndex++;
            }
        }

        int triangleIndex = 0;
        for (int i = 0; i < radialSegments; i++)
        {
            for (int j = 0; j < tubularSegments; j++)
            {
                int a = i * (tubularSegments + 1) + j;
                int b = i * (tubularSegments + 1) + j + 1;
                int c = (i + 1) * (tubularSegments + 1) + j;
                int d = (i + 1) * (tubularSegments + 1) + j + 1;

                triangles[triangleIndex++] = a;
                triangles[triangleIndex++] = c;
                triangles[triangleIndex++] = b;

                triangles[triangleIndex++] = b;
                triangles[triangleIndex++] = c;
                triangles[triangleIndex++] = d;
            }
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
    }
}
