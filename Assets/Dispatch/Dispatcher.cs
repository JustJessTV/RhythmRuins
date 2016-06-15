using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class Dispatcher : MonoBehaviour {
    private static IList<DispatchEvent> events = new List<DispatchEvent>();
    private static IList<DispatchEvent> updateEvents = new List<DispatchEvent>();
    private static IList<DispatchEvent> guiEvents = new List<DispatchEvent>();
    void Awake() {
        BuildEventList();
        foreach (DispatchEvent de in events) {
            de.Awake();
        }
    }
	void Start () {
        foreach (DispatchEvent de in events) {
            de.Start();
        }
	}
	
	void Update () {
        foreach (DispatchEvent de in updateEvents) {
            de.Update();
        }
	}
    void OnGUI() {
        foreach (DispatchEvent de in guiEvents) {
            de.OnGUI();
        }
    }
    void BuildEventList () {
        foreach (System.Type type in
            Assembly.GetAssembly(typeof(Dispatcher)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(DispatchEvent)))) {
            DispatchEvent de = (DispatchEvent)System.Activator.CreateInstance(type);
            events.Add(de);
            if (IsOverrideMethod(type, "Update"))
                updateEvents.Add(de);
            if (IsOverrideMethod(type, "OnGUI"))
                guiEvents.Add(de);
        }
    }
    bool IsOverrideMethod(System.Type type, string methodName) {
        return type.GetMethod(methodName).DeclaringType == type&&!type.GetMethod(methodName).IsAbstract;
    }
}
