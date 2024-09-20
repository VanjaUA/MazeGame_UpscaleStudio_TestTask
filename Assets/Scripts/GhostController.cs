using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [SerializeField] private float wanderingSpeed, runSpeed;
    [SerializeField] private Vector3[] wanderingPositions;
    [SerializeField] private float maxChaseDistance;
}
