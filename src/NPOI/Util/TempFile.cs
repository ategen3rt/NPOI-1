
namespace NPOI.Util
{
    using System;
    using System.IO;
    using System.Threading;

    public class TempFile
    {

        private static string dir;

        private static string GetDir()
        {
            if (dir == null)
            {
                var poipath = Path.GetTempPath() + Path.DirectorySeparatorChar + "poifiles";
                var dirInfo = Directory.CreateDirectory(poipath);
                dir = dirInfo.FullName;
                //dirInfo.Attributes = FileAttributes.Temporary | FileAttributes.Directory; //Possibly makes sure readonly isn't on it??  Is this needed?
            }
            return dir;
        }

        /**
         * Creates a temporary file.  Files are collected into one directory and by default are
         * deleted on exit from the VM.  Files can be kept by defining the system property
         * <c>poi.keep.tmp.files</c>.
         * 
         * Note: there is some concern that with this method, the temp files aren't cleaned up and this can result in issues where you run out of temp file space
         * Dont forget to close all files or it might not be possible to delete them.
         * 
         * See Path.GetTempPath()
         */
        public static FileInfo CreateTempFile(String prefix, String suffix)
        {
            var d = GetDir();
            // Generate a unique new filename 
            string file = d + Path.DirectorySeparatorChar + prefix + Guid.NewGuid().ToString() + suffix;
            while (File.Exists(file))
            {
                file = d + Path.DirectorySeparatorChar + prefix + Guid.NewGuid().ToString() + suffix;
                Thread.Sleep(1);
            }
            using (FileStream newFile = new FileStream(file, FileMode.CreateNew, FileAccess.ReadWrite))
            {
                newFile.Close();
            }

            return new FileInfo(file);
        }

        /**
         * Note: Not used internally, other than exposure in the API through PackageHelper.CreateTempFile() which is not used by this library
         * Otherwise, this is just used by Test Methods.  Everything else uses CreateTempFile()
         */
        public static string GetTempFilePath(String prefix, String suffix)
        {
            var d = GetDir();
            Random rnd = new Random(DateTime.Now.Millisecond);
            Thread.Sleep(10);
            return d + Path.DirectorySeparatorChar + prefix + rnd.Next() + suffix;
        }
    }
}
