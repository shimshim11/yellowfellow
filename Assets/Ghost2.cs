using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost2 : MonoBehaviour
{

    NavMeshAgent agent;

    [SerializeField]
    Material scaredMaterial;
    Material normalMaterial;
    void Start()
    {
    agent = GetComponent<NavMeshAgent>();
    agent.destination = PickRandomPosition();

    normalMaterial = GetComponent<Renderer>().material;

    }

    Vector3 PickRandomPosition()
    {
        Vector3 destination = transform.position;
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle * 8.0f;
        destination.x += randomDirection.x;
        destination.z += randomDirection.y;

        NavMeshHit navHit;
        NavMesh.SamplePosition(destination,out navHit ,8.0f, NavMesh.AllAreas);

        return navHit.position;
    }

    bool hiding = false;

    [SerializeField]
    Fellow player;

    void Update()
    {
        if(player.PowerupActive())
        {
            if(!hiding || agent.remainingDistance < 0.2f)
            {
                hiding = true; 
                agent.destination = PickHidingPlace();
                GetComponent<Renderer>().material = scaredMaterial;
            }
        }
         else   
        {
            if (hiding)
            {
                GetComponent<Renderer>().material = normalMaterial;
                hiding = false;
            }

            if(agent.remainingDistance < 0.2f) 
            {
                agent.destination = PickRandomPosition();
                hiding = false;
                GetComponent<Renderer>().material = normalMaterial;
            }
        } 
    }

    bool CanSeePlayer()
    {
        Vector3 rayPos = transform.position;
        Vector3 rayDir = (player.transform.position - rayPos).normalized;

        RaycastHit info;
        if (Physics.Raycast(rayPos, rayDir, out info))
        {
            if(info.transform.CompareTag("Fellow"))
            {
                return true;
            }
        }
        return false;
    }

    Vector3 PickHidingPlace()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        NavMeshHit navHit;
        NavMesh.SamplePosition(transform.position - (directionToPlayer * 8.0f), out navHit, 8.0f, NavMesh.AllAreas);

        return navHit.position;
    }
}