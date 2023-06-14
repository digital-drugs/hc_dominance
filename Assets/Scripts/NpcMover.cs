using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcMover : MonoBehaviour
{
    private List<GameObject> obstacleList;
    private ObstacleSpawner obstacleSpawner;
    private float sqrDistance;
    private Vector3 target;
    private bool canMove;
    private bool firstTargetDown;
    private int obstacleCount;
    private NavMeshAgent agent;
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    private void Start()
    {
        
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        agent = GetComponent<NavMeshAgent>();
        obstacleList = new List<GameObject>();
        for (int i = 0; i < obstacleSpawner.Dummies.Count; i++)
        {
            obstacleList.Add(obstacleSpawner.Dummies[i]);
        }

        obstacleCount = obstacleList.Count;
        sqrDistance = (obstacleList[0].transform.position - transform.position).sqrMagnitude;
        ChooseRandom();
        
    }

    // Update is called once per frame
    private void Update()
    {
        //CheckObstacleCount();
        if (canMove)
        {
            Move();
        }
    }

    private void Move()
    {
        animator.SetFloat("VelocityY",1);
        agent.destination = target;
    }
    
    private void FindNextTarget()
    {
        foreach (GameObject obstacle in obstacleList)
        {
            if ((transform.position - obstacle.transform.position).sqrMagnitude < sqrDistance)
            {
                sqrDistance = (transform.position - obstacle.transform.position).sqrMagnitude;
                target = obstacle.transform.position;

                transform.rotation = Quaternion.LookRotation(target);
                canMove = true;
            }
        }
    }

    private void CheckObstacleCount()
    {
        
        if (obstacleList.Count != obstacleCount)
        {
            canMove = !canMove;
            if (firstTargetDown)
            {
                FindNextTarget();
            }
        }
    }

    private void ChooseRandom()
    {     
        target = obstacleList[Random.Range(0, obstacleList.Count)].transform.position;
        canMove = true;
        firstTargetDown = true;
    }
}
