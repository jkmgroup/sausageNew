using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Tools/Target camera")]
public class TargetCamera : MonoBehaviour
{
  [Tooltip("Target to look at.")]
  [SerializeField]
  private Transform target = null;

  [Tooltip("Maximum rotation speed.")]
  [SerializeField]
  private float rotationSpeed = 70.0f;

  [Tooltip("Position under target.")]
  [SerializeField]
  private float posY = 4.0f;

  [Tooltip("Distance from target.")]
  [SerializeField]
  private float distance = 10.0f;

  void Update()
  {
    if (!target)
      return;
    var forward = target.forward;
    forward.y -= posY / distance;
    Quaternion direction = Quaternion.LookRotation(forward);
    transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotationSpeed * Time.deltaTime);
    transform.position = target.position - transform.transform.forward * distance;

    RaycastHit hit;
    if (Physics.Raycast(target.position, -transform.forward, out hit, distance))
      transform.position = target.position - transform.forward * hit.distance;
  }

  [ExecuteInEditMode]
  private void OnValidate()
  {
    if (rotationSpeed <= 0)
    {
      Debug.LogWarning("rotationSpeed in TargetCamera (" + name + ") must be more then 0.0f. Value was changed to 5.0f!");
      rotationSpeed = 5.0f;
    }

    if (distance <= 0)
    {
      Debug.LogWarning("distance in TargetCamera (" + name + ") must be more then 0.0f. Value was changed to 10.0f!");
      distance = 10.0f;
    }
  }
}
