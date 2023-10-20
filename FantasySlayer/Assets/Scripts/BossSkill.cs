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
    public GameObject[] tSkillSpwan;
    public GameObject tSkillEnermy;

    // 스킬 3
    public GameObject sSkillSpwan;
    public GameObject sSkillEnermy;

    private void Start()    
    {
        //UseSkill(Skill3);
        StartCoroutine(UseSkillsInSequence());
    }

    private IEnumerator UseSkillsInSequence()
    {
        UseSkill(Skill1);
        yield return new WaitForSeconds(10);
        UseSkill(Skill2);
        yield return new WaitForSeconds(5);
        UseSkill(Skill3);
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
        tSkillEnermy.SetActive(true);

        yield return new WaitForSeconds(2);

        tSkillEnermy.SetActive(false);

        for (int i = 0; i < tSkillSpwan.Length; i++)
        {
            tSkillSpwan[i].SetActive(true);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(2);

        for (int i = 0; i < tSkillSpwan.Length; i++)
        {
            tSkillSpwan[i].SetActive(false);
        }
    }

    IEnumerator Skill3()
    {
        sSkillEnermy.SetActive(true);
        yield return new WaitForSeconds(2);
        sSkillEnermy.SetActive(false);
        sSkillSpwan.SetActive(true);

        yield return new WaitForSeconds(5);
        sSkillSpwan.SetActive(false);

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
