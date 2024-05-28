using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveDistance = 5f; // 발판의 총 이동 거리
    public float moveSpeed = 2f; // 발판 이동 속도

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
        // 발판을 목표 지점으로 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 만약 목표 지점에 도착했다면 방향을 반대로 변경하여 반복
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
        // 플레이어와 발판이 충돌했을 때, 플레이어를 발판 자식으로 만들어 발판 위에 고정시킴
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // 플레이어가 발판에서 떨어질 때, 플레이어의 부모를 초기화하여 발판과의 부모-자식 관계를 해제함
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }
}
