using Terminal.Gui;

namespace WarehouseManager.UI.Utility
{
    public static class Theme
    {
        /*
        Standard dark theme. Nhìn cho đẹp. Cách dùng: ColorScheme = Theme.Dark
        */
        public static ColorScheme Dark { get; } = new ColorScheme()
        {
            Normal = Application.Driver.MakeAttribute(Color.White, Color.Black),
            Focus = Application.Driver.MakeAttribute(Color.Black, Color.White),
            HotNormal = Application.Driver.MakeAttribute(Color.BrightYellow, Color.Black),
            HotFocus = Application.Driver.MakeAttribute(Color.Black, Color.BrightYellow)
        };

        /*
        Light theme 🤢
        */
        public static ColorScheme Light { get; } = new ColorScheme()
        {
            Normal = Application.Driver.MakeAttribute(Color.Black, Color.White),
            Focus = Application.Driver.MakeAttribute(Color.White, Color.Black),
            HotNormal = Application.Driver.MakeAttribute(Color.BrightBlue, Color.White),
            HotFocus = Application.Driver.MakeAttribute(Color.White, Color.BrightBlue)
        };

    }

}