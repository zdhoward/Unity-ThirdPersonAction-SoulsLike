using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] Collider myCollider;

    List<Collider> alreadyCollidedWith = new List<Collider>();

    int damage = 0;
    float knockback = 0f;

    void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == myCollider)
            return;

        if (other.TryGetComponent<Health>(out Health otherHealth))
        {
            if (alreadyCollidedWith.Contains(other))
                return;

            alreadyCollidedWith.Add(other);
            otherHealth.DealDamage(damage);
        }

        if (other.TryGetComponent<ForceReceiver>(out ForceReceiver otherForceReceiver))
        {
            Vector3 forceDirection = (other.transform.position - myCollider.transform.position).normalized;
            otherForceReceiver.AddForce(forceDirection * knockback);
        }
    }

    public void SetDamage(int damage, float knockback = 0f)
    {
        this.damage = damage;
        this.knockback = knockback;
    }
}
