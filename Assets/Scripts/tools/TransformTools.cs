using UnityEngine;

public class TransformTools
{
  static public bool FindNearChildToPos(Transform parent, Vector3 position, ref float distanceSqr, ref Transform result)
  {
    var wasFound = false;
    for (var i = 0; i < parent.childCount; ++i)
    {
      var enemyTransform = parent.GetChild(i);
      var dis = Vector3.SqrMagnitude(position - enemyTransform.position);
      if (dis < distanceSqr)
      {
        distanceSqr = dis;
        result = enemyTransform;
        wasFound = true;
      }
    }
    return wasFound;
  }
}