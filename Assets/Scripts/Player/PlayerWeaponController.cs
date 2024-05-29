using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    public Transform firePoint; // �Ѿ��� �߻�Ǵ� ��ġ
    public float bulletSpeed = 20f; // �Ѿ� �ӵ�
    public float spreadAmountX = 0.1f; // ���������� ���ϴ� ����
    public float spreadAmountY = 0.1f; // ���� ���ϴ� ����

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && CharacterManager.Instance.Player.controller.animator.GetLayerWeight(1) == 1)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Debug.Log("Shoot function called");

        // ī�޶� �ٶ󺸴� ������ ���մϴ�.
        Vector3 cameraForward = Camera.main.transform.forward;

        // ī�޶��� ������ ������ ���մϴ�.
        Vector3 cameraRight = Camera.main.transform.right;

        // ī�޶��� ���� ������ ���մϴ�.
        Vector3 cameraUp = Camera.main.transform.up;

        // �Ѿ��� ���ư��� ���⿡ �����ʰ� ���� ���͸� ���Ͽ� ��¦ �����ʰ� �������� ���ϵ��� �մϴ�.
        Vector3 spreadDirection = cameraForward + (cameraRight * spreadAmountX) + (cameraUp * spreadAmountY);

        // �Ѿ��� ������Ʈ Ǯ���� �����ɴϴ�.
        GameObject bullet = GameManager.Instance.ObjectPool.SpawnFromPool("Bullet");
        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;

            // �Ѿ��� ��ǥ �������� ȸ���ϵ��� ����
            bullet.transform.rotation = Quaternion.LookRotation(spreadDirection);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = spreadDirection.normalized * bulletSpeed;
            }
            else
            {
                Debug.LogError("Bullet does not have a Rigidbody component.");
            }
        }
        else
        {
            Debug.LogError("Bullet prefab not found in pool.");
        }
    }
}
