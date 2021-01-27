using UnityEngine;

[AddComponentMenu("Extended meshes/Plane mesh")]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class MeshPlane : MonoBehaviour
{
  [Tooltip("Size (columns, rows)")]
  [SerializeField]
  private Vector2Int size = new Vector2Int(100, 100);

  private Mesh _mesh;
  private void Start()
  {
    var meshFilter = GetComponent<MeshFilter>();
    _mesh = meshFilter.sharedMesh;
    CreateMesh();
  }

  private void CreateMesh()
  {
    if (!_mesh)
    {
      var meshFilter = GetComponent<MeshFilter>();
      if (meshFilter.sharedMesh)
        _mesh = meshFilter.sharedMesh;
      else
      if (meshFilter.mesh)
        _mesh = meshFilter.mesh;
      else
      {
        Debug.LogError("No mesh in MeshPlane (" + name + "). Componet while be destroy.");
        Destroy(this);
        return;
      };
    }
    _mesh.Clear();
    _mesh.vertices = CreateMeshVertices(1.0f);
    _mesh.triangles = CreateMeshTriangles();
    _mesh.uv = CreateMeshUVs();
  }

  private Vector3[] CreateMeshVertices(float percent)
  {
    var pos = new Vector3(-0.5f, 0.0f, -0.5f);
    var step = new Vector2(1.0f / (size.x-1), 1.0f / (size.y-1));
    var vec = new Vector3[size.x * size.y];
    for (var j = 0; j < size.y; ++j)
    {
      pos.x = -0.5f;
      for (var i = 0; i < size.x; ++i)
      {
        vec[i + j * size.x] = pos;
        pos.x += step.x;
      }
      pos.z += step.y;
    }
    return vec;
  }

  private Vector2[] CreateMeshUVs()
  {
    var pos = new Vector2(0.0f, 0.0f);
    var step = new Vector2(1.0f / (size.x - 1), 1.0f / (size.y - 1));
    var uv = new Vector2[size.x * size.y];
    for (var j = 0; j < size.y; ++j)
    {
      pos.x = 0.0f;
      for (var i = 0; i < size.x; ++i)
      {
        uv[i + j * size.x] = pos;
        pos.x += step.x;
      }
      pos.y += step.y;
    }
    return uv;
  }

  private int[] CreateMeshTriangles()
  {
    var numTri = 0;
    var tri = new int[(size.x - 1) * (size.y - 1) * 6];
    for (var j = 0; j < size.y - 1; ++j)
      for (var i = 0; i < size.x - 1; ++i)
      {
        tri[numTri + 2] = i + j * size.x;
        tri[numTri + 1] = i + 1 + j * size.x;
        tri[numTri + 0] = i + 1 + (j + 1) * size.x;

        tri[numTri + 3] = tri[numTri + 2];
        tri[numTri + 4] = i + (j + 1) * size.x;
        tri[numTri + 5] = tri[numTri + 0];
        numTri += 6;
      }
    return tri;
  }

  [ExecuteInEditMode]
  private void OnValidate()
  {
    if (size.x < 2)
    {
      Debug.LogWarning("size.x in MeshPlane (" + name + ") must be grater then 1. Value was changed to 2!");
      size.x = 2;
    }
    if (size.y < 2)
    {
      Debug.LogWarning("size.y in MeshPlane (" + name + ") must be grater then 1. Value was changed to 2!");
      size.y = 2;
    }
    CreateMesh();
  }
}
