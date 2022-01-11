using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Movement : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    Camera cam;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
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
        UpdateAnimation();
    }

    private void ShootRay()
    {
        Ray mousePositionRay = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mousePositionRay, out RaycastHit hitInfo))
        {
            navMeshAgent.SetDestination(hitInfo.point);
        }
    }

    void UpdateAnimation()
    {
        anim.SetFloat("VelocityZ", navMeshAgent.velocity.magnitude);
    }
}
