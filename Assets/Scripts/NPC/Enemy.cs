using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,        // 대기 상태
    Wandering,   // 방황 상태
}

public class Enemy : MonoBehaviour
{
    private Renderer renderer;

    public float moveSpeed;

    [Header("AI")]
    public float detectDistance;  // 감지 거리
    private AIState aiState;

    [Header("Wandering")]
    public float minWanderDistance;   // 최소 방황 거리
    public float maxWanderDistance;   // 최대 방황 거리
    public float minWanderWaitTime;   // 최소 대기 시간
    public float maxWanderWaitTime;   // 최대 대기 시간

    private NavMeshAgent agent;

    private float fieldOfView = 120f;   // 시야각
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
        PassiveUpdate();  // 매 프레임마다 상태 업데이트
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
                WanderToNewLocation();  // 새로운 위치로 방황 시작
                break;
        }
    }

    void PassiveUpdate()
    {
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);  // 방황 중 목적지에 도달하면 Idle 상태로 전환
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));  // 랜덤 시간 후 새로운 위치로 이동
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
        CalculateWeightedPath(targetPosition);  // 새로운 방황 위치로 가중치가 적용된 경로 계산
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
            // 랜덤한 색상 생성
            Color randomColor = new Color(Random.value, Random.value, Random.value);

            // 적의 색상을 랜덤한 색상으로 변경
            renderer.material.color = randomColor;
        }
    }

    private void CalculateWeightedPath(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(targetPosition, path);

        // 각 지점마다 가중치 적용 로직 추가
        for (int i = 0; i < path.corners.Length; i++)
        {
            // 예: 특정 구조물에 가까운 지점은 가중치를 더하여 피하도록 설정
            if (IsNearObstacle(path.corners[i]))
            {
                // 가중치를 추가하여 피하도록 하는 로직 구현
                Vector3 avoidDirection = (path.corners[i] - transform.position).normalized * 2;
                path.corners[i] += avoidDirection;
            }
        }

        // 최종 경로 설정
        agent.SetPath(path);
    }

    private bool IsNearObstacle(Vector3 position)
    {
        // 특정 구조물에 가까운지 판단하는 로직 구현
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
