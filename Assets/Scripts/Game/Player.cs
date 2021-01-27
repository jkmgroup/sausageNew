using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/Player")]
public class Player : MonoBehaviour
{
  [Tooltip("LevelManager object. It must have PlayersMessage function to receive message from player.")]
  [SerializeField]
  private GameObject levelManager = null;

  [Tooltip("List of traps that player are equipment.")]
  [SerializeField]
  private GameObject[] traps = null;

  [Tooltip("Starting value of players life.")]
  [SerializeField]
  private float maxLife = 100.0f;

  private int _numUsedTraps = 0;
  private bool _canPutTrap = true;
  private float _life = 100.0f;
  private ProgressBar _lifeBar = null;
  private Rifle _rifle = null;
  private Dictionary<string, short> _bonuses = new Dictionary<string, short>();
  
  void Start()
  {
    if (!levelManager)
    {
      Debug.LogError("Player must have levelManager!");
      Debug.Break();
    }
    _rifle = GetComponentInChildren<Rifle>();
    if (!_rifle)
    {
      Debug.LogError("Player must have _rife!");
      Debug.Break();
    }
    _life = maxLife;
    _lifeBar = GetComponentInChildren<ProgressBar>();
  }

  void Update()
  {
    UpdateWeaposAngle();
    if (Input.GetButton("Fire1"))
      _rifle.Shot();
    if (_canPutTrap && Input.GetButton("Fire2"))
      StartCoroutine(PutTrap());
  }
  private void UpdateWeaposAngle()
  {
    RaycastHit hitInfo = new RaycastHit();
    if (Physics.Raycast(new Ray(transform.position, transform.forward), out hitInfo, 150.0f))
      _rifle.transform.LookAt(hitInfo.point);
    else
      _rifle.transform.localRotation = Quaternion.identity;
  }

  private IEnumerator PutTrap()
  {
    if (_numUsedTraps >= traps.Length)
      yield break;

    while (!traps[_numUsedTraps])
    {
      _numUsedTraps++;
      if (_numUsedTraps >= traps.Length)
        yield break;
    }
    _canPutTrap = false;
    var obj = Instantiate(traps[_numUsedTraps], transform.parent);
    traps[_numUsedTraps++] = null;
    var pos = transform.position + transform.forward * 1.5f;
    pos.y += 10.0f;
    RaycastHit hit;
    if (Physics.Raycast(pos, Vector3.down, out hit))
      obj.transform.position = hit.point + Vector3.up * obj.transform.localScale.y;
    yield return new WaitForSeconds(1.0f);
    _canPutTrap = true;
  }

  public void Damage(float damage)
  {
    _life -= damage;
    if (_life <= 0)
    {
      Destroy(gameObject);
      levelManager.SendMessage("PlayersMessage", "PlayersDeath");
      return;
    }
    if (_lifeBar)
      _lifeBar.SetPercent(_life / maxLife);
  }

  public void AddBonus(string name)
  {
    if (_bonuses.ContainsKey(name))
      _bonuses[name]++;
    else
      _bonuses.Add(name, 1);
    Debug.Log("Bonus was added " + name);
    if (_bonuses.Count == 3)
      levelManager.SendMessage("PlayersMessage", "AllBonus");
  }

  private void OnGUI()
  {
    var width = 100;
    var height = 30;
    var buffer = 10;
    var posX = 10;
    var posY = 10;
    GUI.Box(new Rect(10, posY, width, height), "Traps : " + (traps.Length - _numUsedTraps));
    if (_bonuses.Count == 0)
      return;
    
    posX = -(width * _bonuses.Count + buffer * (_bonuses.Count - 1)) / 2;
    posX += Camera.main.pixelWidth / 2;
 
    foreach (var bonus in _bonuses)
    {
      GUI.Box(new Rect(posX, posY, width, height), bonus.Key);
      posX += width + buffer;
    }
  }

  [ExecuteInEditMode]
  private void OnValidate()
  {
    if (maxLife<=0)
    {
      Debug.LogWarning("maxLife in Player (" + name + ") must be more then 0.0f. Value was changed to 1.0f!");
      maxLife = 1.0f;
    }

    if (!levelManager)
      Debug.LogWarning("levelManager in Player (" + name + ") can be null!");
  }
}
