using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPlatform : MonoBehaviour
{
    private bool isPlayerColliding = false; // �÷��̾���� �浹 ���θ� �����ϱ� ���� ����

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // �浹�� ��ü�� �÷��̾����� Ȯ��
        {
            isPlayerColliding = true; // �÷��̾�� �浹 ���� ������Ʈ

            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>(); // �÷��̾��� Rigidbody ������Ʈ ��������

            if (playerRigidbody != null)
            {
                playerRigidbody.useGravity = false; // �÷��̾��� �߷��� ��Ȱ��ȭ
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // �浹�� �������� Ȯ��
        {
            isPlayerColliding = false; // �÷��̾���� �浹 ���� ������Ʈ

            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>(); // �÷��̾��� Rigidbody ������Ʈ ��������

            if (playerRigidbody != null)
            {
                playerRigidbody.useGravity = true; // �÷��̾��� �߷��� �ٽ� Ȱ��ȭ
            }
        }
    }
}
