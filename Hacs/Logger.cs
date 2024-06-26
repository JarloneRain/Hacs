namespace Hacs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Logger {
    public static readonly string LogPath = "hacs.log";

    public Logger() {
        if (!File.Exists(LogPath)) {
            File.Create(LogPath).Close();
        }
        File.WriteAllText(LogPath, "");
    }

    public void Log(string message) {
        File.AppendAllText(LogPath, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {message}{Environment.NewLine}");
    }
}
