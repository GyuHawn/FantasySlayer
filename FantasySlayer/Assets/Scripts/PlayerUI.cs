using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

        if (SceneManager.GetActiveScene().name == "MainMeun")
        {
            Destroy(hp.transform.root.gameObject);
        }
    }
    
    void UpdateHealthBar()
    {
        float ratio = currentHealth / maxHealth;
        healthBar.fillAmount = ratio;
    }
}
