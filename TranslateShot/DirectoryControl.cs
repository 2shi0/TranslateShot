using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslateShot
{
    internal class DirectoryControl
    {

        //ダウンロードフォルダ名
        public readonly string folderPath = "downloads";

        public void Init()
        {
            //ダウンロードフォルダ作成
            if (Directory.Exists(folderPath))
            {
                Clean();
            }
            else
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        public void Clean()
        {
            DirectoryInfo di = new DirectoryInfo(folderPath+"\\");
            FileInfo[] files = di.GetFiles();
            foreach (FileInfo file in files)
            {
                file.Delete();
            }
        }

        /*
        public async void Watch()
        {
            await Task.Run(() =>
            {
            });


        }
        */

        System.IO.FileSystemWatcher watcher = null;

        public void WatchStart()
        {
            if (watcher != null) return;

            watcher = new System.IO.FileSystemWatcher();
            //監視するディレクトリを指定
            watcher.Path = folderPath;
            //最終アクセス日時、最終更新日時、ファイル、フォルダ名の変更を監視する
            watcher.NotifyFilter =
                (System.IO.NotifyFilters.LastAccess
                | System.IO.NotifyFilters.LastWrite
                | System.IO.NotifyFilters.FileName
                | System.IO.NotifyFilters.DirectoryName);
            //すべてのファイルを監視
            watcher.Filter = "";
            //UIのスレッドにマーシャリングする
            //コンソールアプリケーションでの使用では必要ない
            //watcher.SynchronizingObject = this;

            //イベントハンドラの追加
            watcher.Changed += new System.IO.FileSystemEventHandler(watcher_Changed);
            watcher.Created += new System.IO.FileSystemEventHandler(watcher_Changed);
            watcher.Deleted += new System.IO.FileSystemEventHandler(watcher_Changed);

            //監視を開始する
            watcher.EnableRaisingEvents = true;
            Debug.WriteLine("監視を開始しました。");
        }

        public void WatchEnd()
        {
            //監視を終了
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();
            watcher = null;
            Debug.WriteLine("監視を終了しました。");
        }

        //イベントハンドラ
        private void watcher_Changed(System.Object source,
            System.IO.FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case System.IO.WatcherChangeTypes.Changed:
                    Debug.WriteLine(
                        "ファイル 「" + e.FullPath + "」が変更されました。");
                    break;
                case System.IO.WatcherChangeTypes.Created:
                    Debug.WriteLine(
                        "ファイル 「" + e.FullPath + "」が作成されました。");
                    break;
                case System.IO.WatcherChangeTypes.Deleted:
                    Debug.WriteLine(
                        "ファイル 「" + e.FullPath + "」が削除されました。");
                    break;
            }
        }
    }
}
