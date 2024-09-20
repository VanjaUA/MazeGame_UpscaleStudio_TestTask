using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostController : MonoBehaviour
{
    [SerializeField] private float wanderingSpeed, runSpeed;
    [SerializeField] private Vector3[] wanderingPositions;
    private float minRemainingDistance = 1f;

    private int newPositionIndex = 0;

    private NavMeshAgent meshAgent;

    [Header("FOV settings")]
    [SerializeField] private float radius;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstructionMask;

    private bool canSeePlayer;
    private PlayerController player;

    private void Start()
    {
        meshAgent = GetComponent<NavMeshAgent>();
        meshAgent.speed = wanderingSpeed;

        StartCoroutine(FOVRoutine());

        transform.position = wanderingPositions[0];

        UpdatePositionIndex();
        meshAgent.SetDestination(wanderingPositions[newPositionIndex]);
    }

    private void Update()
    {
        if (canSeePlayer)
        {
            meshAgent.speed = runSpeed;
            meshAgent.SetDestination(player.transform.position);
            return;
        }

        meshAgent.speed = wanderingSpeed;
        if (meshAgent.remainingDistance < minRemainingDistance)
        {
            UpdatePositionIndex();
            meshAgent.SetDestination(wanderingPositions[newPositionIndex]);
        }
    }

    private void UpdatePositionIndex() 
    {
        newPositionIndex++;
        if (newPositionIndex >= wanderingPositions.Length)
        {
            newPositionIndex = 0;
        }
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            player = target.GetComponent<PlayerController>();
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask)) 
            {
                canSeePlayer = true;
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else
        {
            canSeePlayer = false;
        }
    }
}
