using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public GameObject hp;
    public Image healthBar;
    public float maxHealth; // �ִ� ü��
    private float currentHealth; // ���� ü��

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
