using UnityEngine;

[AddComponentMenu("Triggers/Kill Zone")]
public class KillZone : MonoBehaviour
{
  private void OnTriggerEnter(Collider other)
  {
    other.SendMessage("Damage", 100000, SendMessageOptions.DontRequireReceiver);
    Debug.Log("Kill zone " + other.name);
  }
}
