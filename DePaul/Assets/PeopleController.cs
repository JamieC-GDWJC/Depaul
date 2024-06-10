using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PeopleController : MonoBehaviour
{
    private NavMeshAgent _agent;

    private float timer = 0;

    private Vector3 lookAt;

    private float timeLimit = 10;
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        int gender = Random.Range(0, 1);
        Destroy(transform.GetChild(gender).gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Mathf.Approximately(_agent.remainingDistance, 0) || timer > timeLimit)
        {
            int x = Random.Range(-150, -50);
            int y = Random.Range(-50, 50);
            _agent.SetDestination(new Vector3(x, 0, y));
            timer = 0;
            
        }

        _agent.transform.LookAt(_agent.steeringTarget);

    }

}
