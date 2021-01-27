using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("Game/Level Manager")]
public class LevelManager : MonoBehaviour
{
  [Tooltip("Escape rocket shows after collect all bonus.")]
  [SerializeField]
  GameObject escapeRocket = null;

  [Tooltip("Window with message after players death.")]
  [SerializeField]
  GameObject deahWindowPrefab = null;

  [Tooltip("Window with message after players won.")]
  [SerializeField]
  GameObject wonWindowPrefab = null;

  [Tooltip("Show message window time.")]
  [SerializeField]
  float showWindowTime = 15.0f;

  public void PlayersMessage(string message)
  {
    if (message == "PlayersDeath")
      StartCoroutine(PlayersDeath());
    else
    if (message == "AllBonus")
      escapeRocket.SetActive(true);
    else
    if (message == "PlayersWon")
      StartCoroutine(PlayersWon());
  }

  private IEnumerator PlayersDeath()
  {
    Instantiate(deahWindowPrefab);
    yield return new WaitForSeconds(showWindowTime);
    SceneManager.LoadScene("Menu");
  }

  private IEnumerator PlayersWon()
  {
    Instantiate(wonWindowPrefab);
    yield return new WaitForSeconds(showWindowTime);
    SceneManager.LoadScene("Menu");
  }
}
