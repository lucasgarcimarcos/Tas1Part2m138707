using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControl1 : MonoBehaviour {

    GameObject[] goalLocations;
    NavMeshAgent agent2;
    Animator anim;
    float speedMult;
    float detectionRadius = 20;
    float fleeRadius = 10;

    void Start() {

        agent2 = this.GetComponent<NavMeshAgent>();
        goalLocations = GameObject.FindGameObjectsWithTag("goal");
        int i = Random.Range(0, goalLocations.Length);
        agent2.SetDestination(goalLocations[i].transform.position);
        anim = this.GetComponent<Animator>();
        anim.SetFloat("wOffset", Random.Range(0.0f, 1.0f));
        ResetAgent();

    }

    void ResetAgent()
    {
        speedMult = Random.Range(0.5f, 1.5f);
        anim.SetFloat("speedMult", speedMult);
        agent2.speed *= speedMult;
        anim.SetTrigger("isWalking");
        agent2.angularSpeed = 120;
        agent2.ResetPath();
    }

    public void DetectNewObstacle(Vector3 position)
    {
        if(Vector3.Distance(position, this.transform.position) < detectionRadius)
        {
            Vector3 fleeDirection = (this.transform.position - position).normalized;
            Vector3 newgoal = this.transform.position + fleeDirection * fleeRadius;

            NavMeshPath path = new NavMeshPath();
            agent2.CalculatePath(newgoal, path);

            if(path.status != NavMeshPathStatus.PathInvalid)
            {
                agent2.SetDestination(path.corners[path.corners.Length - 1]);
                anim.SetTrigger("isRunning");
                agent2.speed = 10;
                agent2.angularSpeed = 500;
            }

        }
    }


    void Update() {
        if(agent2.remainingDistance < 1)
        {
            ResetAgent();
            int i = Random.Range(0, goalLocations.Length);
            agent2.SetDestination(goalLocations[i].transform.position);
        }
    }
}