using UnityEngine;
using Verse;

namespace BulkBillCopy.Textures
{
    
    public static class TextureHandler
    {

        public static readonly Texture2D CopyAll = LoadTexture("CopyAll");
        public static readonly Texture2D PasteAll = LoadTexture("PasteAll");
        private static Texture2D LoadTexture(string key) => ContentFinder<Texture2D>.Get($"BulkBillCopy/{key}");
    }
}