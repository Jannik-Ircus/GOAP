using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPPlanner : MonoBehaviour
{
      /*
      * OnPlannerUpdate:
      * List of all agents for this planner
      * foreach agent:
      *     Check if agent has open goals, if no goals are to be done, do nothing
      *     if agent is running an action:
      *         get current plan of agent
      *         make new best plan for agent and compare to current agent plan
      *         if new plan is better and plan is not the same:
      *             abort current action
      *             abort current plan and get new plan
      *             start new plan
      *         else if new plan is not better or no new plan can generate:
      *             continue with old plan
      *     else if agent is not running an action:
      *         make new best plan for agent
      *         if no plan can generate:
      *             error log
      *         else:
      *             start new plan
      *    
      * StartPlanner:
      *     Start looping coroutine with OnPlannerUpdate every x seconds
      *     
      * PausePlanner
      *     Abort the looping coroutine
      *     
      * PlanOnce:
      *     Only do OnPlannerUpdate once
      *    
      * MakePlan:
      * 
      * 
      * StartPlan:
      *     Set Agent plan queue to the new plan
      *     Start first action of queue
      * ComparePlans:
      *     sum of cost for all actions for both and then compare values
      *     
      * Check if goal is achieved:
      *     Compare Agent goal states with Agent states and beliefs
      * 
      * Sort goals for priority
      * 
      * Debuglogging
      *     Method for different debug categories
      *     Depending on category the debug will be displayed or not
      *     easier to enable/disable debug logging
      */
}
