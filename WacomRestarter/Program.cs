using System.ServiceProcess;
using System.Windows.Forms;
using System.Diagnostics;

namespace WacomRestarter
{
    class Program
    {
        private const string MSGBOX_TITLE_RESTART_COMPLETED = "Wacom Service Restarter";

        static void Main(string[] args)
        {
            // Wacom プロセスを停止
            KillWacomProcess("Wacom_Tablet");
            KillWacomProcess("Wacom_TabletUser");
            KillWacomProcess("Wacom_TouchUser");
            KillWacomProcess("WacomHost");

            // Wacom サービスを再起動
            RestartWindowsService("TabletServiceWacom");
            RestartWindowsService("WTabletServicePro");

            // メッセージ
            MessageBox.Show(WacomRestarter.Properties.Resources.WacomeServiceRestartCompleted, MSGBOX_TITLE_RESTART_COMPLETED, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        }

        private static void KillWacomProcess(string processName)
        {
            try
            {
                Process[] ps = Process.GetProcessesByName(processName);

                foreach (Process item in ps)
                {
                    item.Kill();
                }

            }
            catch
            {

            }
        }

        private static void RestartWindowsService(string serviceName)
        {
            ServiceController sc = new ServiceController(serviceName);

            try
            {
                if ((sc.Status.Equals(ServiceControllerStatus.Running)) || (sc.Status.Equals(ServiceControllerStatus.StartPending)))
                {
                    sc.Stop();
                }

                sc.WaitForStatus(ServiceControllerStatus.Stopped);
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running);

            }
            catch
            {
                
            }

        }

    }
}

