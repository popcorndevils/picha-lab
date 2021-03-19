using System.Collections.Generic;

using PichaLib;

static class PDefaults
{
    internal const int LAYER_WIDTH = 8;
    internal const int LAYER_HEIGHT = 8;

    

    internal static GenLayer Layer {
        get {
            return new GenLayer() 
            {
                Data = new Layer() 
                {
                    Name = "[unamed]",
                    AnimTime = 3f,
                    Cycles = PDefaults.Cycles,
                    Frames = PDefaults.Frames,
                    Pixels = PDefaults.Pixels
                }
            };
        }
    }

    internal static SortedList<int, string[,]> Frames {
        get {
            return new SortedList<int, string[,]>() {
                {0, new string[,] {
                    {PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY,},
                    {PIXEL_IDS.EMPTY, PIXEL_IDS.BODY1, PIXEL_IDS.BODY1, PIXEL_IDS.BODY1, PIXEL_IDS.BODY1, PIXEL_IDS.BODY1, PIXEL_IDS.BODY1, PIXEL_IDS.EMPTY,},
                    {PIXEL_IDS.EMPTY, PIXEL_IDS.BODY1, PIXEL_IDS.BODY2, PIXEL_IDS.BODY2, PIXEL_IDS.BODY2, PIXEL_IDS.BODY2, PIXEL_IDS.BODY1, PIXEL_IDS.EMPTY,},
                    {PIXEL_IDS.EMPTY, PIXEL_IDS.BODY1, PIXEL_IDS.BODY2, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.BODY2, PIXEL_IDS.BODY1, PIXEL_IDS.EMPTY,},
                    {PIXEL_IDS.EMPTY, PIXEL_IDS.BODY1, PIXEL_IDS.BODY2, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.BODY2, PIXEL_IDS.BODY1, PIXEL_IDS.EMPTY,},
                    {PIXEL_IDS.EMPTY, PIXEL_IDS.BODY1, PIXEL_IDS.BODY2, PIXEL_IDS.BODY2, PIXEL_IDS.BODY2, PIXEL_IDS.BODY2, PIXEL_IDS.BODY1, PIXEL_IDS.EMPTY,},
                    {PIXEL_IDS.EMPTY, PIXEL_IDS.BODY1, PIXEL_IDS.BODY1, PIXEL_IDS.BODY1, PIXEL_IDS.BODY1, PIXEL_IDS.BODY1, PIXEL_IDS.BODY1, PIXEL_IDS.EMPTY,},
                    {PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY,},
                }}
            };
        }
    }

    internal static Dictionary<string, Pixel> Pixels {
        get {
            return new Dictionary<string, Pixel>() {
                {PIXEL_IDS.EMPTY, new Pixel(){
                    Name = PIXEL_IDS.EMPTY,
                    Color = Chroma.CreateFromBytes(0, 0, 0, 0),
                    Paint = Chroma.CreateFromBytes(255, 255, 255, 255),
                    RandomCol = false,
                    FadeDirection = FadeDirection.NORTH,
                    BrightNoise = .5f,
                    MinSaturation = .5f,}
                },
                {PIXEL_IDS.BODY1, new Pixel(){
                    Name = PIXEL_IDS.BODY1,
                    Color = Chroma.CreateFromBytes(0, 0, 255, 255),
                    Paint = Chroma.CreateFromBytes(0, 0, 255, 255),
                    RandomCol = true,
                    FadeDirection = FadeDirection.NORTH,
                    BrightNoise = .5f,
                    MinSaturation = .5f,}
                },
                {PIXEL_IDS.BODY2, new Pixel(){
                    Name = PIXEL_IDS.BODY2,
                    Color = Chroma.CreateFromBytes(0, 255, 0, 255),
                    Paint = Chroma.CreateFromBytes(0, 255, 0, 255),
                    RandomCol = true,
                    FadeDirection = FadeDirection.NORTH,
                    BrightNoise = .5f,
                    MinSaturation = .5f,}
                },
                {PIXEL_IDS.OUTLINE, new Pixel(){
                    Name = PIXEL_IDS.OUTLINE,
                    Color = Chroma.CreateFromBytes(0, 0, 0, 255),
                    Paint = Chroma.CreateFromBytes(0, 0, 0, 255),
                    RandomCol = false,
                    FadeDirection = FadeDirection.NONE,
                    BrightNoise = .5f,
                    MinSaturation = 0f,}
                },
            };
        }
    }

    internal static SortedList<int, Cycle> Cycles {
        get {
            var _policy1 = new Policy() {
                Input = PDefaults.PIXEL_IDS.BODY1,
                Output = PDefaults.PIXEL_IDS.EMPTY,
                Rate = .5f,
                ConditionA = ConditionTarget.NONE,
                ConditionLogic = ConditionExpression.NONE,
                ConditionB = PDefaults.PIXEL_IDS.EMPTY,
            };

            var _policy2 = new Policy() {
                Input = PDefaults.PIXEL_IDS.BODY2,
                Output = PDefaults.PIXEL_IDS.BODY1,
                Rate = .5f,
                ConditionA = ConditionTarget.NONE,
                ConditionLogic = ConditionExpression.NONE,
                ConditionB = PDefaults.PIXEL_IDS.EMPTY,
            };

            var _policy3 = new Policy() {
                Input = PDefaults.PIXEL_IDS.EMPTY,
                Output = PDefaults.PIXEL_IDS.OUTLINE,
                Rate = 1f,
                ConditionA = ConditionTarget.NEIGHBOR,
                ConditionLogic = ConditionExpression.IS_NOT,
                ConditionB = PDefaults.PIXEL_IDS.EMPTY,
            };

            return new SortedList<int, Cycle>() {
                {0, new Cycle() {
                    Name = "DEGRADE",
                    Policies = new List<Policy>() {
                        _policy1, _policy2
                    }}},
                {1, new Cycle() {
                    Name = "OUTLINE",
                    Policies = new List<Policy>() { _policy3 }}
                }};
        }
    }

    private static class PIXEL_IDS
    {
        public static string EMPTY = "EMPTY";
        public static string BODY1 = "BODY1";
        public static string BODY2 = "BODY2";
        public static string OUTLINE = "OUTLINE";
    }
}