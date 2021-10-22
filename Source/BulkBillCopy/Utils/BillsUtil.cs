using System.Collections.Generic;
using RimWorld;

namespace BulkBillCopy.Utils
{
    public static class BillsUtil
    {
        private static readonly List<Bill> _bills = new List<Bill>();

        public static List<Bill> Bills => _bills;

        public static void AddBill(Bill bill)
        {
            _bills.Add(bill);
        }

        public static void AddBills(IEnumerable<Bill> bills)
        {
            _bills.AddRange(bills);
        }

        public static void ClearBills()
        {
            _bills.Clear();
        }
    }
}