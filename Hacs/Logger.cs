namespace Hacs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class Logger
{
    public const string LogPath = "hacs.log";

    static Logger()
    {
        if(!File.Exists(LogPath))
            File.Create(LogPath).Close();
        File.WriteAllText(LogPath, "");
    }

    public static void Log(string message) =>
        File.AppendAllText(LogPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff)} {message}{Environment.NewLine}");
}
