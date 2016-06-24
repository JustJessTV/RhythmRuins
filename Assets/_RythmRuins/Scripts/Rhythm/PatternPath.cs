using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;

public class PatternPath : MonoBehaviour {

    private static string _directoryPath;
    public static string DIRECTORYPATH{
        get {
            return _directoryPath ?? (_directoryPath = "C:/PatternFiles" ); //Application.dataPath + "/" +"PatternFiles/"); 
        }
        set {
            _directoryPath = value;
        }
    }
    public static string FILENAME{
        get{return "_DANCEPATTERN";}
    }
    private static List<string> _fileNames;
    public static List<string> fileNames {
        get {
            if (_fileNames == null) {
                _fileNames = new List<string>();
            
                var info = new DirectoryInfo(DIRECTORYPATH);
                var fileInfo = info.GetFiles();
                foreach (FileInfo f in fileInfo) {
                    _fileNames.Add(f.ToString());
                }
            }
            return _fileNames;
        }
    }
    public static void SaveNewFile(JSONNode jnode, string name) {
        if (!Directory.Exists(DIRECTORYPATH)) {
            Directory.CreateDirectory(DIRECTORYPATH);
        }
        string stringData = jnode.ToString();
        string prefix = fileNames.Count.ToString()+"_";
        File.WriteAllText(DIRECTORYPATH + name + FILENAME, stringData);
    }
    public static string LoadPatternFile(string name) {
        string result = "";
        foreach (string s in fileNames) {
            if (s.Contains(name) &&
                !s.Contains(".meta")) {
                Debug.Log("found :" + s);
                result = File.ReadAllText(s);
            }
        }
        return result;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
