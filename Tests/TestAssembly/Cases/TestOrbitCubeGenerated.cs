using UnityEngine;

public class OrbitingCube : MonoBehaviour
{
    public float orbitRadius = 5f;
    public float orbitSpeed = 30f;
    
    private GameObject sphere;
    private GameObject cube;
    private float currentAngle = 0f;
    
    void Start()
    {
        CreateSphere();
        CreateCube();
    }
    
    void CreateSphere()
    {
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = Vector3.zero;
        sphere.transform.localScale = Vector3.one * 2f;
        
        Material sphereMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        sphereMaterial.color = Color.blue;
        sphere.GetComponent<Renderer>().material = sphereMaterial;
    }
    
    void CreateCube()
    {
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = Vector3.one * 0.5f;
        
        Material cubeMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        cubeMaterial.color = Color.red;
        cube.GetComponent<Renderer>().material = cubeMaterial;
    }
    
    void Update()
    {
        if (cube != null && sphere != null)
        {
            currentAngle += orbitSpeed * Time.deltaTime;
            
            float x = sphere.transform.position.x + Mathf.Cos(currentAngle * Mathf.Deg2Rad) * orbitRadius;
            float z = sphere.transform.position.z + Mathf.Sin(currentAngle * Mathf.Deg2Rad) * orbitRadius;
            
            cube.transform.position = new Vector3(x, sphere.transform.position.y, z);
        }
    }
}