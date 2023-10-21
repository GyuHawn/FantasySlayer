using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public GameObject hp;
    public Image healthBar;
    public float maxHealth; // 최대 체력
    private float currentHealth; // 현재 체력

    public GameObject back;
    void Start()
    {
        DontDestroyOnLoad(hp.transform.root.gameObject);
    }

    private void Update()
    {
        back.SetActive(true);
    }

    void UpdateHealthBar()
    {
        float ratio = currentHealth / maxHealth;
        healthBar.fillAmount = ratio;
    }
}
