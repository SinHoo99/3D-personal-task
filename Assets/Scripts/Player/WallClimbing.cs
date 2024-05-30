using UnityEngine;

public class WallClimbing : MonoBehaviour
{
    public float wallCheckDistance = 0.5f; // ���� �����ϱ� ���� �Ÿ�
    public float wallClimbSpeed = 3f; // ���� ���� �����̴� �ӵ�
    public float climbInputThreshold = 0.5f; // �ö󰡱� �Է� ���� �Ӱ谪
    public LayerMask wallLayer; // �� ���̾�

    private Rigidbody playerRigidbody; // �÷��̾��� Rigidbody ������Ʈ
    private bool isWallClinging = false; // ���� �پ��ִ��� ���θ� �����ϱ� ���� ����

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>(); // �÷��̾��� Rigidbody ������Ʈ ��������
    }

    void FixedUpdate()
    {
        // �÷��̾ ���� ��Ҵ��� �˻�
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, wallCheckDistance, wallLayer))
        {
            // ���� ������� ���� ���� �����̰� ��
            ClimbWall(hit.normal);
        }
        else
        {
            // ���� ���� �ʾ��� ���� �߷��� Ȱ��ȭ�ϰ�, ���� ���� �ö��� �ʴ� ���·� ����
            if (isWallClinging)
            {
                playerRigidbody.useGravity = true;
                isWallClinging = false;
            }
        }
    }

    void ClimbWall(Vector3 wallNormal)
    {
        // ���� ���� �����̴� �ӵ��� ���
        Vector3 wallParallel = Vector3.ProjectOnPlane(transform.forward, wallNormal).normalized;
        Vector3 wallClimbVelocity = wallParallel * wallClimbSpeed;

        // �÷��̾��� �̵� �ӵ��� �����Ͽ� ���� ���� ������
        playerRigidbody.velocity = wallClimbVelocity;

        // �߷��� ��Ȱ��ȭ�Ͽ� �÷��̾ ���� ���� �ö� �� �ֵ��� ��
        playerRigidbody.useGravity = false;
        isWallClinging = true;

        // �ö󰡱� �Է��� �����Ͽ� ���� ���� �ö�
        float climbInput = Input.GetAxis("Vertical");
        if (climbInput > climbInputThreshold)
        {
            // �÷��̾��� �ӵ��� ����� ���Ͽ� �������� �̵�
            playerRigidbody.velocity += transform.up * wallClimbSpeed * climbInput;
        }
    }
}
