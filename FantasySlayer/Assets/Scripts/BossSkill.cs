using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossSkill : MonoBehaviour
{
    // 스킬1
    public GameObject[] fSkillSpwan;
    public GameObject fSkill;
    public float spd; // 속도
    public int skillNumber; // 사용횟수

    // 스킬 2

    private void Start()
    {
        // UseSkill(Skill1); // 스킬사용예시
    }

    private void UseSkill(Func<IEnumerator> skill)
    {
        StartCoroutine(skill());
    }

    IEnumerator Skill1()
    {
        for (int i = 0; i < skillNumber; i++)
        {
            for (int j = 0; j < fSkillSpwan.Length; j += 2)
            {
                var spawned = Instantiate(fSkill, fSkillSpwan[j].transform.position, Quaternion.identity);
                StartCoroutine(MoveAndDestroy(spawned));
            }

            yield return new WaitForSeconds(1);

            for (int j = 1; j < fSkillSpwan.Length; j += 2)
            {
                var spawned = Instantiate(fSkill, fSkillSpwan[j].transform.position, Quaternion.identity);
                StartCoroutine(MoveAndDestroy(spawned));
            }

            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator Skill2()
    {

        yield break;
    }

    IEnumerator Skill3()
    {

        yield break;
    }


    IEnumerator MoveAndDestroy(GameObject obj)
    {
        while (obj.transform.position.y != 0)
        {
            obj.transform.Translate(new Vector3(0, -spd, 0) * Time.deltaTime);

            if (obj.transform.position.y <= 0)
                Destroy(obj);

            yield return null;
        }

        Destroy(obj);
    }
}
