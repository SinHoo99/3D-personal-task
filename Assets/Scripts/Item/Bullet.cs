using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnEnable()
    {
        // 3�� �Ŀ� �ڵ����� ReturnToPool �޼��带 ȣ���մϴ�.
        Invoke("ReturnToPool", 3f);
    }

    private void OnDisable()
    {
        // ��Ȱ��ȭ�� �� ��� Invoke ȣ���� ����մϴ�.
        CancelInvoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        // ������Ʈ�� Ǯ�� ��ȯ�ϰ� ��Ȱ��ȭ�մϴ�.
        GameManager.Instance.ObjectPool.ReturnToPool("Bullet", gameObject);
    }
}
