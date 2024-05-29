using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public BulletPool ObjectPool { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        ObjectPool = GetComponent<BulletPool>();
    }
}
