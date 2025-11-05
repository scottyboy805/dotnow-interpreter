using UnityEngine;

public class CubeCarController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 100f;
    
    private Rigidbody carRigidbody;
    
    void Start()
    {
        SetupCube();
        SetupRigidbody();
    }
    
    void SetupCube()
    {
        if (GetComponent<MeshRenderer>() == null)
        {
            gameObject.AddComponent<MeshRenderer>();
            gameObject.AddComponent<MeshFilter>();
            GetComponent<MeshFilter>().mesh = CreateCubeMesh();
            
            Material cubeMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            cubeMaterial.color = Color.red;
            GetComponent<MeshRenderer>().material = cubeMaterial;
        }
        
        if (GetComponent<BoxCollider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }
    }
    
    void SetupRigidbody()
    {
        carRigidbody = GetComponent<Rigidbody>();
        if (carRigidbody == null)
        {
            carRigidbody = gameObject.AddComponent<Rigidbody>();
        }
        carRigidbody.mass = 1f;
        carRigidbody.linearDamping = 2f;
        carRigidbody.angularDamping = 5f;
		carRigidbody.useGravity = false;
    }
    
    Mesh CreateCubeMesh()
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
    
    void Update()
    {
        HandleInput();
    }
    
    void HandleInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f);
        
        if (moveDirection.magnitude > 0.1f)
        {
            carRigidbody.AddForce(moveDirection * moveSpeed, ForceMode.Force);
            
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}