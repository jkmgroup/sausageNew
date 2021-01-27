using UnityEngine;

[AddComponentMenu("Triggers/Escape Rocket")]
public class EscapeRocket : MonoBehaviour
{
  [SerializeField]
  private GameObject levelManager = null;

  [SerializeField]
  private float rockertSpeed = 50.0f;

  private bool _enginOn = false;
  private void Start()
  {
    if (!levelManager)
    {
      Debug.LogError("EndGamePlatform (" + name + ") must have levelManager set!");
      Debug.Break();
    }
  }
  private void OnTriggerEnter(Collider other)
  {
    if (_enginOn)
      return;
    if (other.tag != "Player")
      return;
    levelManager.SendMessage("PlayersMessage", "PlayersWon");
    other.transform.parent = transform.parent;
    Destroy(other.GetComponent<PlayerMovement>());
    _enginOn = true;
  }

  private void Update()
  {
    if (!_enginOn)
      return;
    transform.parent.position += Vector3.up * Time.deltaTime * rockertSpeed;
  }
}
