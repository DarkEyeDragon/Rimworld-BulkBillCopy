using System.Reflection;
using HarmonyLib;
using Verse;

namespace BulkBillCopy
{
    [StaticConstructorOnStartup]
    public static class BulkBillCopy
    {
        static BulkBillCopy()
        {
            var harmony = new Harmony("me.darkeyedragon.bulkbillcopy");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}