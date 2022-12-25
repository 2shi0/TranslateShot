using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace TranslateShot
{
    public partial class Form1 : Form
    {
        KeyboardHook keyboardHook = new KeyboardHook();
        KeySend keySend = new KeySend();
        SeleniumControl seleniumControl = new SeleniumControl();
        DirectoryControl directoryControl = new DirectoryControl();

        //TODO:
        //タスクトレイ常駐
        //重複起動検知
        //Form2起動

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //chrome driver取得
            seleniumControl.Init();

            //ダウンロードディレクトリ初期化
            directoryControl.Init();

            //ディレクトリ監視
            directoryControl.WatchStart();

            //キーフック
            keyboardHook.KeyDownEvent += KeyboardHook_KeyDownEvent;
            keyboardHook.KeyUpEvent += KeyboardHook_KeyUpEvent;
            keyboardHook.Hook();

            Debug.WriteLine("起動処理完了");

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //フック解除
            keyboardHook.UnHook();


            //ディレクトリ監視終了
            directoryControl.WatchEnd();
        }


        private void KeyboardHook_KeyDownEvent(object sender, KeyEventArg e)
        {
            // キーが押されたときにやりたいこと
            if (e.KeyCode == 121)
            {
                keySend.AltPrintScreen();
                seleniumControl.OpenWebPage();
            }

        }

        private void KeyboardHook_KeyUpEvent(object sender, KeyEventArg e)
        {
            // キーが離されたときにやりたいこと
        }
    }
}
