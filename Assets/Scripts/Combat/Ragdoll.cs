using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] CharacterController controller;

    Collider[] allColliders;
    Rigidbody[] allRigidbodies;

    void Start()
    {
        allColliders = GetComponentsInChildren<Collider>(true);
        allRigidbodies = GetComponentsInChildren<Rigidbody>(true);

        SetRagdoll(false);
    }

    public void SetRagdoll(bool isRagdoll)
    {
        foreach (Collider collider in allColliders)
        {
            if (collider.gameObject.CompareTag("Ragdoll"))
            {
                collider.enabled = isRagdoll;
            }
        }
        foreach (Rigidbody rigidbody in allRigidbodies)
        {
            if (rigidbody.gameObject.CompareTag("Ragdoll"))
            {
                rigidbody.isKinematic = !isRagdoll;
                rigidbody.useGravity = isRagdoll;
            }
        }

        controller.enabled = !isRagdoll;
        animator.enabled = !isRagdoll;
    }
}
