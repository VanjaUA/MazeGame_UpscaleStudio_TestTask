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
        SetDestinationHandle();
    }

    private void SetDestinationHandle() 
    {
        //Method to set new destination to enemy
        //Set target to player if can see him

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
        //Method to set new destination index
        newPositionIndex++;
        if (newPositionIndex >= wanderingPositions.Length)
        {
            newPositionIndex = 0;
        }
    }

    private IEnumerator FOVRoutine()
    {
        //Coroutine to call FieldOfViewCheck method with delay
        WaitForSeconds delay = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return delay;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        //Method to check if enemy can see player
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
