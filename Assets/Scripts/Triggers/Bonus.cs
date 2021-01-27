using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("Triggers/Bonus")]
public class Bonus : MonoBehaviour
{
  private void OnTriggerEnter(Collider other)
  {
    if (other.tag != "Player")
      return;
    other.gameObject.SendMessage("AddBonus", name);
    Destroy(gameObject);
  }
}
