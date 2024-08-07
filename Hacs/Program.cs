namespace Hacs;

using static Logger;
internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        InitMouse();
#if DEBUG
        new LogForm().Show();
#endif
        Application.Run(new HomeForm());
    }

    #region IsActive
    static bool isActive = true;
    public static bool IsActive
    {
        get => isActive;
        set
        {
            isActive = value;
            if(isActive)
                MouseHooker.Start();
            else
                MouseHooker.Stop();
        }
    }
    #endregion

    #region keys
    public static readonly KeysConfig KeysConfig = new();
    #endregion
    #region intraction
    static readonly InteractionController ic = new();
    #endregion
    #region mouse
    static void InitMouse()
    {
        MouseHooker.MouseMove += mouseInfo =>
        {
            Log($"MouseMove: {mouseInfo.pt.x}, {mouseInfo.pt.y}");
            ic.OnMouseMove(mouseInfo.pt.x, mouseInfo.pt.y);
        };

        MouseHooker.RightButtonDown += mouseInfo =>
        {
            Log($"RButtonDown: {mouseInfo.pt.x}, {mouseInfo.pt.y}");
            ic.OnRDown(mouseInfo.pt.x, mouseInfo.pt.y);
        };

        MouseHooker.RightButtonUp += mouseInfo =>
        {
            Log($"RButtonUp: {mouseInfo.pt.x}, {mouseInfo.pt.y}");
            ic.OnRUp(mouseInfo.pt.x, mouseInfo.pt.y);
        };

        MouseHooker.Start();
    }
    #endregion

}
