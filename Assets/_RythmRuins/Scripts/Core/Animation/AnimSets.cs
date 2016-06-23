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
        ETTA_SWEEP = new AnimSet
        {
            fileName = etta,
            idle        = new Pair(32, 39),
            run         = new Pair(42, 42),
            runPre      = new Pair(41, 41),
            runPost     = new Pair(43, 43),
            jump        = new Pair(47, 47),
            jumpPre     = new Pair(45, 46),
            jumpPost    = new Pair(48, 49),
            attack      = new Pair(23, 26),
            hurt        = new Pair(50, 50),
        };
        
        ETTA_POKE = new AnimSet
        {
            fileName = etta,
            idle    = new Pair(51, 55),
            run     = new Pair(60, 60),
            runPre  = new Pair(59, 59),
            runPost = new Pair(61, 61),
            jump    = new Pair(66, 66),
            jumpPre = new Pair(64, 65),
            jumpPost = new Pair(67, 67),
            attack  = new Pair(56, 58),
            hurt    = new Pair(62, 62),
        };

        ETTA_BARE = new AnimSet
        {
            fileName = etta,
            idle    = new Pair(0, 8),
            run     = new Pair(19, 19),
            runPre  = new Pair(18, 18),
            runPost = new Pair(20, 20),
            jump    = new Pair(29, 29),
            jumpPre = new Pair(27, 28),
            jumpPost = new Pair(30, 30),
            attack  = new Pair(9, 12),
            hurt    = new Pair(16, 16),
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
        TRIQ_POKE = new AnimSet
        {
            fileName     = triq,
            idle    = new Pair(51, 55),
            run     = new Pair(63, 63),
            runPre  = new Pair(62, 62),
            runPost = new Pair(64, 64),
            jump    = new Pair(68, 68),
            jumpPre = new Pair(66, 67),
            jumpPost = new Pair(69, 70),
            attack  = new Pair(57, 59),
            hurt    = new Pair(60, 60),
        };
        TRIQ_BARE = new AnimSet
        {
            fileName = triq,
            idle = new Pair(0, 5),
            run = new Pair(19, 19),
            runPre = new Pair(18, 18),
            runPost = new Pair(20, 20),
            jump = new Pair(26, 26),
            jumpPre = new Pair(24, 25),
            jumpPost = new Pair(27, 28),
            attack = new Pair(8, 11),
            hurt = new Pair(29, 29),
        };
    }
}
