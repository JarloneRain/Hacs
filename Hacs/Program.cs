namespace Hacs;

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
    #region keys
    public static readonly KeysConfig KeysConfig = new();
    #endregion

    #region log
    static readonly Logger logger = new();
    public static void Log(string message) => logger.Log(message);
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
            return false;
        };

        MouseHooker.RightButtonDown += mouseInfo =>
        {
            Log($"RButtonDown: {mouseInfo.pt.x}, {mouseInfo.pt.y}");
            ic.OnRDown(mouseInfo.pt.x, mouseInfo.pt.y);
            return false;
        };

        MouseHooker.RightButtonUp += mouseInfo =>
        {
            Log($"RButtonUp: {mouseInfo.pt.x}, {mouseInfo.pt.y}");
            ic.OnRUp(mouseInfo.pt.x, mouseInfo.pt.y);
            return false;
        };

        MouseHooker.Start();
    }
    #endregion

}
