using System;
using System.Diagnostics;
using System.IO;

namespace Test
{
    class Program
    {
        public static string CmdExcute(string cmdStr)
        {
            Process process = new Process();
            string output = "";

            IntPtr ptr = new IntPtr();

            try
            {
                process.StartInfo.FileName = @"cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WorkingDirectory = @"C:\Users\zhang\django_basic_venv\Scripts";


                if (process.Start())//开始进程
                {
                    process.StandardInput.AutoFlush = true;
                    process.StandardInput.WriteLine(cmdStr);
                    process.StandardInput.WriteLine("exit");
                    output = process.StandardOutput.ReadToEnd();
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                if (process != null)
                {
                    process.Close();
                }
            }
            return output;
        }



        /// <summary>
        /// 运行cmd命令
        /// 不显示命令窗口
        /// </summary>
        /// <param name="cmdExe">指定应用程序的完整路径</param>
        /// <param name="cmdStr">执行命令行参数</param>
        static bool RunCmd2(string cmdExe, string cmdStr)
        {
            bool result = false;
            try
            {
                using (Process myPro = new Process())
                {
                    myPro.StartInfo.FileName = "cmd.exe";
                    myPro.StartInfo.UseShellExecute = false;
                    myPro.StartInfo.RedirectStandardInput = true;
                    myPro.StartInfo.RedirectStandardOutput = true;
                    myPro.StartInfo.RedirectStandardError = true;
                    myPro.StartInfo.CreateNoWindow = true;
                    myPro.Start();
                    //如果调用程序路径中有空格时，cmd命令执行失败，可以用双引号括起来 ，在这里两个引号表示一个引号（转义）
                    string str = string.Format(@"""{0}"" {1}", cmdExe, cmdStr);
                    myPro.StandardInput.WriteLine("H:");
                    myPro.StandardInput.WriteLine(@"{0} {1}", "cd", "H:\\PycharmProjects\\scrapyTest\\mySpider");
                    myPro.StandardInput.WriteLine(str);
                    myPro.StandardInput.AutoFlush = true;
                    StreamReader reader = myPro.StandardOutput;//截取输出流
                    string line = reader.ReadLine();//每次读取一行
                    Console.WriteLine(line);
                    while (!reader.EndOfStream)
                    {
                        Console.WriteLine(reader.ReadLine());
                    }
                    myPro.WaitForExit();

                    result = true;
                }
            }
            catch
            {

            }
            return result;
        }


        static void Main(string[] args)
        {
            //RunCmd2(@"C:\Users\zhang\django_basic_venv\Scripts\python.exe",@"H:/PycharmProjects/scrapyTest/mySpider/main.py");
            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}
