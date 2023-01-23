using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostDirection : MonoBehaviour
{
    public GhostTargetFinder targetFinder;
    public LayerMask mask;
    Vector3 step = Vector3.zero;


    public Vector3 MoveInDirection(Vector3 dirction)
    {
        return transform.position + (dirction * 3);
    }

    public Vector3 FindNextDirection()
    {
        if (targetFinder.Target() != transform.position)
            step = FindShortestDirectionToTarget(PossibleDirections(), targetFinder.Target());
        else
            step = RandomDirection(PossibleDirections());

        return step;
    }

    List<Vector3> PossibleDirections()
    {
        var possibleDirections = new List<Vector3>();

        if (DirCheck(Vector3.forward))
            possibleDirections.Add(Vector3.forward);

        if (DirCheck(Vector3.left))
            possibleDirections.Add(Vector3.left);

        if (DirCheck(Vector3.back))
            possibleDirections.Add(Vector3.back);

        if (DirCheck(Vector3.right))
            possibleDirections.Add(Vector3.right);

        return possibleDirections;
    }

    Vector3 FindShortestDirectionToTarget(List<Vector3> possibilisies, Vector3 target)
    {
        if (possibilisies.Count == 0)
            return Vector3.zero;
        Vector3 shortestDirection = possibilisies[0];
        foreach (Vector3 item in possibilisies)
        {
            if (Vector3.Distance(MoveInDirection(item), target) < Vector3.Distance(MoveInDirection(shortestDirection), target))
                shortestDirection = item;

            if (Vector3.Distance(MoveInDirection(item) + new Vector3(-54f, 0f, 0f), target) < Vector3.Distance(MoveInDirection(shortestDirection), target))
                shortestDirection = item;
            if (Vector3.Distance(MoveInDirection(item) + new Vector3(54f, 0f, 0f), target) < Vector3.Distance(MoveInDirection(shortestDirection), target))
                shortestDirection = item;
        }
        return shortestDirection;
    }

    RaycastHit HitInfu(Vector3 direction)
    {
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hitInfu;
        Physics.Raycast(ray, out hitInfu, 100f, mask, QueryTriggerInteraction.Ignore);
        return hitInfu;
    }

    bool DirCheck(Vector3 Direction)
    {

        if (Direction != step * -1)
            if (HitInfu(Direction).distance >= 3f)
                return true;
        return false;
    }

    Vector3 RandomDirection(List<Vector3> possibilities)
    {
        int randomNumber = Random.Range(0, possibilities.Count);
        return possibilities[randomNumber];
    }
}