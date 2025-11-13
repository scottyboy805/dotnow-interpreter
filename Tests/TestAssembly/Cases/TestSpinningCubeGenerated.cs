using UnityEngine;

public class SpinningCube : MonoBehaviour
{
    public float rotationSpeed = 90f;
    
    void Start()
    {
        if (GetComponent<MeshRenderer>() == null)
        {
            gameObject.AddComponent<MeshRenderer>();
            gameObject.AddComponent<MeshFilter>();
            GetComponent<MeshFilter>().mesh = CreateCubeMesh();
            
            Material cubeMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            cubeMaterial.color = Color.cyan;
            GetComponent<MeshRenderer>().material = cubeMaterial;
        }
    }
    
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.right, rotationSpeed * 0.5f * Time.deltaTime);
    }
    
    Mesh CreateCubeMesh()
    {
        Mesh mesh = new Mesh();
        
        Vector3[] vertices = {
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f)
        };
        
        int[] triangles = {
            0, 2, 1, 0, 3, 2,
            1, 6, 5, 1, 2, 6,
            5, 7, 4, 5, 6, 7,
            4, 3, 0, 4, 7, 3,
            3, 6, 2, 3, 7, 6,
            4, 1, 5, 4, 0, 1
        };
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        
        return mesh;
    }
}
