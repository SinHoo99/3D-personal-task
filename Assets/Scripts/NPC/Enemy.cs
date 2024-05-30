using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,        // ��� ����
    Wandering,   // ��Ȳ ����
}

public class Enemy : MonoBehaviour
{
    private Renderer renderer;

    public float moveSpeed;

    [Header("AI")]
    public float detectDistance;  // ���� �Ÿ�
    private AIState aiState;

    [Header("Wandering")]
    public float minWanderDistance;   // �ּ� ��Ȳ �Ÿ�
    public float maxWanderDistance;   // �ִ� ��Ȳ �Ÿ�
    public float minWanderWaitTime;   // �ּ� ��� �ð�
    public float maxWanderWaitTime;   // �ִ� ��� �ð�

    private NavMeshAgent agent;

    private float fieldOfView = 120f;   // �þ߰�
    private float playerDistance;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        renderer = GetComponentInChildren<Renderer>();
    }

    private void Start()
    {
        SetState(AIState.Wandering);
    }

    private void Update()
    {
        PassiveUpdate();  // �� �����Ӹ��� ���� ������Ʈ
    }

    private void SetState(AIState state)
    {
        aiState = state;

        switch (aiState)
        {
            case AIState.Idle:
                agent.speed = moveSpeed;
                agent.isStopped = true;
                break;
            case AIState.Wandering:
                agent.speed = moveSpeed;
                agent.isStopped = false;
                WanderToNewLocation();  // ���ο� ��ġ�� ��Ȳ ����
                break;
        }
    }

    void PassiveUpdate()
    {
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);  // ��Ȳ �� �������� �����ϸ� Idle ���·� ��ȯ
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));  // ���� �ð� �� ���ο� ��ġ�� �̵�
        }
    }

    void WanderToNewLocation()
    {
        if (aiState != AIState.Idle)
        {
            return;
        }
        SetState(AIState.Wandering);
        Vector3 targetPosition = GetWanderLocation();
        CalculateWeightedPath(targetPosition);  // ���ο� ��Ȳ ��ġ�� ����ġ�� ����� ��� ���
    }

    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

        int attempts = 0;
        while (Vector3.Distance(transform.position, hit.position) < detectDistance)
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            attempts++;
            if (attempts == 30)
                break;
        }

        return hit.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            // ������ ���� ����
            Color randomColor = new Color(Random.value, Random.value, Random.value);

            // ���� ������ ������ �������� ����
            renderer.material.color = randomColor;
        }
    }

    private void CalculateWeightedPath(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(targetPosition, path);

        // �� �������� ����ġ ���� ���� �߰�
        for (int i = 0; i < path.corners.Length; i++)
        {
            // ��: Ư�� �������� ����� ������ ����ġ�� ���Ͽ� ���ϵ��� ����
            if (IsNearObstacle(path.corners[i]))
            {
                // ����ġ�� �߰��Ͽ� ���ϵ��� �ϴ� ���� ����
                Vector3 avoidDirection = (path.corners[i] - transform.position).normalized * 2;
                path.corners[i] += avoidDirection;
            }
        }

        // ���� ��� ����
        agent.SetPath(path);
    }

    private bool IsNearObstacle(Vector3 position)
    {
        // Ư�� �������� ������� �Ǵ��ϴ� ���� ����
        Collider[] hitColliders = Physics.OverlapSphere(position, 2.0f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Obstacle"))
            {
                return true;
            }
        }
        return false;
    }
}
