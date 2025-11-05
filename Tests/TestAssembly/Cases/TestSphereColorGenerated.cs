using UnityEngine;

public class ColorChangingSphere : MonoBehaviour
{
    private Renderer sphereRenderer;
    private Material sphereMaterial;
    private Color[] availableColors = { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta, Color.cyan, Color.white };
    private int currentColorIndex = 0;

    void Start()
    {
        CreateSphere();
        SetupMaterial();
    }

    void CreateSphere()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.SetParent(transform);
        sphere.transform.localPosition = Vector3.zero;
        sphereRenderer = sphere.GetComponent<Renderer>();
    }

    void SetupMaterial()
    {
        sphereMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        sphereMaterial.color = availableColors[currentColorIndex];
        sphereRenderer.material = sphereMaterial;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckForClick();
        }
    }

    void CheckForClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.transform.IsChildOf(transform))
            {
                ChangeColor();
            }
        }
    }

    void ChangeColor()
    {
        currentColorIndex = (currentColorIndex + 1) % availableColors.Length;
        sphereMaterial.color = availableColors[currentColorIndex];
    }
}