using UnityEngine;

[AddComponentMenu("Tools/Team")]
public class Team : MonoBehaviour
{
  [Tooltip("Enemy teams for this team.")]
  [SerializeField]
  private GameObject[] enemyTeams = null;

  public Transform FindNearEnemy(Vector3 position, float maxDistance)
  {
    var minDistanceSqr = maxDistance * maxDistance;
    Transform result = null;
    foreach (var team in enemyTeams)
      TransformTools.FindNearChildToPos(team.transform, position, ref minDistanceSqr, ref result);

    return result;
  }
}
