using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NFS_portable_save_
{
    class Program
    {
        private static string EXENAME = "Speed.exe";
        private static string SaveGame = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\NFS Most Wanted";

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr h, string m, string c, int type);


        static void Main(string[] args)
        {
            if (!File.Exists(Directory.GetCurrentDirectory() + "\\" + EXENAME))
            {
                Console.WriteLine(Directory.GetCurrentDirectory() + "\\" + EXENAME + "\n not found");
                MessageBox((IntPtr)0, Directory.GetCurrentDirectory() + "\\" + EXENAME + "\n not found", "Error", 0);
            }
            else {
                Program program = new Program();
                program.Launch();
            }

        }

        public void Launch()
        {
            RestoreSave();
            Process pSpeed = Process.Start(EXENAME);
            pSpeed.WaitForExit();
            CopySave();
            DeleteSave();

        }

        public void CopySave()
        {
            if (Directory.Exists(SaveGame)) {
                if (!Directory.Exists("Save"))
                {
                    Directory.CreateDirectory("Save");
                }
                DirectoryCopy(SaveGame, @"Save", true);
            }
            else
            {
                MessageBox((IntPtr)0, "Save not found\n"+SaveGame, "Error", 0);
            }
        }


        public void DeleteSave()
        {
            try 
            { 
                Directory.Delete(SaveGame,true); 
            } 
            catch
            {
                MessageBox((IntPtr)0, "Delete \n" + SaveGame, "Error", 0);
            }
          
        }

        public void RestoreSave()
        {
            if (Directory.Exists("Save"))
            {
                if (!Directory.Exists(SaveGame))
                {
                    Directory.CreateDirectory(SaveGame);
                }
                else
                {
                    DirectoryCopy(SaveGame, SaveGame + "_bak", true);
                }
                DirectoryCopy(@"Save",SaveGame, true);
            }
            else
            {
                MessageBox((IntPtr)0, "Save not found\n" + SaveGame, "Error", 0);
            }
        }
        #region DirectoryCopy
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
        #endregion
    }
}
