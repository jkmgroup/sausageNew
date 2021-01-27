using UnityEngine;

[AddComponentMenu("Tools/Progrss bar")]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ProgressBar : MonoBehaviour
{
  [Tooltip("Material to use.")]
  [SerializeField]
  private Material material = null;

  [Tooltip("Size (width, height)")]
  [SerializeField]
  private Vector2 size = new Vector2(0.3f, 0.2f);

  private Mesh _mesh;

  private void Start()
  {
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
        return;
      GetComponent<MeshRenderer>().material = material;
    }
    _mesh.Clear();
    _mesh.vertices = CreateMeshVertices(1.0f);
    _mesh.triangles = CreateMeshTriangles();
    _mesh.uv = CreateMeshUVs();
  }

  private Vector3[] CreateMeshVertices(float percent)
  {
    var vec = new Vector3[6];
    vec[0] = new Vector3(-size.x, -size.y, 0.0f);
    vec[1] = new Vector3(-size.x, size.y, 0.0f);

    vec[2] = new Vector3(size.x - (percent * size.x * 2.0f), -size.y, 0.0f);
    vec[3] = new Vector3(size.x - (percent * size.x * 2.0f), size.y, 0.0f);

    vec[4] = new Vector3(size.x, -size.y, 0.0f);
    vec[5] = new Vector3(size.x, size.y, 0.0f);
    return vec;
  }

  private Vector2[] CreateMeshUVs()
  {
    var uv = new Vector2[6];
    uv[0] = new Vector2(1.0f, 0.0f);
    uv[1] = new Vector2(1.0f, 1.0f);

    uv[2] = new Vector2(0.5f, 0.0f);
    uv[3] = new Vector2(0.5f, 1.0f);

    uv[4] = new Vector2(0.0f, 0.0f);
    uv[5] = new Vector2(0.0f, 1.0f);
    return uv;
  }

  private int[] CreateMeshTriangles()
  {
    var tri = new int[4 * 3];
    tri[0 + 0] = 0;
    tri[0 + 1] = 1;
    tri[0 + 2] = 2;

    tri[0 + 3] = 1;
    tri[0 + 4] = 3;
    tri[0 + 5] = 2;

    tri[6 + 0] = 2;
    tri[6 + 1] = 3;
    tri[6 + 2] = 4;

    tri[6 + 3] = 3;
    tri[6 + 4] = 5;
    tri[6 + 5] = 4;
    return tri;
  }

  public void SetPercent(float percent)
  {
    _mesh.vertices = CreateMeshVertices(percent);
  }

  void Update ()
  {
		transform.rotation = Camera.main.transform.rotation;
	}

  [ExecuteInEditMode]
  private void OnValidate()
  {
    if (size.x <= 0.0f)
    {
      Debug.LogWarning("size.x in ProgressBar (" + name + ") must be grater then 0. Value was changed to 0.2!");
      size.x = 0.2f;
    }
    if (size.y <= 0.0f)
    {
      Debug.LogWarning("size.y in ProgressBar (" + name + ") must be grater then 0. Value was changed to 0.1!");
      size.y = 0.1f;
    }
//    CreateMesh();
  }
}
