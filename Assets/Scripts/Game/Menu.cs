using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("Game/Menu")]
public class Menu : MonoBehaviour
{
  private void Awake()
  {
    Debug.Log("Menu awake.");
  }
  private void Start()
  {
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
  }
  public void LoadLevel(string name)
  {
    SceneManager.LoadScene(name);
  }
}
