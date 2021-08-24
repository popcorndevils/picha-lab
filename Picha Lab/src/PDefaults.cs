using System;
using System.Collections.Generic;

using PichaLib;

using Godot;

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
                    Name = "[unnamed]",
                    AnimTime = 2f,
                    Cycles = PDefaults.Cycles,
                    Frames = PDefaults.Frames,
                    Pixels = PDefaults.Pixels
                }
            };
        }
    }

    internal static List<string[,]> Frames {
        get {
            return new List<string[,]>() {
                new string[,] {
                    {PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY,},
                    {PIXEL_IDS.EMPTY, PIXEL_IDS.BODY1, PIXEL_IDS.BODY1, PIXEL_IDS.BODY1, PIXEL_IDS.BODY1, PIXEL_IDS.BODY1, PIXEL_IDS.BODY1, PIXEL_IDS.EMPTY,},
                    {PIXEL_IDS.EMPTY, PIXEL_IDS.BODY1, PIXEL_IDS.BODY2, PIXEL_IDS.BODY2, PIXEL_IDS.BODY2, PIXEL_IDS.BODY2, PIXEL_IDS.BODY1, PIXEL_IDS.EMPTY,},
                    {PIXEL_IDS.EMPTY, PIXEL_IDS.BODY1, PIXEL_IDS.BODY2, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.BODY2, PIXEL_IDS.BODY1, PIXEL_IDS.EMPTY,},
                    {PIXEL_IDS.EMPTY, PIXEL_IDS.BODY1, PIXEL_IDS.BODY2, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.BODY2, PIXEL_IDS.BODY1, PIXEL_IDS.EMPTY,},
                    {PIXEL_IDS.EMPTY, PIXEL_IDS.BODY1, PIXEL_IDS.BODY2, PIXEL_IDS.BODY2, PIXEL_IDS.BODY2, PIXEL_IDS.BODY2, PIXEL_IDS.BODY1, PIXEL_IDS.EMPTY,},
                    {PIXEL_IDS.EMPTY, PIXEL_IDS.BODY1, PIXEL_IDS.BODY1, PIXEL_IDS.BODY1, PIXEL_IDS.BODY1, PIXEL_IDS.BODY1, PIXEL_IDS.BODY1, PIXEL_IDS.EMPTY,},
                    {PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY, PIXEL_IDS.EMPTY,},
                }
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

    internal static List<Cycle> Cycles {
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

            return new List<Cycle>() {
                {new Cycle() {
                    Name = "DEGRADE",
                    Policies = new List<Policy>() {
                        _policy1, _policy2
                    }}},
                {new Cycle() {
                    Name = "OUTLINE",
                    Policies = new List<Policy>() { _policy3 }}
                }};
        }
    }

    internal static Policy Policy {
        get {
            return new Policy() {
                Input = PDefaults.PIXEL_IDS.EMPTY,
                Output = PDefaults.PIXEL_IDS.EMPTY,
                Rate = 0f,
                ConditionA = ConditionTarget.NONE,
                ConditionLogic = ConditionExpression.NONE,
                ConditionB = PDefaults.PIXEL_IDS.EMPTY,
            };
        }
    }

    internal static Pixel Pixel {
        get {
            var _ran = new Random();
            var _r = (byte)(_ran.NextDouble() * 255);
            var _g = (byte)(_ran.NextDouble() * 255);
            var _b = (byte)(_ran.NextDouble() * 255);
            
            var _rP = (byte)(_ran.NextDouble() * 255);
            var _gP = (byte)(_ran.NextDouble() * 255);
            var _bP = (byte)(_ran.NextDouble() * 255);

            return new Pixel() {
                Color = Chroma.CreateFromBytes(_r, _g, _b, 255),
                Paint = Chroma.CreateFromBytes(_rP, _gP, _bP, 255),
                RandomCol = true,
                FadeDirection = FadeDirection.NORTH,
                BrightNoise = .5f,
                MinSaturation = .5f,
            };
        }
    }
    internal static class ToolHints
    {
        internal static class Canvas
        {
            internal static string AutoGenerate = "Automatically Re-Generate the Sprite\nat intervals set by 'Time-to-Gen'.";
            internal static string ReGenerate = "Re-Generate Sprite.";
            internal static string CanvasWidth = "Set width of sprite.";
            internal static string CanvasHeight = "Set height of sprite.";
        }

        internal static class Layer
        {
            internal static string NewLayer = "Add new Layer to the stack.\nCreates new Canvas if necessary.";
            internal static string DeleteLayer = "Delete Layer from Canvas.";
            internal static string OpenTemplate = "Open Template Editor.";
        }

        internal static class Pixel
        {
            internal static string NewPixel = "Create new Pixel Type.";
            internal static string Color = "When enabled, sets color used in\ngenerated layer for pixel type.";
            internal static string DeletePixel = "Delete Pixel.";
            internal static string PixelName = "Unique identifier for Pixel types. Changes are saved\nwhen pressing 'Enter' or closing the section.";
        }

        internal static class Cycle
        {
            internal static string DeleteCycle = "Delete Cycle.";
            internal static string AddPolicy = "Add New Policy to Cycle.";
            internal static string NewCycle = "Create new Cycle.";
        }

        internal static class Policy
        {
            internal static string DeletePolicy = "Delete Policy from Cycle.";
        }

        internal static class ViewPort
        {
            internal static string CenterButton = "Center Sprite in View.";
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