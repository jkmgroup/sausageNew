using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("Triggers/Trap")]
public class Trap : MonoBehaviour
{
  [Tooltip("The destruction that this trap is doing.")]
  [SerializeField]
  private float damagePower = 20.0f;
  private void OnTriggerEnter(Collider other)
  {
    other.SendMessage("Damage", damagePower);
    Destroy(gameObject);
  }

  [ExecuteInEditMode]
  private void OnValidate()
  {
    if (damagePower <= 0)
    {
      Debug.LogWarning("damagePower in Trap (" + name + ") must be grater then 0.0f. Value was changed to 10.0f!");
      damagePower = 10.0f;
    };
  }
}
