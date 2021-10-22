using HarmonyLib;
using RimWorld;
using UnityEngine;

namespace BulkBillCopy.Patches
{
    public class ReflectionHandler
    {
        public static class Static
        {
            private static readonly Traverse Traverse = new Traverse(typeof(ITab_Bills));
            
            public static Vector2 WinSize { get; } = Traverse.Field("WinSize").GetValue<Vector2>();
            public static float PasteX { get; } = Traverse.Field("PasteX").GetValue<float>();
            public static float PasteY { get; } = Traverse.Field("PasteY").GetValue<float>();
            public static float PasteSize { get; } = Traverse.Field("PasteSize").GetValue<float>();
        }
    }
}