using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPlatform : MonoBehaviour
{
    private bool isPlayerColliding = false; // 플레이어와의 충돌 여부를 추적하기 위한 변수

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // 충돌한 객체가 플레이어인지 확인
        {
            isPlayerColliding = true; // 플레이어와 충돌 상태 업데이트

            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>(); // 플레이어의 Rigidbody 컴포넌트 가져오기

            if (playerRigidbody != null)
            {
                playerRigidbody.useGravity = false; // 플레이어의 중력을 비활성화
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // 충돌이 끝났는지 확인
        {
            isPlayerColliding = false; // 플레이어와의 충돌 상태 업데이트

            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>(); // 플레이어의 Rigidbody 컴포넌트 가져오기

            if (playerRigidbody != null)
            {
                playerRigidbody.useGravity = true; // 플레이어의 중력을 다시 활성화
            }
        }
    }
}
