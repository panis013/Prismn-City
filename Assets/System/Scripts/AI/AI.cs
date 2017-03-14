using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour2 {
    //控制行为
    public enum TriggerType//触发具体行为的条件 列表于此
    {   
        always,
        distanceOver,
        distanceIn,
        onhit,
		onLighthit,
        die
    }
    [System.Serializable]
    public struct AIAction
    {
        public TriggerType trigger;
        public Actions action;
    }
    public AIAction[] actionList;
    public float DistanceBorder = 500f;
    private ArrayList runtimeList = new ArrayList();
    float[] t;
    

    private void Start()
    {
        
        initActionList(actionList);
        t = new float[runtimeList.Count];
        for (int i=0; i < runtimeList.Count; i++)
        {
            AIAction temp = (AIAction)runtimeList[i];
            if (temp.action.functionType == Actions.ActionType.attack)
            {
                t[i] = 0f;
            }
        }
    }
    public void FixedUpdate()
    {

        for (int i = 0; i < this.runtimeList.Count; i++)
        {
            
            AIAction temp = (AIAction)this.runtimeList[i];
            if (temp.action.functionType == Actions.ActionType.attack)
            {
                t[i] += Time.fixedDeltaTime;
                if (temp.trigger == TriggerType.distanceIn)
                {
					if (TopScript.GetPlayerDistance(gameObject) <= DistanceBorder)
                    {
                        if (t[i] >= temp.action.AttackInterval)
                        {
                            t[i] = 0;
                            temp.action.run();
                        }

                    }

                }
                if(temp.trigger == TriggerType.always)
                {
                    if (t[i] >= temp.action.AttackInterval)
                    {
                        t[i] = 0;
                        temp.action.run();
                    }
                }
         }
            if(temp.trigger == TriggerType.distanceOver)
            {
				if (TopScript.GetPlayerDistance(gameObject) > DistanceBorder)
                { 
                    temp.action.run();
                }
            }
        }

    }
	public void OnHit()
    {   
        for(int i = 0; i < runtimeList.Count; i++)
        {
            AIAction temp = (AIAction)runtimeList[i];
            if (temp.trigger == TriggerType.onhit)
            {
                temp.action.run();
            }
        }
    }
	public void OnLightHit(){
		for(int i = 0; i < runtimeList.Count; i++)
		{
			AIAction temp = (AIAction)runtimeList[i];
			if (temp.trigger == TriggerType.onLighthit)
			{
				temp.action.run();
			}
		}
	}




    public void initActionList(AIAction[] temp)
    {
        AIAction tempActionList;
        Actions tempAction;
        if (temp.Length == 0)
        {
            return;
        }

        for (int i = 0; i < temp.Length; i++)
        {
            tempAction = ScriptableObject.Instantiate(temp[i].action) as Actions;
            tempAction.setOwner(this);
            tempActionList.action = tempAction;
            tempActionList.trigger = temp[i].trigger;
            runtimeList.Add(tempActionList);

        }


    }

}
