using System.Globalization;
using System.Linq;
using BulkBillCopy.Textures;
using BulkBillCopy.Utils;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace BulkBillCopy.Patches
{
    
    [HarmonyPatch(typeof(ITab_Bills))]
    [HarmonyPatch("FillTab")]
    public class WorkTablePatch
    {

        private static readonly Vector2 WinSize = ReflectionHandler.Static.WinSize;
        private static readonly float PasteX = ReflectionHandler.Static.PasteX;
        private static readonly float PasteY = ReflectionHandler.Static.PasteY;
        private static readonly float PasteSize = ReflectionHandler.Static.PasteSize;
        
        
        private static string CopyTooltipText = "Copy all bills";
        private static string PasteTooltipText = "Paste all bills:";
        private static string NoPasteTooltipText = "Copied bills cannot be pasted here:";
        private static TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;
        
        static bool Prefix(ITab_Bills __instance)
        {
            var actionCopyAll = new Rect(WinSize.x - PasteX - 50, PasteY, PasteSize, PasteSize);
            var actionPasteAll = new Rect(WinSize.x - PasteX - 30, PasteY, PasteSize, PasteSize);

            var workTable = Traverse.Create(__instance).Property("SelTable").GetValue<Building_WorkTable>();
            var pasteTooltipText = PasteTooltipText;
            if (workTable.billStack.Count > 0)
            {
                if (Widgets.ButtonImageFitted(actionCopyAll, TextureHandler.CopyAll, Color.white))
                {
                    BillsUtil.ClearBills();
                    BillsUtil.AddBills(workTable.billStack.Bills);
                    Messages.Message($"Copied {BillsUtil.Bills.Count} bills", MessageTypeDefOf.NeutralEvent);
                }
            }

            if (BillsUtil.Bills.Count > 0)
            {
                if (workTable.def.AllRecipes.Contains(BillsUtil.Bills[0].recipe))
                {
                    if (Widgets.ButtonImageFitted(actionPasteAll, TextureHandler.PasteAll, Color.white))
                    {
                        foreach (var bill in BillsUtil.Bills)
                        {
                            if (workTable.billStack.Bills.Count < BillStack.MaxCount)
                            {
                                if (!workTable.def.AllRecipes.Contains(bill.recipe) && !bill.recipe.AvailableNow &&
                                    !bill.recipe.AvailableOnNow(workTable)) continue;
                                var billCopy = bill.Clone();
                                billCopy.InitializeAfterClone();
                                workTable.billStack.AddBill(billCopy);
                                SoundDefOf.Tick_Low.PlayOneShotOnCamera();
                            }
                            else
                            {
                                Messages.Message($"Reached max bill size. No more bills can be added.",
                                    MessageTypeDefOf.NeutralEvent);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Widgets.ButtonImageFitted(actionPasteAll, TexButton.Paste, Color.grey);
                    pasteTooltipText = NoPasteTooltipText;
                }
            }

            GUI.color = Color.white;
            if (Mouse.IsOver(actionCopyAll))
            {
                TooltipHandler.TipRegion(actionCopyAll, CopyTooltipText);
            }

            if (Mouse.IsOver(actionPasteAll))
            {
                TooltipHandler.TipRegion(actionPasteAll, $"{pasteTooltipText} {string.Join(", ", BillsUtil.Bills.Select(bill =>  _textInfo.ToTitleCase(bill.Label)).OfType<object>())}");
            }

            return true;
        }
    }
}