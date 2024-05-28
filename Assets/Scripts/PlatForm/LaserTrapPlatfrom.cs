using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LaserTrapPlatfrom : MonoBehaviour
{
    public Transform startPoint; // ������ ������
    public float laserDistance = 10f; // �������� ����
    public LayerMask targetLayer; // �������� ������ ���̾�
    public TextMeshProUGUI warningText; // ��� �޽����� ǥ���� UI Text ���
    public int gridSizeX = 5; // ������ ���� ũ��
    public int gridSizeZ = 5; // ������ ���� ũ��
    public float warningDuration = 1f; // ��� �޽����� ǥ���� �ð�

    private float warningTimer; // ��� �޽��� ǥ�� Ÿ�̸�

    private void Update()
    {
        ShootRay();
    }

    public void ShootRay()
    {
        // ������ �� ������ ���� �߻�
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                // �� ���� ��ġ ���
                Vector3 startPointOffset = new Vector3(
                    x * transform.localScale.x / gridSizeX - transform.localScale.x / 2f,
                    0f,
                    z * transform.localScale.z / gridSizeZ - transform.localScale.z / 2f
                );
                Vector3 rayStartPoint = startPoint.position + startPointOffset;

                // ���� ����
                Ray laserRay = new Ray(rayStartPoint, -startPoint.up);
                RaycastHit hit;

                // ����ĳ��Ʈ�� �浹 �˻�
                if (Physics.Raycast(laserRay, out hit, laserDistance, targetLayer))
                {
                    // �浹�� ����� �÷��̾��� ���
                    if (hit.distance < laserDistance)
                    {
                        // ��� �޽��� ǥ��
                        warningText.gameObject.SetActive(true);
                        warningText.text = "���\n�������� �����Ǿ����ϴ�.";

                        // ��� �޽��� ǥ�� Ÿ�̸� ����
                        warningTimer = warningDuration;

                        return; // �� �� �����Ǹ� �ٷ� ����
                    }
                }
            }
        }

        // ��� �޽��� ǥ�� Ÿ�̸� ����
        warningTimer -= Time.deltaTime;
        if (warningTimer <= 0f)
        {
            // ��� �޽��� �����
            warningText.gameObject.SetActive(false);
        }
    }
}
