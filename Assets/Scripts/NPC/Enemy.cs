using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Renderer renderer;
    private Coroutine hitCoroutine;

    private void Start()
    {
        // 자식 오브젝트인 Sphere의 Renderer 컴포넌트 가져오기
        renderer = GetComponentInChildren<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet")) // 총알에 "Bullet" 태그를 설정해야 합니다.
        {
            // 랜덤한 색상 생성
            Color randomColor = new Color(Random.value, Random.value, Random.value);

            // 적의 색상을 랜덤한 색상으로 변경
            renderer.material.color = randomColor;
        }
    }


}
