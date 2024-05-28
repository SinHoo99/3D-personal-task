using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Interaction;

public class PlatformObject : MonoBehaviour, IInteractable
{
    public float moveDistance = 10f; // 이동할 거리
    public float moveSpeed = 10f;    // 이동 속도
    public PlatFormData data;
    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.platFormData = data;

        StartCoroutine(MoveForward());
    }


    private IEnumerator MoveForward()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + transform.forward * moveDistance;

        float elapsedTime = 0;
        while (elapsedTime < moveDistance / moveSpeed)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime * moveSpeed) / moveDistance);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
}
