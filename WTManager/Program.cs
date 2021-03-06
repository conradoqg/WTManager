﻿using System;
using System.Threading;
using System.Windows.Forms;
using WTManager.Helpers;
using WTManager.UI;

namespace WTManager
{
    internal static class Program
    {
        private static readonly Mutex AppMutex = new Mutex(true, "27652D93-308D-475B-BC5D-417B06026CF3");
        private const string TaskName = @"WTManager";

        [STAThread]
        private static void Main(string[] args) {
            if (args.Length > 0) {
                switch (args[0]) {
                    case "/installtask":
                        SchedulerHelpers.InstallTask(TaskName);
                        break;

                    case "/removetask":
                        SchedulerHelpers.RemoveTask(TaskName);
                        break;
                }
                Environment.Exit(0);
            }
            if (AppMutex.WaitOne(TimeSpan.Zero, true)) {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
                AppMutex.ReleaseMutex();
            }
        }
    }
}