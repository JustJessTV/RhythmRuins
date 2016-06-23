using UnityEngine;
using System.Collections;

public class AnimSets{
    public struct Pair {
        public int start;
        public int end;
        public Pair(int start,int end){
            this.start=start;
            this.end=end;
        }
    }
    public struct AnimSet {
        
        public string fileName;
        public Pair idle;
        public Pair run;
        public Pair runPre;
        public Pair runPost;
        public Pair jump;
        public Pair jumpPre;
        public Pair jumpPost;
        public Pair attack;
        public Pair hurt;

    }
    public AnimSet ETTA_SWEEP;
    public AnimSet TRIQ_SWEEP;
    public AnimSet ETTA_POKE;
    public AnimSet TRIQ_POKE;
    public AnimSet ETTA_BARE;
    public AnimSet TRIQ_BARE;
    public AnimSets() {
        Build();
    }
    public void Build() {
        string etta = "ettaCLEAN";
        string triq = "triq spritesCLEAN";
        ETTA_SWEEP = new AnimSet {
            fileName    = etta,
            idle        = new Pair(32, 39),
            run         = new Pair(42, 42),
            runPre      = new Pair(41, 41),
            runPost     = new Pair(43, 43),
            jump        = new Pair(47, 47),
            jumpPre     = new Pair(45, 46),
            jumpPost    = new Pair(48, 49),
            attack      = new Pair(23, 26),
            hurt        = new Pair(50, 50)
        };
        TRIQ_SWEEP      = new AnimSet {
            fileName    = triq,
            idle        = new Pair(30, 36),
            run         = new Pair(43, 43),
            runPre      = new Pair(42, 42),
            runPost     = new Pair(44, 44),
            jump        = new Pair(47, 47),
            jumpPre     = new Pair(46, 46),
            jumpPost    = new Pair(48, 49),
            attack      = new Pair(38, 41),
            hurt        = new Pair(50, 50)
        };
    }
}
