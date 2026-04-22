using UnityEngine;

[ExecuteInEditMode]
public class InvertSphere : MonoBehaviour
{
    // A simple toggle so you can flip it back if needed
    public bool isInverted = false;

    void Awake()
    {
        if (!isInverted)
        {
            Invert();
        }
    }

    [ContextMenu("Force Invert")] // Allows you to right-click the component to run this
    public void Invert()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        if (filter != null && filter.sharedMesh != null)
        {
            // Use sharedMesh to avoid the "leak" warning
            Mesh mesh = filter.sharedMesh;
            
            Vector3[] normals = mesh.normals;
            for (int i = 0; i < normals.Length; i++)
                normals[i] = -normals[i];
            
            mesh.normals = normals;

            for (int m = 0; m < mesh.subMeshCount; m++)
            {
                int[] triangles = mesh.GetTriangles(m);
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    int temp = triangles[i + 0];
                    triangles[i + 0] = triangles[i + 1];
                    triangles[i + 1] = temp;
                }
                mesh.SetTriangles(triangles, m);
            }
            
            isInverted = true;
            Debug.Log("Sky Sphere Inverted Successfully.");
        }
    }
}