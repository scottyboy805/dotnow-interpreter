using UnityEngine;

public class CubeController : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    private void Start()
    {
        if (GetComponent<MeshRenderer>() == null)
        {
            gameObject.AddComponent<MeshRenderer>();
            gameObject.AddComponent<MeshFilter>();
            GetComponent<MeshFilter>().mesh = CreateCubeMesh();
            
            Material cubeMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            cubeMaterial.color = Color.blue;
            GetComponent<MeshRenderer>().material = cubeMaterial;
        }
    }
    
    private void Update()
    {
        Vector3 movement = Vector3.zero;
        
        if (Input.GetKey(KeyCode.UpArrow))
            movement += Vector3.forward;
        if (Input.GetKey(KeyCode.DownArrow))
            movement += Vector3.back;
        if (Input.GetKey(KeyCode.LeftArrow))
            movement += Vector3.left;
        if (Input.GetKey(KeyCode.RightArrow))
            movement += Vector3.right;
        
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
    
    private Mesh CreateCubeMesh()
    {
        Mesh mesh = new Mesh();
        
        Vector3[] vertices = {
            new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(-0.5f, -0.5f, 0.5f)
        };
        
        int[] triangles = {
            0, 2, 1, 0, 3, 2, 2, 3, 4, 2, 4, 5, 1, 2, 5, 1, 5, 6,
            0, 7, 4, 0, 4, 3, 5, 4, 7, 5, 7, 6, 0, 6, 7, 0, 1, 6
        };
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        
        return mesh;
    }
}