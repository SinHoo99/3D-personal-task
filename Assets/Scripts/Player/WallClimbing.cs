using UnityEngine;

public class WallClimbing : MonoBehaviour
{
    public float wallCheckDistance = 0.5f; // 벽을 감지하기 위한 거리
    public float wallClimbSpeed = 3f; // 벽을 따라 움직이는 속도
    public float climbInputThreshold = 0.5f; // 올라가기 입력 감지 임계값
    public LayerMask wallLayer; // 벽 레이어

    private Rigidbody playerRigidbody; // 플레이어의 Rigidbody 컴포넌트
    private bool isWallClinging = false; // 벽에 붙어있는지 여부를 추적하기 위한 변수

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>(); // 플레이어의 Rigidbody 컴포넌트 가져오기
    }

    void FixedUpdate()
    {
        // 플레이어가 벽에 닿았는지 검사
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, wallCheckDistance, wallLayer))
        {
            // 벽에 닿았으면 벽을 따라 움직이게 함
            ClimbWall(hit.normal);
        }
        else
        {
            // 벽에 닿지 않았을 때는 중력을 활성화하고, 벽을 따라 올라가지 않는 상태로 변경
            if (isWallClinging)
            {
                playerRigidbody.useGravity = true;
                isWallClinging = false;
            }
        }
    }

    void ClimbWall(Vector3 wallNormal)
    {
        // 벽을 따라 움직이는 속도를 계산
        Vector3 wallParallel = Vector3.ProjectOnPlane(transform.forward, wallNormal).normalized;
        Vector3 wallClimbVelocity = wallParallel * wallClimbSpeed;

        // 플레이어의 이동 속도를 설정하여 벽을 따라 움직임
        playerRigidbody.velocity = wallClimbVelocity;

        // 중력을 비활성화하여 플레이어가 벽을 따라 올라갈 수 있도록 함
        playerRigidbody.useGravity = false;
        isWallClinging = true;

        // 올라가기 입력을 감지하여 벽을 따라 올라감
        float climbInput = Input.GetAxis("Vertical");
        if (climbInput > climbInputThreshold)
        {
            // 플레이어의 속도에 상수를 곱하여 위쪽으로 이동
            playerRigidbody.velocity += transform.up * wallClimbSpeed * climbInput;
        }
    }
}
