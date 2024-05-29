using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public interface IInteractable
    {
        public string GetInteractPrompt();
        public void OnInteract();
    }

    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera camera;

    [Header("WeaponPrefabs")]
    public GameObject Rifle;
    public GameObject Pistol;



    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            if (curInteractable is ItemObject)
            {
                CheckItem();
            }
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
            
        }
    }

    void CheckItem()
    {
        ItemData itemData = curInteractGameObject.GetComponent<ItemObject>().data;
        switch (itemData.type)
        {
            case ItemType.Equipable:
                // 장비 아이템 처리
                HandleEquipableItem(itemData);
                break;
            case ItemType.Consumable:
                // 소비 아이템 처리
                HandleConsumableItem(itemData);
                break;
            case ItemType.Resource:
                // 리소스 아이템 처리
                HandleResourceItem(itemData);
                break;
            default:
                Debug.LogWarning("알 수 없는 아이템 타입입니다.");
                break;
        }
    }
    void HandleEquipableItem(ItemData itemData)
    {
        if (curInteractGameObject.name == "Rifle")
        {
            Rifle.gameObject.SetActive(true);
        }
        else if (curInteractGameObject.name == "Pistol")
        {
            Pistol.gameObject.SetActive(true);
            StartCoroutine(SpeedBoostCoroutine(1f));
        }
        else
        {
            Debug.LogWarning("알 수 없는 장비 아이템입니다.");
        }
    }

    void HandleConsumableItem(ItemData itemData)
    {
        foreach (var consumable in itemData.consumables)
        {
            switch (consumable.type)
            {
                case ConsumableType.Hunger:

                    break;
                case ConsumableType.Health:

                    break;
                case ConsumableType.ZeroGravity:

                    CharacterManager.Instance.Player.condition.ApplyZeroGravity(consumable.value);
                    break;
                default:
                    Debug.LogWarning("알 수 없는 소비 아이템 타입입니다.");
                    break;
            }
        }
    }

    void HandleResourceItem(ItemData itemData)
    {
      
    }

    private IEnumerator SpeedBoostCoroutine(float duration)
    {
        float originalSpeed = CharacterManager.Instance.Player.controller.runSpeed;
        CharacterManager.Instance.Player.controller.runSpeed += 5f;
        yield return new WaitForSeconds(duration);
        CharacterManager.Instance.Player.controller.runSpeed = originalSpeed;
    }
}
