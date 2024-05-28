using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LaserTrapPlatfrom : MonoBehaviour
{
    public Transform startPoint; // 레이저 시작점
    public float laserDistance = 10f; // 레이저의 길이
    public LayerMask targetLayer; // 레이저가 감시할 레이어
    public TextMeshProUGUI warningText; // 경고 메시지를 표시할 UI Text 요소
    public int gridSizeX = 5; // 격자의 가로 크기
    public int gridSizeZ = 5; // 격자의 세로 크기
    public float warningDuration = 1f; // 경고 메시지를 표시할 시간

    private float warningTimer; // 경고 메시지 표시 타이머

    private void Update()
    {
        ShootRay();
    }

    public void ShootRay()
    {
        // 격자의 각 점에서 레이 발사
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                // 각 점의 위치 계산
                Vector3 startPointOffset = new Vector3(
                    x * transform.localScale.x / gridSizeX - transform.localScale.x / 2f,
                    0f,
                    z * transform.localScale.z / gridSizeZ - transform.localScale.z / 2f
                );
                Vector3 rayStartPoint = startPoint.position + startPointOffset;

                // 레이 생성
                Ray laserRay = new Ray(rayStartPoint, -startPoint.up);
                RaycastHit hit;

                // 레이캐스트로 충돌 검사
                if (Physics.Raycast(laserRay, out hit, laserDistance, targetLayer))
                {
                    // 충돌한 대상이 플레이어인 경우
                    if (hit.distance < laserDistance)
                    {
                        // 경고 메시지 표시
                        warningText.gameObject.SetActive(true);
                        warningText.text = "경고\n움직임이 감지되었습니다.";

                        // 경고 메시지 표시 타이머 시작
                        warningTimer = warningDuration;

                        return; // 한 번 감지되면 바로 종료
                    }
                }
            }
        }

        // 경고 메시지 표시 타이머 감소
        warningTimer -= Time.deltaTime;
        if (warningTimer <= 0f)
        {
            // 경고 메시지 숨기기
            warningText.gameObject.SetActive(false);
        }
    }
}
