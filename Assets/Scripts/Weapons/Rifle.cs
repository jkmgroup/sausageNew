using System.Collections;
using UnityEngine;

[AddComponentMenu("Weapons/Rifle")]
public class Rifle : MonoBehaviour
{
  [Tooltip("Fire at end of barrel.")]
  [SerializeField]
  private GameObject fire = null;

  [Tooltip("Bullet prefab.")]
  [SerializeField]
  private GameObject shotPrefab= null;

  [Tooltip("Start bullet disatance from rifle.")]
  [SerializeField]
  private float startShotDistance = 0.75f;

  [Tooltip("Shot fly speed.")]
  [SerializeField]
  private float shotSpeed = 100.0f;

  [SerializeField]
  private float delayBetweenShots = 5.0f;

  [SerializeField]
  private float hitPower = 5.0f;

  private float _timeConunter = 0.0f;

  void Start()
  {
    _timeConunter = delayBetweenShots;
  }

  void Update()
  {
    if (_timeConunter < delayBetweenShots)
      _timeConunter += Time.deltaTime;     
  }

  public void Shot()
  {
    if (_timeConunter < delayBetweenShots)
      return;
    _timeConunter = 0.0f;
    var maxDistance = 50.0f;
    RaycastHit hit;
    if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
    {
      hit.collider.SendMessage("Damage", hitPower, SendMessageOptions.DontRequireReceiver);
      maxDistance = hit.distance;
    }
    StartCoroutine(ShowShot(maxDistance));
    StartCoroutine(ShowFire());
  }

  private IEnumerator ShowShot(float maxDistance)
  {
    var distance = 0.0f;
    var shot = Instantiate(shotPrefab, transform.position + transform.forward * startShotDistance, transform.rotation);
    yield return null;
    while (distance < maxDistance)
    {
      distance += Time.deltaTime * shotSpeed;
      shot.transform.position += shot.transform.forward * Time.deltaTime * shotSpeed;
      yield return new WaitForSeconds(0.02f);
    }
    Destroy(shot);
  }

  private IEnumerator ShowFire()
  {
    fire.SetActive(true);
    yield return new WaitForSeconds(0.25f);
    fire.SetActive(false);
  }

  [ExecuteInEditMode]
  private void OnValidate()
  {
    if (shotSpeed <= 0)
    {
      Debug.LogWarning("shotSpeed in Rifle (" + name + ") must be more then 0.0f. Value was changed to 10.0f!");
      shotSpeed = 10.0f;
    }
    if (delayBetweenShots < 0)
    {
      Debug.LogWarning("delayBetweenShots in Rifle (" + name + ") must be at lasr 0.0f. Value was changed to 0.0f!");
      delayBetweenShots = 0.0f;
    }
    if (hitPower <= 0)
    {
      Debug.LogWarning("hitPower in Rifle (" + name + ") must be more then 0.0f. Value was changed to 1.0f!");
      hitPower = 1.0f;
    }
  }
}
