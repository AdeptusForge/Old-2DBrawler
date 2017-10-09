using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationProcessorWizard : ScriptableWizard {

    public GameObject gameObject; 



    [MenuItem ("MyTools/AnimationProcessor")]

    static void AnimationProcessor()
    {
        ScriptableWizard.DisplayWizard<AnimationProcessorWizard>("Animation Processor", "Create/Modify Animation" );
    }

    void OnWizardCreate()
    {
        List<AnimationClip> wizardAnimationList = new List<AnimationClip>();
        AnimationClip[] animations = gameObject.GetComponent<Animator>().runtimeAnimatorController.animationClips;
        foreach (AnimationClip animation in animations)
        {
            wizardAnimationList.Add(animation);
        }        
    }
}
