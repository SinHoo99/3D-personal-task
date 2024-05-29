using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator animator;
    public GameObject Rifle;
    public GameObject Pistol;
    public GameObject BackRifle; // 추가된 BackRifle 오브젝트

    [Header("PlayerAnimationController Value")]
    float maxWeight = 1; // 최대 가중치
    float changeSpeed = 2; // 변화 속도

    private Coroutine changeCoroutine; // 가중치 변경 코루틴 참조

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnDrawRifle(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && CharacterManager.Instance.Player.itemData != null)
        {
            HandleDrawInput(1);
        }
    }

    public void OnDrawPistol(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && CharacterManager.Instance.Player.itemData != null)
        {
            HandleDrawInput(2);
        }
    }

    private void HandleDrawInput(int layerIndex)
    {
        if (changeCoroutine != null)
            StopCoroutine(changeCoroutine);

        float currentWeight = animator.GetLayerWeight(layerIndex);

        // 이미 최대 가중치인 경우 현재 가중치를 서서히 감소시킵니다.
        if (currentWeight >= maxWeight)
        {
            changeCoroutine = StartCoroutine(ReduceWeightToZero(layerIndex));
            return;
        }

        // 다른 무기 레이어의 가중치를 서서히 감소시킵니다.
        changeCoroutine = StartCoroutine(ToggleWeight(layerIndex));
    }

    private IEnumerator ReduceWeightToZero(int layerIndex)
    {
        while (animator.GetLayerWeight(layerIndex) > 0)
        {
            float weight = animator.GetLayerWeight(layerIndex);
            weight -= Time.deltaTime * changeSpeed;
            animator.SetLayerWeight(layerIndex, Mathf.Max(weight, 0)); // 0 이하로 내려가지 않도록 합니다.
            yield return null;
        }

        // 현재 무기가 비활성화되도록 설정
        if (layerIndex == 1)
        {
            Rifle.SetActive(false);
            BackRifle.SetActive(true); // 1번 레이어의 가중치가 0이 되면 BackRifle을 활성화
        }
        else if (layerIndex == 2)
        {
            Pistol.SetActive(false);
        }

        changeCoroutine = null;
    }

    private IEnumerator ToggleWeight(int layerIndex)
    {
        int otherLayerIndex = 3 - layerIndex; // 다른 레이어의 인덱스

        // 다른 무기 레이어의 가중치를 감소시킵니다.
        while (animator.GetLayerWeight(otherLayerIndex) > 0)
        {
            float weight = animator.GetLayerWeight(otherLayerIndex);
            weight -= Time.deltaTime * changeSpeed;
            animator.SetLayerWeight(otherLayerIndex, Mathf.Max(weight, 0)); // 0 이하로 내려가지 않도록 합니다.
            yield return null;
        }

        // 현재 무기 레이어의 가중치를 증가시킵니다.
        while (animator.GetLayerWeight(layerIndex) < maxWeight)
        {
            float weight = animator.GetLayerWeight(layerIndex);
            weight += Time.deltaTime * changeSpeed;
            animator.SetLayerWeight(layerIndex, Mathf.Min(weight, maxWeight)); // 최대값을 넘지 않도록 합니다.
            yield return null;
        }

        // 현재 무기가 활성화되도록 설정
        if (layerIndex == 1)
        {
            Rifle.SetActive(true);
            BackRifle.SetActive(false); // 1번 레이어의 가중치가 1일 때 BackRifle을 비활성화
            Pistol.SetActive(false); // 다른 무기는 비활성화
        }
        else if (layerIndex == 2)
        {
            Pistol.SetActive(true);
            Rifle.SetActive(false); // 다른 무기는 비활성화
            BackRifle.SetActive(true); // 2번 레이어의 가중치가 1일 때 BackRifle을 활성화
        }

        changeCoroutine = null;
    }
}
