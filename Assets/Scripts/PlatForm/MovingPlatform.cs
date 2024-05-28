using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveDistance = 5f; // ������ �� �̵� �Ÿ�
    public float moveSpeed = 2f; // ���� �̵� �ӵ�

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool movingRight = true;

    private void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + Vector3.right * moveDistance;
    }

    private void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        // ������ ��ǥ �������� �̵�
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // ���� ��ǥ ������ �����ߴٸ� ������ �ݴ�� �����Ͽ� �ݺ�
        if (transform.position == targetPosition)
        {
            if (movingRight)
                targetPosition = initialPosition;
            else
                targetPosition = initialPosition + Vector3.right * moveDistance;

            movingRight = !movingRight;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �÷��̾�� ������ �浹���� ��, �÷��̾ ���� �ڽ����� ����� ���� ���� ������Ŵ
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // �÷��̾ ���ǿ��� ������ ��, �÷��̾��� �θ� �ʱ�ȭ�Ͽ� ���ǰ��� �θ�-�ڽ� ���踦 ������
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }
}
