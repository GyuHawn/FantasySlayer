using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFuntionButton : MonoBehaviour
{
    private HeroKnight knight;

    public Button attack;
    public Button jump;
    public Button clime;

    private void Update()
    {
        if (knight == null)
        {
            knight = GameObject.Find("Player").GetComponent<HeroKnight>();
        }
    }

    public void JumpButtonClicked()
    {
        knight.Jump();
    }

    public void AttackButtonClicked()
    {
        knight.Attack();
    }
}
