using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossUI : MonoBehaviour
{
    private MonsterController mController;
    public Image healthBar;
    public TMP_Text bossHpText;

    void Start()
    {
        mController = GetComponent<MonsterController>();
    }

    private void Update()
    {
        if (mController.maxHealth != 0)
        {
            bossHpText.text = "HP : " + ((float)mController.currentHealth / mController.maxHealth) * 100 + "% [" + mController.currentHealth + "/" + mController.maxHealth + "]";
        }
    }

    public void UpdateBossHealthBar()
    {
        if (mController.maxHealth != 0)
        {
            float ratio = (float)mController.currentHealth / mController.maxHealth;
            healthBar.fillAmount = ratio;
        }
    }
}
