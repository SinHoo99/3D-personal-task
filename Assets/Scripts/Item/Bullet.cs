using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnEnable()
    {
        // 3초 후에 자동으로 ReturnToPool 메서드를 호출합니다.
        Invoke("ReturnToPool", 3f);
    }

    private void OnDisable()
    {
        // 비활성화될 때 모든 Invoke 호출을 취소합니다.
        CancelInvoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        // 오브젝트를 풀로 반환하고 비활성화합니다.
        GameManager.Instance.ObjectPool.ReturnToPool("Bullet", gameObject);
    }
}
