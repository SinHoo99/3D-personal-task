using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    public Transform firePoint; // 총알이 발사되는 위치
    public float bulletSpeed = 20f; // 총알 속도
    public float spreadAmountX = 0.1f; // 오른쪽으로 향하는 정도
    public float spreadAmountY = 0.1f; // 위로 향하는 정도

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

        // 카메라가 바라보는 방향을 구합니다.
        Vector3 cameraForward = Camera.main.transform.forward;

        // 카메라의 오른쪽 방향을 구합니다.
        Vector3 cameraRight = Camera.main.transform.right;

        // 카메라의 위쪽 방향을 구합니다.
        Vector3 cameraUp = Camera.main.transform.up;

        // 총알이 나아가는 방향에 오른쪽과 위쪽 벡터를 더하여 살짝 오른쪽과 위쪽으로 향하도록 합니다.
        Vector3 spreadDirection = cameraForward + (cameraRight * spreadAmountX) + (cameraUp * spreadAmountY);

        // 총알을 오브젝트 풀에서 가져옵니다.
        GameObject bullet = GameManager.Instance.ObjectPool.SpawnFromPool("Bullet");
        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;

            // 총알이 목표 방향으로 회전하도록 설정
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
