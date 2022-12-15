using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Targeter : MonoBehaviour
{
    [SerializeField] CinemachineTargetGroup cineTargetGroup;

    List<Target> targets = new List<Target>();

    public Target CurrentTarget { get; private set; }

    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Target>(out Target target))
        {
            if (!targets.Contains(target))
            {
                targets.Add(target);
                target.OnDestroyed += Target_OnDestroyed;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Target>(out Target target))
        {
            RemoveTarget(target);
        }
    }

    public bool SelectTarget()
    {
        if (targets.Count == 0)
            return false;

        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;
        foreach (Target target in targets)
        {
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);

            if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
                continue;

            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);
            float toCenterSqrMagnitude = toCenter.sqrMagnitude;
            if (toCenterSqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = toCenterSqrMagnitude;
            }
        }

        if (closestTarget == null)
            return false;

        CurrentTarget = closestTarget;
        cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);

        return true;
    }

    public void Cancel()
    {
        if (CurrentTarget == null)
            return;

        cineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }

    void Target_OnDestroyed(Target target)
    {
        RemoveTarget(target);
    }

    void RemoveTarget(Target target)
    {
        if (CurrentTarget == target)
        {
            cineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        target.OnDestroyed -= Target_OnDestroyed;
        targets.Remove(target);
    }
}
