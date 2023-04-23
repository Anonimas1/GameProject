using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    public Transform destination;
    public float destinationUpdateRate;
    
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(UpdateTargetPosition());
    }

    private IEnumerator UpdateTargetPosition()
    {
        while (true)
        {
            agent.destination = destination.position;
            yield return new WaitForSeconds(destinationUpdateRate);
        }
    }
}
