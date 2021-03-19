using System.Collections.Generic;

using PichaLib;

static class PDefaults
{
    internal const int LAYER_WIDTH = 8;
    internal const int LAYER_HEIGHT = 8;

    internal static Dictionary<int, Pixel> Pixels {
        get {
            return new Dictionary<int, Pixel>() {
                {0, new Pixel(){
                    ID = 0,
                    Name = "EMPTY",
                    Color = Chroma.CreateFromBytes(0, 0, 0, 0),
                    Paint = Chroma.CreateFromBytes(255, 255, 255, 255),
                    RandomCol = false,
                    FadeDirection = FadeDirection.NORTH,
                    BrightNoise = .5f,
                    MinSaturation = .5f,}
                },
                {1, new Pixel(){
                    ID = 1,
                    Name = "BODY1",
                    Color = Chroma.CreateFromBytes(0, 0, 255, 255),
                    Paint = Chroma.CreateFromBytes(0, 0, 255, 255),
                    RandomCol = true,
                    FadeDirection = FadeDirection.NORTH,
                    BrightNoise = .5f,
                    MinSaturation = .5f,}
                },
                {2, new Pixel(){
                    ID = 2,
                    Name = "BODY2",
                    Color = Chroma.CreateFromBytes(0, 255, 0, 255),
                    Paint = Chroma.CreateFromBytes(0, 255, 0, 255),
                    RandomCol = true,
                    FadeDirection = FadeDirection.NORTH,
                    BrightNoise = .5f,
                    MinSaturation = .5f,}
                },
                {3, new Pixel(){
                    ID = 3,
                    Name = "OUTLINE",
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
                Rate = .5f
            };

            var _policy2 = new Policy() {
                Input = PDefaults.PIXEL_IDS.BODY2,
                Output = PDefaults.PIXEL_IDS.BODY1,
                Rate = .5f
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