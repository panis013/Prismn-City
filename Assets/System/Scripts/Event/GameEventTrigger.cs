using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("Game/EventTrigger")]
public class GameEventTrigger : MonoBehaviour2 {
	public enum TriggerType{
		contact,
		stay,
		timing,
		exit
	}
	public enum EndType
	{
		repeat,
		suicide,
		suicideGO
	}
	public GameUnitCheck gameUCheck = new GameUnitCheck();
	public TriggerType triggerType;
	public EndType endType;
	public GameEvent[] eventList;
	private ArrayList runtimeEventList;
	public GameObject triggeredTarget;
	public GameUnit triggeredTargetUnit;
	[SerializeField]
	private int eventFlag = 0;
	public bool running = false;

	public GameEvent currentEvent{get{return (GameEvent)runtimeEventList [eventFlag];}}
	// Logics

	void OnTriggerEnter2D(Collider2D other){
		if (running == true) {
			return;
		}
		if (triggerType != TriggerType.contact) {
			return;
		}
		GameObject triGO = other.gameObject;
		GameUnit triGU = triGO.GetComponent<GameUnit> ();
		if (triGU == null) {
			return;
		}
		if (gameUCheck.check (triGU.unitType) == false) {
			return;
		}
		triggeredTarget = triGO;
		triggeredTargetUnit = triGU;
		run ();
	}
	void OnTriggerStay2D(Collider2D other){
		if (running == true) {
			return;
		}
		if (triggerType != TriggerType.stay) {
			return;
		}
		GameObject triGO = other.gameObject;
		GameUnit triGU = triGO.GetComponent<GameUnit> ();
		if (triGU == null) {
			return;
		}
		if (gameUCheck.check (triGU.unitType) == false) {
			return;
		}
		triggeredTarget = triGO;
		triggeredTargetUnit = triGU;
		run ();
	}
	void OnTriggerExit2D(Collider2D other){
		if (running == true) {
			return;
		}
		if (triggerType != TriggerType.exit) {
			return;
		}
		GameObject triGO = other.gameObject;
		GameUnit triGU = triGO.GetComponent<GameUnit> ();
		if (triGU == null) {
			return;
		}
		if (gameUCheck.check (triGU.unitType) == false) {
			return;
		}
		triggeredTarget = triGO;
		triggeredTargetUnit = triGU;
		run ();
	}
	//logics

	public void run(){
		if (running == false) {
			running = true;
		}
		if (eventFlag >= runtimeEventList.Count) {
			end ();
			return;
		}
		GameEvent ge = currentEvent;
		bool wait = ge.Wait;
		bool over = ge.Over;
		bool geRunning;
		if (over == true) {
			next ();
			run ();
		} else {
			geRunning = ge.running;
			if (geRunning == false) {
				ge.run ();
			}
			over = ge.Over;
			wait = ge.Wait;
			if (over == true||wait==false) {
				next ();
				run ();
			}
		}
	}
	public void end(){
		if (endType == EndType.repeat) {
			InitRuntimeEventList ();
			running = false;
			eventFlag = 0;
		} else if (endType == EndType.suicide) {
			Destroy (this);
		} else if (endType == EndType.suicideGO) {
			Destroy (gameObject);
		}
	}
	public void next(){
		turnto (eventFlag + 1);
		if (eventFlag > 0&&eventFlag<runtimeEventList.Count) {
			passTargetPool (eventFlag - 1, eventFlag);
		}
	}
	public void jump(int flag){
		if (flag > 0&&flag<runtimeEventList.Count) {
			passTargetPool (eventFlag, flag);
		}
		turnto (flag);
	}
	public void turnto(int flag){
		if (flag > 0&&flag<=runtimeEventList.Count) {
			eventFlag = flag;
		}

	}
	public void InitRuntimeEventList(){
		if (eventList.Length == 0) {
			return;
		}
		if (runtimeEventList != null && runtimeEventList.Count > 0) {
			object[] ges = runtimeEventList.ToArray ();
			foreach (object ge in ges) {
				GameEvent cge = ge as GameEvent;
				ScriptableObject.Destroy (cge);
			}
			runtimeEventList.Clear ();
		} else {
			runtimeEventList = new ArrayList ();
		}

		for (int i = 0; i < eventList.Length; i++) {
			GameEvent newge = (GameEvent)(ScriptableObject.Instantiate(eventList [i]));
			newge.setOwner (this);
			runtimeEventList.Add (newge);
		}
	}

	void Start(){
		InitRuntimeEventList ();
	}
	void PushUpdate(){
		if (runtimeEventList != null && runtimeEventList.Count > 0) {
			object[] ges = runtimeEventList.ToArray ();
			foreach (object ge in ges) {
				GameEvent cge = ge as GameEvent;
				cge.Update ();
			}
		}
	}
	void Update(){
		PushUpdate ();
		if (running == true) {
			run ();
		}
	}
	//Tools
	public void setFlag(int newFlag){
		eventFlag = newFlag;
	}
	public int getFlagOfEvent(GameEvent ge){
		if (runtimeEventList.Count == 0||runtimeEventList.Contains(ge)==false) {
			return -1;
		}
		return runtimeEventList.IndexOf (ge);
	}
	public void passTargetPool(int from,int to){
		GameEvent f = (GameEvent)runtimeEventList [from];
		GameEvent t = (GameEvent)runtimeEventList [to];
		t.targetPool = f.targetPool;
	}
}
