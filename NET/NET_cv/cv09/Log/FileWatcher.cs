using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    /// <summary>
    /// Typ zmeny
    /// </summary>
    public enum FileWatcherChangedType
    {
        None, Added, Removed, Modified
    }

    /// <summary>
    /// Argument udalosti
    /// </summary>
    public class FileWatcherChangedEventArgs
    {
        public byte[] OriginalData { get; set; }
        public byte[] NewData { get; set; }
        public int ChangePosition { get; set; }
        public FileWatcherChangedType Type { get; set; }
    }

    /// <summary>
    /// Udalost
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="evetArgs"></param>
    public delegate void FileWatcherChanged(object sender, FileWatcherChangedEventArgs evetArgs);

    /// <summary>
    /// Sledovani souboru
    /// </summary>
    public class FileWatcher
    {
        byte[] originalData;
        byte[] newData;
        string path;

        FileSystemWatcher watcher;

        public FileWatcher(string path)
        {
            this.path = path;
            if (File.Exists(path))
            {
                using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    originalData = new byte[fs.Length];
                    fs.Read(originalData, 0, (int)fs.Length);
                }
            }
            watcher = new FileSystemWatcher("./", path);
            watcher.EnableRaisingEvents = true;
            watcher.Changed += watcher_Changed;
        }

        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
            watcher.Changed -= watcher_Changed;
        }

        void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                newData = new byte[fs.Length];
                fs.Read(newData, 0, (int)fs.Length);
            }

            if (Changed != null)
            {
                var eventArg = new FileWatcherChangedEventArgs()
                {
                    NewData = newData,
                    OriginalData = originalData,
                    Type = FileWatcherChangedType.None
                };

                if (newData.Length > originalData.Length)
                {
                    eventArg.Type = FileWatcherChangedType.Added;
                }
                else if (newData.Length < originalData.Length)
                {
                    eventArg.Type = FileWatcherChangedType.Removed;
                }
                else
                {
                    for (int i = 0; i < originalData.Length; i++)
                    {
                        if (newData[i] != originalData[i])
                        {
                            eventArg.Type = FileWatcherChangedType.Modified;
                            eventArg.ChangePosition = i;
                        }
                    }
                }

                if (eventArg.Type != FileWatcherChangedType.None)
                {
                    Changed(this, eventArg);
                }
            }
        }

        public event FileWatcherChanged Changed;
    }

}
