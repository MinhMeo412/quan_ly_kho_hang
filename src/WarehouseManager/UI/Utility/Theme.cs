using Terminal.Gui;

namespace WarehouseManager.UI.Utility
{
    public static class Theme
    {
        /*
        Standard dark theme. Nhìn cho đẹp. Cách dùng: ColorScheme = Theme.SerikaDark
        */
        public static ColorScheme SerikaDark { get; } = new ColorScheme()
        {
            Normal = Application.Driver.MakeAttribute(Color.White, Color.Black),
            Focus = Application.Driver.MakeAttribute(Color.Black, Color.White),
            HotNormal = Application.Driver.MakeAttribute(Color.BrightYellow, Color.Black),
            HotFocus = Application.Driver.MakeAttribute(Color.Black, Color.BrightYellow)
        };
    }

}