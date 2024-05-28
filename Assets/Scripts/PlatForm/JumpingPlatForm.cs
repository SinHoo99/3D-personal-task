using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPlatForm : MonoBehaviour
{
    public float jumpForce = 1000f;

    // ������� �浹�� �� ȣ��Ǵ� �޼ҵ�
    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� Rigidbody ��������
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();

        // Rigidbody�� �����ϸ�
        if (rb != null)
        {
            // �������� ���� ����
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
