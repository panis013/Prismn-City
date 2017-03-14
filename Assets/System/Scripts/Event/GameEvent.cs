using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class GameEvent : ScriptableObject {
	public enum GameEventType
	{
		ApplyDamege,
		CreateUnit,
		Wait,
		TriggerToTarget,
		AllUnitInRange,
		TargetFilter,
		RandomTarget,
		If,
		ForAllTargetInPool,
		BranchEnd
	}
	public GameEventType functionType;
	private GameEventTrigger OwnerTrigger;
	public bool Over = false;
	public bool Wait = true;
	public bool running = false;
	public ArrayList targetPool;
	[System.Serializable]
	public struct ApplyDamegeP{
		public float damege;
	}
	public ApplyDamegeP ApplyDamegeParameter;
	[System.Serializable]
	public struct WaitP
	{
		public float time;
	}
	public WaitP WaitParameter;


	/**************************/
	float timerforWait = 0f;
	/***************************/
	public bool run(){
		running = true;
		switch (functionType) {
		case GameEventType.ApplyDamege:
			GameObject go = (GameObject)getFirstTarget ();
			if (go != null) {
				GameUnit gu = go.GetComponent<GameUnit> ();
				if (gu != null) {
					//gu.getDamege (ApplyDamegeParameter.damege);
				}
			}
			getOverred ();
			break;
		case GameEventType.TriggerToTarget:
			if (targetPool == null) {
				targetPool = new ArrayList ();
			} else {
				targetPool.Clear ();
			}
			targetPool.Add (OwnerTrigger.triggeredTarget);
			getOverred ();
			break;
		case GameEventType.Wait:
			timerforWait = WaitParameter.time;
			Wait = true;
			break;
		}
		return true;
	}

	public void Update(){
		if (running == false) {
			return;
		}
		switch (functionType) {
		case GameEventType.Wait:
			if (timerforWait > 0f) {
				timerforWait -= Time.deltaTime;
			} else {
				getOverred ();
			}
			break;
		}
	}
	public void getOverred(){
		Over = true;
	}
	public void getReady(){
		Over = false;
	}















	//functions

	public void setOwner(GameEventTrigger gameET){
		OwnerTrigger = gameET;
	}
	public void clearTargetPool(){
		targetPool = null;
	}
	public bool checkTargetPool(){
		if (targetPool == null || targetPool.Count == 0) {
			return false;
		}
		return true;
	}
	public object getFirstTarget(){
		if (targetPool == null || targetPool.Count == 0) {
			return null;
		}
		return targetPool [0];
	}
	public void targetPoolFilter(GameUnitCheck guc){
		for (int i = 0; i < targetPool.Count; i++) {
			if (targetPool [i].GetType () == typeof(GameObject)) {
				GameUnit gu = ((GameObject)targetPool [i]).GetComponent<GameUnit> ();
				if (guc.check (gu.unitType) == true) {
					continue;
				} else {
					targetPool.RemoveAt (i);
				}
			} else {
				targetPool.RemoveAt (i);
			}
		}
	}





}
