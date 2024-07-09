using System.Data;
using WarehouseManager.Data.Entity;
using WarehouseManager.Core.Utility;
using WarehouseManager.UI.Pages;

namespace WarehouseManager.Core.Pages
{
    public static class ChangePasswordLogic
    {

        public static void ChangePassword(string oldPassword, string newPassword)
        {

            Program.Warehouse.ChangePassword(oldPassword,newPassword);

        }

    }
}

