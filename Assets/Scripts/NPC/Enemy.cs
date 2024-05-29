using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Renderer renderer;
    private Coroutine hitCoroutine;

    private void Start()
    {
        // �ڽ� ������Ʈ�� Sphere�� Renderer ������Ʈ ��������
        renderer = GetComponentInChildren<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet")) // �Ѿ˿� "Bullet" �±׸� �����ؾ� �մϴ�.
        {
            // ������ ���� ����
            Color randomColor = new Color(Random.value, Random.value, Random.value);

            // ���� ������ ������ �������� ����
            renderer.material.color = randomColor;
        }
    }


}
