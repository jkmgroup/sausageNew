using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[AddComponentMenu("Enemies/Fat Sausage")]
[RequireComponent(typeof(NavMeshAgent))]
public class FatSausage : MonoBehaviour
{
  private readonly static float maxDistanceToTarget = 1000.0f;
  private readonly static float showWeaponTime = 0.4f;
  enum State{ Stand, GoToTarget, Attack };

  [Tooltip("Bonus after the warrior's death.")]
  [SerializeField]
  private GameObject bonusPrefab = null;

  [Tooltip("Weapon of a warrior.")]
  [SerializeField]
  private GameObject weapon = null;

  [Tooltip("Maximum life value.")]
  [SerializeField]
  public float maxLife = 100.0f;

  [Tooltip("Maximum distance to attack.")]
  [SerializeField]
  private float distanceToAttack = 2.0f;

  [Tooltip("Delay between atack.")]
  [SerializeField]
  private float delayToNextHit = 1.0f;

  [Tooltip("Hit power value.")]
  [SerializeField]
  private float hitPower = 1.0f;


  private float _life = 0.0f;
  private float _distanceToAttackSqr;
  private NavMeshAgent _agent;
  private State _state = State.Stand;
  private Team _team = null;
  private Transform _target = null;
  private float _attackTimeCounter = 0;
  private ProgressBar _lifeBar;

  // Use this for initialization
  void Start()
  {
    _team = GetComponentInParent<Team>();
    if (!_team)
    {
      Debug.LogError("Warrior (" + name + ") must have parent with Team component. Warrior will be destroy.");
      Destroy(gameObject);
      return;
    }

    if (!weapon)
    {
      Debug.LogError("Warrior (" + name + ") didnt have weapon. Warrior will be destroy.");
      Destroy(gameObject);
      return;
    }
    weapon.SetActive(false);
    _distanceToAttackSqr = distanceToAttack * distanceToAttack;
    _agent = GetComponent<NavMeshAgent>();
    _lifeBar = GetComponentInChildren<ProgressBar>();
    _life = maxLife;
  }

  void Update()
  {
    if (!_target)
    {
      _target = _team.FindNearEnemy(transform.position, maxDistanceToTarget);
      if (!_target)
        return;
    }
    if (_attackTimeCounter < delayToNextHit)
      _attackTimeCounter += Time.deltaTime;

    if (CanAttack())
    {
      if (_state != State.Attack)
        StartStateAttack();
      UpdateStateAttack();
    }
    else
    {
      if (_agent.SetDestination(_target.position)) 
      {
        if (_state != State.GoToTarget)
          StartStateGoToTarget();
      }
      else
      {
        if (_state != State.Stand)
          StartStateStand();
        UpdateStateStand();
      }
    }
  }

  private bool CanAttack()
  {
    var distance = Vector2.SqrMagnitude(new Vector2(transform.position.x - _target.position.x, transform.position.z - _target.position.z));
    if (distance > _distanceToAttackSqr)
      return false;

    RaycastHit hit;
    if (Physics.Raycast(transform.position, transform.forward, out hit, distance))
      return hit.collider.tag == "Player";
    return true;
  }

  public void Damage(float damage)
  {
    _life -= damage;
    if (_life <= 0)
    {
      if (bonusPrefab)
        Instantiate<GameObject>(bonusPrefab, transform.position, Quaternion.identity, _team.transform.parent);
      Destroy(gameObject);
      return;
    }
    if (_lifeBar)
      _lifeBar.SetPercent(_life / maxLife);
  }

  private void StartStateAttack()
  {
    _state = State.Attack;
    _agent.isStopped = true;
  }

  private void UpdateStateAttack()
  {
    LookOnTarget();
    if (_attackTimeCounter > delayToNextHit)
    {
      StartCoroutine(HitTarget());
      _attackTimeCounter %= delayToNextHit;
    }
  }

  private void LookOnTarget()
  {
    transform.LookAt(new Vector3(_target.transform.position.x, transform.position.y, _target.transform.position.z));
  }

  private void StartStateGoToTarget()
  {
    _state = State.GoToTarget;
    _agent.isStopped = false;
  }

  private void StartStateStand()
  {
    _state = State.Stand;
    _agent.isStopped = true;
  }

  private void UpdateStateStand()
  {
    LookOnTarget();
  }

  private IEnumerator HitTarget()
  {
    weapon.SetActive(true);
    RaycastHit hit;
    if (Physics.Raycast(weapon.transform.position, weapon.transform.forward, out hit, 1.0f))
      hit.collider.SendMessage("Damage", hitPower, SendMessageOptions.DontRequireReceiver);
    yield return new WaitForSeconds(showWeaponTime);
    weapon.SetActive(false);
  }

  [ExecuteInEditMode]
  private void OnValidate()
  {
    if (!weapon)
    {
      Debug.LogError("weapon in warrior (" + name + ") can be null!");
      Debug.Break();
    }
    if (maxLife <= 0)
    {
      Debug.LogWarning("maxLife in warrior (" + name + ") must be more then 0.0f. Value was changed to 1.0f!");
      maxLife = 1.0f;
    }
    if (hitPower < 0.0f)
    {
      Debug.LogWarning("hitPower in warrior (" + name + ") can be less then 0.0f. Value was changed to 1.0f!");
      hitPower = 1.0f;
    }
    var minTimeToDelay = showWeaponTime * 1.1f; 
    if (delayToNextHit < minTimeToDelay)
    {
      Debug.LogWarning("delayToNextHit in warrior (" + name + ") can be less then " + minTimeToDelay + ". Value was changed to " + minTimeToDelay + "!");
      delayToNextHit = minTimeToDelay;
    }
    if (distanceToAttack < 1.0f) 
    {
      Debug.LogWarning("distanceToAttack in warrior (" + name + ") can be less then 1.0f. Value was changed to 1.0f!");
      distanceToAttack = 1.0f;
    }
    _distanceToAttackSqr = distanceToAttack * distanceToAttack;
  }
}
