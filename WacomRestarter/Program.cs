using System.ServiceProcess;
using System.Windows.Forms;
using System.Diagnostics;
using WacomServiceRestarter.Properties;

namespace WacomServiceRestarter
{
    class Program
    {
        /// <summary>
        /// 定数
        /// </summary>
        private const string MSGBOX_TITLE_RESTART_COMPLETED = "Wacom Service Restarter";
        private static readonly string[] WACOM_PROCESS = { "Wacom_Tablet",
                                                           "Wacom_TabletUser",
                                                           "Wacom_TouchUser",
                                                           "WacomHost" };
        private static readonly string[] WACOM_SERVICE = { "TabletServiceWacom",
                                                           "WTabletServicePro" };
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            RestartWacomMain();
        }

        /// <summary>
        /// メイン処理
        /// </summary>
        private static void RestartWacomMain()
        {
            try
            {
                // Wacom プロセスを停止
                KillProcesses(WACOM_PROCESS);

                // Wacom サービスを再起動
                RestartWindowsServices(WACOM_SERVICE);

                // 完了メッセージ
                MessageBox.Show(Resources.WacomeServiceRestartCompleted, MSGBOX_TITLE_RESTART_COMPLETED, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            catch
            {
                // 予期せぬエラー
                MessageBox.Show(Resources.ErrorUnexpected, MSGBOX_TITLE_RESTART_COMPLETED, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            finally
            {

            }

        }

        /// <summary>
        /// 指定の名称のプロセスを停止させる（配列による複数指定）
        /// </summary>
        /// <param name="processes"></param>
        private static void KillProcesses(string[] processes)
        {
            try
            {
                foreach (string processName in processes)
                {
                    KillProcess(processName);
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 指定の名称のプロセスを停止させる
        /// </summary>
        /// <param name="processName"></param>
        private static void KillProcess(string processName)
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

        private static void RestartWindowsServices(string[] services)
        {
            try
            {
                foreach (string serviceName in services)
                {
                    KillProcess(serviceName);
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 指定のWindowsサービスを再起動させる
        /// </summary>
        /// <param name="serviceName"></param>
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

