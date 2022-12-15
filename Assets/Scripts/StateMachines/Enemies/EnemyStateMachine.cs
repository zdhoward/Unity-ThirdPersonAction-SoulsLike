using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public Target Target { get; private set; }
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float PlayerChasingRange { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public int AttackDamage { get; private set; }
    [field: SerializeField] public float AttackKnockback { get; private set; }

    public GameObject Player { get; private set; }

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        Agent.updatePosition = false;
        Agent.updateRotation = false;

        SwitchState(new EnemyIdleState(this));
    }

    void OnEnable()
    {
        Health.OnTakeDamage += Health_OnTakeDamage;
        Health.OnDeath += Health_OnDeath;
    }

    void OnDisable()
    {
        Health.OnTakeDamage -= Health_OnTakeDamage;
        Health.OnDeath -= Health_OnDeath;
    }

    void Health_OnTakeDamage()
    {
        SwitchState(new EnemyImpactState(this));
    }

    void Health_OnDeath()
    {
        SwitchState(new EnemyDeathState(this));
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);
    }
}
