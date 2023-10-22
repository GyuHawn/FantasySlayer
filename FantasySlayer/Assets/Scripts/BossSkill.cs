using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossSkill : MonoBehaviour
{
    // 스킬1
    public GameObject[] fSkillSpwan;
    public GameObject fSkill;
    public GameObject fSkillEnermy;
    public float spd; // 속도
    public int skillNumber; // 사용횟수

    // 스킬 2
    public GameObject[] tSkillSpwan;
    public GameObject tSkill;
    public GameObject tSkillEnermy;
    public bool isUsingSecondSkill;

    // 스킬 3
    public GameObject sSkillSpwan;
    public GameObject sSkillEnermy;

    private List<Func<IEnumerator>> skills;
    private List<Func<IEnumerator>> availableSkills;

    private void Start()
    {
        skills = new List<Func<IEnumerator>>() { Skill1, Skill2, Skill3 };
        availableSkills = new List<Func<IEnumerator>>(skills);
        isUsingSecondSkill = false;

        //UseSkill(Skill1);

        StartCoroutine(StartSkills());
    }

    private IEnumerator StartSkills()
    {
        yield return new WaitForSeconds(2);

        while (true)
        {
            if (availableSkills.Count == 0)
            {
                availableSkills = new List<Func<IEnumerator>>(skills);
            }

            int randomIndex = UnityEngine.Random.Range(0, availableSkills.Count);
            UseSkill(availableSkills[randomIndex]);

            availableSkills.RemoveAt(randomIndex);

            yield return new WaitForSeconds(5);
        }
    }

    private void UseSkill(Func<IEnumerator> skill)
    {
        StartCoroutine(skill());
    }

    IEnumerator Skill1()
    {
        fSkillEnermy.SetActive(true);

        yield return new WaitForSeconds(2);

        fSkillEnermy.SetActive(false);

        yield return new WaitForSeconds(1);

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
        isUsingSecondSkill = true;

        tSkillEnermy.SetActive(true);

        yield return new WaitForSeconds(2);

        tSkillEnermy.SetActive(false);

        List<GameObject> spawnedObjects = new List<GameObject>();

        for (int i = 0; i < tSkillSpwan.Length; i++)
        {
            var spawned = Instantiate(tSkill, tSkillSpwan[i].transform.position, Quaternion.identity);
            spawnedObjects.Add(spawned);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(2);

        foreach (var obj in spawnedObjects)
        {
            Destroy(obj);
        }

        isUsingSecondSkill = false;
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
