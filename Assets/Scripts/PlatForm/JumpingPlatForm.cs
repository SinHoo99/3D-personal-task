using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPlatForm : MonoBehaviour
{
    public float jumpForce = 1000f;

    // 점프대와 충돌할 때 호출되는 메소드
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트의 Rigidbody 가져오기
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();

        // Rigidbody가 존재하면
        if (rb != null)
        {
            // 위쪽으로 힘을 가함
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
