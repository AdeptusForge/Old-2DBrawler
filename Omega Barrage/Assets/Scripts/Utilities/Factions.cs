using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Factions : MonoBehaviour {

    public enum AllGameFactions
    {
        PlayerFaction,
        EnemyFaction1,
        EnemyFaction2,
        NPCFaction1
    }

    public void GetMyEnemiesAndAllies(AllGameFactions faction, GameObject myObject)
    {
        Creature me = myObject.GetComponent<Creature>();
        List<AllGameFactions> enemiesList;
        List<AllGameFactions> alliesList;
        switch (faction)
        {
            case AllGameFactions.PlayerFaction:
                {
                    enemiesList = new List<AllGameFactions> {AllGameFactions.EnemyFaction1, AllGameFactions.EnemyFaction2 };
                    alliesList = new List<AllGameFactions> {AllGameFactions.NPCFaction1};
                    me.myAllies.AddRange(alliesList);
                    me.myEnemies.AddRange(enemiesList);
                    return;
                }

            case AllGameFactions.EnemyFaction1:
                {
                    enemiesList = new List<AllGameFactions> { AllGameFactions.PlayerFaction };
                    alliesList = new List<AllGameFactions> { };
                    me.myAllies.AddRange(alliesList);
                    me.myEnemies.AddRange(enemiesList);
                    return;
                }
            case AllGameFactions.EnemyFaction2:
                {
                    enemiesList = new List<AllGameFactions> { };
                    alliesList = new List<AllGameFactions> { };
                    me.myAllies.AddRange(alliesList);
                    me.myEnemies.AddRange(enemiesList);
                    return;
                }
            case AllGameFactions.NPCFaction1:
                {
                    enemiesList = new List<AllGameFactions> { };
                    alliesList = new List<AllGameFactions> { };
                    me.myAllies.AddRange(alliesList);
                    me.myEnemies.AddRange(enemiesList);
                    return;
                }
            default:
                {
                    return;
                }
        }
    }

}
