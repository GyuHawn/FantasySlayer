using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public GameObject hp;
    public Image healthBar;
    public float maxHealth; // 최대 체력
    private float currentHealth; // 현재 체력

    void Start()
    {
        DontDestroyOnLoad(hp.transform.root.gameObject);
    }

    void UpdateHealthBar()
    {
        float ratio = currentHealth / maxHealth;
        healthBar.fillAmount = ratio;
    }
}
