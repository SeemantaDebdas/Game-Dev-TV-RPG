using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Movement : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    Camera cam;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            ShootRay();
    }

    private void ShootRay()
    {
        Ray mousePositionRay = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mousePositionRay, out RaycastHit hitInfo))
        {
            navMeshAgent.SetDestination(hitInfo.point);
        }
    }
}
