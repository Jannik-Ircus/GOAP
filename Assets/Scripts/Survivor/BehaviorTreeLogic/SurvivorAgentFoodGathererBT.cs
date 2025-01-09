using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SurvivorAgentFoodGathererBT : BTTree
{

    public NavMeshAgent navAgent;
    public SurvivorAgentUpdaterBT agent;

    protected override BTNode SetupTree()
    {
        if(navAgent == null || agent == null)
        {
            Debug.LogError("Missing references on " + name);
            return null;
        }

        BTNode root = new BTSelector(new List<BTNode>
        {
            new BTSequence(new List<BTNode>
            {
                new CheckSpottedEnemy(agent),
                new TaskRunRandom(navAgent)
            }),

            new BTSequence(new List<BTNode>
            {
                new CheckHunger(agent),
                new BTSelector(new List<BTNode>
                {
                    new BTSequence(new List<BTNode>
                    {
                        new CheckBerryStorage(agent),
                        new TaskUseBerryStorage(agent, navAgent)
                    }),
                    new TaskEatBerry(agent, navAgent)
                })
            }),

            new BTSequence(new List<BTNode>
            {
                new CheckWarmth(agent),
                new BTSelector(new List<BTNode>
                {
                    new BTSequence(new List<BTNode>
                    {
                        new CheckFirepitActive(agent),
                        new TaskWarmUp(agent, navAgent)
                    }),
                    new BTSelector(new List<BTNode>
                    {
                        new BTSequence(new List<BTNode>
                        {
                            new CheckWoodStorage(agent),
                            new TaskUseWoodStorage(agent, navAgent),
                            new TaskWarmUp(agent, navAgent)
                        }),
                        new BTSelector(new List<BTNode>
                        {
                            new BTSequence(new List<BTNode>
                            {
                                new CheckWoodInInventory(agent),
                                new TaskPickUpWood(agent, navAgent)
                            }),
                            new BTSequence(new List<BTNode>
                            {
                                new TaskBuildFirepit(agent, navAgent),
                                new TaskWarmUp(agent, navAgent)
                            })
                        })
                    })
                })
            }),

            new BTSequence(new List<BTNode>
            {
                new CheckBerryStorageFull(agent),
                new BTSelector(new List<BTNode>
                {
                    new BTSequence(new List<BTNode>
                    {
                        new CheckBerryInInventory(agent),
                        new TaskPickUpBerry(agent, navAgent)
                    }),
                    new TaskStoreBerry(agent, navAgent)
                })
            }),

            new BTSequence(new List<BTNode>
            {
                new CheckWoodStorageFull(agent),
                new BTSelector(new List<BTNode>
                {
                    new BTSequence(new List<BTNode>
                    {
                        new CheckWoodInInventory(agent),
                        new TaskPickUpWood(agent, navAgent),
                    }),

                    new TaskStoreWood(agent, navAgent)
                })
            })

            
        });

        return root;
    }
}