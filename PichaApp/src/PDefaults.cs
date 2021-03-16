using System.Collections.Generic;

using PichaLib;

static class PDefaults
{
    internal const int LAYER_WIDTH = 8;
    internal const int LAYER_HEIGHT = 8;

    internal static Layer Layer {
        get {
            return new Layer() {
                Frames = PDefaults.Frames,
                Pixels = PDefaults.Pixels,
                Cycles = PDefaults.Cycles,
                Locus = (8, 8, 0, 0),
                MirrorX = false,
                MirrorY = false,
            };
        }
    }
    
    internal static SortedList<int, int[,]> Frames {
        get {
            return new SortedList<int, int[,]> {
                {0, new int[,] {{0, 0, 0, 0, 0, 0, 0, 0},
                                {0, 1, 1, 1, 1, 1, 1, 0},
                                {0, 1, 2, 2, 2, 2, 1, 0},
                                {0, 1, 2, 0, 0, 2, 1, 0},
                                {0, 1, 2, 0, 0, 2, 1, 0},
                                {0, 1, 2, 2, 2, 2, 1, 0},
                                {0, 1, 1, 1, 1, 1, 1, 0},
                                {0, 0, 0, 0, 0, 0, 0, 0}}},
                {1, new int[,] {{0, 0, 0, 0, 0, 0, 0, 0},
                                {0, 0, 0, 0, 0, 0, 0, 0},
                                {0, 0, 2, 2, 2, 2, 0, 0},
                                {0, 0, 2, 2, 2, 2, 0, 0},
                                {0, 0, 2, 2, 2, 2, 0, 0},
                                {0, 0, 2, 2, 2, 2, 0, 0},
                                {0, 0, 0, 0, 0, 0, 0, 0},
                                {0, 0, 0, 0, 0, 0, 0, 0}}}};
        }
    }

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

    internal static SortedList<int, Dictionary<int, Policy>> Cycles {
        get {
            var _policy1 = new Policy() {
                Input = (int)PDefaults.PIXEL_IDS.BODY1,
                Output = (int)PDefaults.PIXEL_IDS.EMPTY,
                Rate = .5f
            };

            var _policy2 = new Policy() {
                Input = (int)PDefaults.PIXEL_IDS.BODY2,
                Output = (int)PDefaults.PIXEL_IDS.BODY1,
                Rate = .5f
            };

            var _policy3 = new Policy() {
                Input = (int)PDefaults.PIXEL_IDS.EMPTY,
                Output = (int)PDefaults.PIXEL_IDS.OUTLINE,
                Rate = 1f,
                ConditionA = ConditionTarget.NEIGHBOR,
                ConditionLogic = ConditionExpression.IS_NOT,
                ConditionB = (int)PDefaults.PIXEL_IDS.EMPTY,
            };

            return new SortedList<int, Dictionary<int, Policy>>() {
                {0, new Dictionary<int, Policy>() {
                        {(int)PDefaults.PIXEL_IDS.BODY1, _policy1},
                        {(int)PDefaults.PIXEL_IDS.BODY2, _policy2}
                    }
                },
                {1, new Dictionary<int, Policy>() {
                        {(int)PDefaults.PIXEL_IDS.EMPTY, _policy3}
                    }
                }};
        }
    }

    private enum PIXEL_IDS
    {
        EMPTY = 0,
        BODY1 = 1,
        BODY2 = 2,
        OUTLINE = 3,
    }
}