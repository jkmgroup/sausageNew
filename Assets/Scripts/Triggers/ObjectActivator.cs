using UnityEngine;

[AddComponentMenu("Triggers/Object activator")]
public class ObjectActivator : MonoBehaviour
{
  [Tooltip("Object to activate when Player enter.")]
  [SerializeField]
  private GameObject[] objectsToActivate = null;

  [Tooltip("Activate only once and destroy this object.")]
  [SerializeField]
  private bool activateOnlyOnce = true;

  private void Start()
  {
    if (objectsToActivate.Length==0)
    {
      Debug.LogError("Field objectsToActivate must have at last 1 element ObjectActivator (" + name + ")! Object was destroy");
      Destroy(gameObject);
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.tag != "Player")
      return;
    foreach (var obj in objectsToActivate)
      obj.SetActive(true);

    if (activateOnlyOnce)
      Destroy(gameObject);
  }

  [ExecuteInEditMode]
  private void OnDrawGizmos()
  {
    Gizmos.color = new Color(0.0f, 0.0f, 1.0f, 0.5f);
    Gizmos.DrawCube(transform.position, transform.localScale);
#if UNITY_EDITOR 
    UnityEditor.Handles.color = Color.green;
    UnityEditor.Handles.Label(transform.position + Vector3.up * (transform.localScale.y + 0.5f), name);
#endif
  }
}
