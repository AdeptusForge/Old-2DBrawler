using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAbility : Ability {



    public float useNumber;
    public float usePercent;
    public float upperLimit;
    public float lowerLimit;

    /*--Use Ranges--*/
    // RESPEC FOR NEW AI HEIRARCHY ON COMPLETION OF WORKING AI
    public List<EnemyAI.Range> combatUseRanges = new List<EnemyAI.Range>();
    protected override void Awake()
    {
        base.Awake();
    }




}
