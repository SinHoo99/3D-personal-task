using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator animator;
    public GameObject Rifle;
    public GameObject Pistol;
    public GameObject BackRifle; // �߰��� BackRifle ������Ʈ

    [Header("PlayerAnimationController Value")]
    float maxWeight = 1; // �ִ� ����ġ
    float changeSpeed = 2; // ��ȭ �ӵ�

    private Coroutine changeCoroutine; // ����ġ ���� �ڷ�ƾ ����

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

        // �̹� �ִ� ����ġ�� ��� ���� ����ġ�� ������ ���ҽ�ŵ�ϴ�.
        if (currentWeight >= maxWeight)
        {
            changeCoroutine = StartCoroutine(ReduceWeightToZero(layerIndex));
            return;
        }

        // �ٸ� ���� ���̾��� ����ġ�� ������ ���ҽ�ŵ�ϴ�.
        changeCoroutine = StartCoroutine(ToggleWeight(layerIndex));
    }

    private IEnumerator ReduceWeightToZero(int layerIndex)
    {
        while (animator.GetLayerWeight(layerIndex) > 0)
        {
            float weight = animator.GetLayerWeight(layerIndex);
            weight -= Time.deltaTime * changeSpeed;
            animator.SetLayerWeight(layerIndex, Mathf.Max(weight, 0)); // 0 ���Ϸ� �������� �ʵ��� �մϴ�.
            yield return null;
        }

        // ���� ���Ⱑ ��Ȱ��ȭ�ǵ��� ����
        if (layerIndex == 1)
        {
            Rifle.SetActive(false);
            BackRifle.SetActive(true); // 1�� ���̾��� ����ġ�� 0�� �Ǹ� BackRifle�� Ȱ��ȭ
        }
        else if (layerIndex == 2)
        {
            Pistol.SetActive(false);
        }

        changeCoroutine = null;
    }

    private IEnumerator ToggleWeight(int layerIndex)
    {
        int otherLayerIndex = 3 - layerIndex; // �ٸ� ���̾��� �ε���

        // �ٸ� ���� ���̾��� ����ġ�� ���ҽ�ŵ�ϴ�.
        while (animator.GetLayerWeight(otherLayerIndex) > 0)
        {
            float weight = animator.GetLayerWeight(otherLayerIndex);
            weight -= Time.deltaTime * changeSpeed;
            animator.SetLayerWeight(otherLayerIndex, Mathf.Max(weight, 0)); // 0 ���Ϸ� �������� �ʵ��� �մϴ�.
            yield return null;
        }

        // ���� ���� ���̾��� ����ġ�� ������ŵ�ϴ�.
        while (animator.GetLayerWeight(layerIndex) < maxWeight)
        {
            float weight = animator.GetLayerWeight(layerIndex);
            weight += Time.deltaTime * changeSpeed;
            animator.SetLayerWeight(layerIndex, Mathf.Min(weight, maxWeight)); // �ִ밪�� ���� �ʵ��� �մϴ�.
            yield return null;
        }

        // ���� ���Ⱑ Ȱ��ȭ�ǵ��� ����
        if (layerIndex == 1)
        {
            Rifle.SetActive(true);
            BackRifle.SetActive(false); // 1�� ���̾��� ����ġ�� 1�� �� BackRifle�� ��Ȱ��ȭ
            Pistol.SetActive(false); // �ٸ� ����� ��Ȱ��ȭ
        }
        else if (layerIndex == 2)
        {
            Pistol.SetActive(true);
            Rifle.SetActive(false); // �ٸ� ����� ��Ȱ��ȭ
            BackRifle.SetActive(true); // 2�� ���̾��� ����ġ�� 1�� �� BackRifle�� Ȱ��ȭ
        }

        changeCoroutine = null;
    }
}
