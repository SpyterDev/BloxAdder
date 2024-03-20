//This program was created SpyterDev :)
using System.Diagnostics;

namespace BloxAdder;
public class AvatarCopy {
    static string RobloxDir = "/Applications/RobloxPlayer.app/";
    /*
        Note Roblox is recommended to be in your Applications Folder, if it isn't in your applications folder change this string to Roblox's Directory.
        It is also important if you change this string, to remember to end this string with a "/" to show that it has subdirectories!
    */
    static string ModDir = RobloxDir+"Contents/Resources/";
    static string RootBloxAdder = "/Applications/BloxAdder/";
    //This makes a new folder and is not the program, the program (compiled in the releases) is "BloxAdder.app" it may seem a little confusing in finder
    static Thread thread = new Thread(new ThreadStart(BloxAdderMainThread));
    public static void Main(string[] args) {
        Directory.CreateDirectory(RootBloxAdder);
        //This doesn't overwrite the directory if it is already there
        thread.Start();
    }
    public static string ReturnParentDirectory(string path) {
        int i = path.Length-2;
        for (;path[i]!='/';i--);
        return path.Remove(i,path.Length-i)+"/";
    }
    private static void CopyFiles(DirectoryInfo source, DirectoryInfo target) {
        foreach (FileInfo file in source.GetFiles()) {
            if (!File.Exists(Path.Combine(target.FullName,file.Name)) || new FileInfo(Path.Combine(target.FullName,file.Name).ToString()).Length != new FileInfo(Path.Combine(file.FullName).ToString()).Length) {
                file.CopyTo(Path.Combine(target.FullName,file.Name),true);
                Console.WriteLine(Path.Combine(target.FullName,file.Name).ToString());
            }
        }
        /*
            Copies the files and compairs if they are different.
            Note this just compairs the size in bytes for speed and for simplisty, this 99% of the time will probably work
        */
        foreach (DirectoryInfo sourceSubDir in source.GetDirectories()) {
            DirectoryInfo targetSubDir = target.CreateSubdirectory(sourceSubDir.Name);
            CopyFiles(sourceSubDir,targetSubDir);
        }
        //Does the same thing for any subdirectories
    }
    public static void BloxAdderMainThread() {
        while (true) {
            DirectoryInfo RootBloxAdderDirInfo = new DirectoryInfo(RootBloxAdder);
            DirectoryInfo ModDirInfo = new DirectoryInfo(ModDir);
            bool IsRobloxOpen = false;
            foreach (Process process in Process.GetProcesses()) {
                if (process.ProcessName == "RobloxPlayer") {
                    IsRobloxOpen = true;
                }
                //Not 100% sure if this is the right process will probably still work
            }
            if (!IsRobloxOpen) {
                Stopwatch time = Stopwatch.StartNew(); //Note: The Stopwatch Is Optional
                CopyFiles(RootBloxAdderDirInfo,ModDirInfo);
                time.Stop();//Note: The Stopwatch Is Optional
                Console.WriteLine(time.ElapsedMilliseconds+"ms");//Note: The Stopwatch Is Optional
            }
            //Note: You may want to modify roblox while it is running which is NOT recommended just delete the if block here and write "CopyFiles(RootBloxAdderDirInfo,ModDirInfo);" Do not copy the quotes
            Thread.Sleep(5000);
            //Waits 5 secs before we check again, you can change the number (the method to pause the thread uses milliseconds)
        }
    }
}