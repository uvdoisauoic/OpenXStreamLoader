using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Management;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;

namespace OpenXStreamLoader
{
    static class Utils
    {
        public static string INI_ReadValueFromFile(string strSection, string strKey, string strDefault, string strFile)
        {
            string lpReturnedString = new String(' ', 1024);
            int privateProfileString = Native.GetPrivateProfileString(ref strSection, ref strKey, ref strDefault, ref lpReturnedString, lpReturnedString.Length, ref strFile);

            return lpReturnedString.Substring(0, privateProfileString);
        }

        public static bool INI_WriteValueToFile(string strSection, string strKey, string strValue, string strFile)
        {
            return Native.WritePrivateProfileString(ref strSection, ref strKey, ref strValue, ref strFile) != 0;
        }

        public static string getNonExistingFileName(string fileName)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            string result = fileName;
            int i = 0;

            while (File.Exists(result))
            {
                i++;
                result = fileNameWithoutExtension + "(" + i + ")" + extension;
            }

            return result;
        }

        public static string pathAddBackSlash(string path)
        {
            string separator = Path.DirectorySeparatorChar.ToString();

            if (path.EndsWith(separator))
            {
                return path;
            }

            return path + separator;
        }

        public static string getFullPathWithEndingSlash(string path)
        {
            string separator = Path.DirectorySeparatorChar.ToString();

            if (path.Contains(separator))
            {
                return path.Substring(0, path.LastIndexOf(separator) + 1);
            }

            return "";
        }

        public static bool runCmd(string cmd)
        {
            Native.ProcessInformation pInfo;

            return Native.CreateProcessA(null, cmd, null, null, false,
                Native.CreateProcessFlags.NORMAL_PRIORITY_CLASS | Native.CreateProcessFlags.CREATE_NEW_CONSOLE | Native.CreateProcessFlags.CREATE_NEW_PROCESS_GROUP,
                IntPtr.Zero,
                null,
                new Native.StartupInfo(),
                out pInfo) != IntPtr.Zero;
        }

        public static string formatBytes(long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }

        public static int ToInt32Def(this string value, int defaultValue)
        {
            int result;

            return int.TryParse(value, out result) ? result : defaultValue;
        }

        public static float ToFloat32Def(this string value, float defaultValue)
        {
            float result;

            return float.TryParse(value, out result) ? result : defaultValue;
        }

        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        public static int ToInt32(this Decimal d)
        {
            return Decimal.ToInt32(d);
        }

        public static void enableDoubleBuffering(this Control control, bool enable)
        {
            var method = typeof(Control).GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);

            method.Invoke(control, new object[] { ControlStyles.OptimizedDoubleBuffer, enable });
        }

        public static void killProcessTree(int pid)
        {
            if (pid == 0)
            {
                return;
            }

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();

            foreach (ManagementObject mo in moc)
            {
                killProcessTree(Convert.ToInt32(mo["ProcessID"]));
            }

            try
            {
                Process proc = Process.GetProcessById(pid);

                proc.Kill();
            }
            catch (ArgumentException)
            {

            }
        }

        public static bool ToBoolean(this string str)
        {
            return str == "1" || str.ToLower() == "true";
        }

        public static bool isBitmapsEqual(Bitmap b1, Bitmap b2)
        {
            if (b1 == null || b2 == null || b1.Size != b2.Size)
            {
                return false;
            }

            var bd1 = b1.LockBits(new Rectangle(new Point(0, 0), b1.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var bd2 = b2.LockBits(new Rectangle(new Point(0, 0), b2.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            try
            {
                unsafe
                {
                    // Processing only upper left half width half height

                    const int noiseError = 3;

                    byte* row1 = (byte*)bd1.Scan0.ToPointer();
                    byte* row2 = (byte*)bd2.Scan0.ToPointer();
                    int widthBytes2 = b1.Size.Width * 4 / 2;
                    int height2 = b1.Size.Height / 2;
                    int difference = 0;
                    int stride = bd1.Stride / 4;
                    int threshold = b1.Size.Width / 2 * height2 * noiseError;

                    for (int j = 0; j < height2; j++)
                    {
                        for (int i = 0; i < widthBytes2; i += 4) // only red component for optimization
                        {
                            difference += Math.Abs(row1[i] - row2[i]);
                        }

                        row1 += stride;
                        row2 += stride;
                    }

                    return difference < threshold;
                }
            }
            finally
            {
                b1.UnlockBits(bd1);
                b2.UnlockBits(bd2);
            }
        }
    }
}
