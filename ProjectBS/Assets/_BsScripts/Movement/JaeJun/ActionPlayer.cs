using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPlayer : CharacterProperty
{
    int hashSkill;
    Vector2 inputDir;
    Vector2 targetDir;

    // Start is called before the first frame update
    void Start()
    {
        hashSkill = Animator.StringToHash("Skill");
    }

    // Update is called once per frame
    void Update()
    {
        targetDir.x = Input.GetAxis("Horizontal");
        targetDir.y = Input.GetAxis("Vertical");

        inputDir = Vector2.Lerp(inputDir, targetDir, Time.deltaTime * 10.0f);

        myAnim.SetFloat("x", inputDir.x);
        myAnim.SetFloat("y", inputDir.y);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            myAnim.SetTrigger(hashSkill);
        }
    }
}

