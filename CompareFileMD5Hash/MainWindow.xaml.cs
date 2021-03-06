﻿using System;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace CompareFileMD5Hash
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void file1Chooser(object sender, RoutedEventArgs e) {
            this.file1.Text = getFileName();
        }

        private void file2Chooser(object sender, RoutedEventArgs e) {
            this.file2.Text = getFileName();
        }

        private void doCompare_Click(object sender, RoutedEventArgs e) {
            var hasFile1Error = true;
            var hasFile2Error = true;
            var errMsg = "";

            if (file1.Text == string.Empty) {
                errMsg = "Please select File1.";
            } else if (!File.Exists(file1.Text)) {
                errMsg += "The File1 doesn't exist.";
            } else {
                hasFile1Error = false;
            }

            if (file2.Text == string.Empty) {
                if (errMsg.Length != 0){
                    errMsg += "\n";
                }
                errMsg += "Please select File2.";
            } else if (!File.Exists(file2.Text)) {
                if (errMsg.Length != 0)
                {
                    errMsg += "\n";
                }
                errMsg += "The File2 doesn't exist.";
            } else
            {
                hasFile2Error = false;
            }

            if (hasFile1Error || hasFile2Error) {
                result.Content = errMsg;
                return;
            }

            var file1Hash = MD5Sum(file1.Text);
            var file2Hash = MD5Sum(file2.Text);
            var resultMsg = "";

            if (file1Hash.Equals(file2Hash)) {
                resultMsg = "same md5 hash\n\n";
            } else {
                resultMsg = "different md5 hash\n\n";
            }
            resultMsg += "File1："　+ file1Hash + "\n" + "File2：" + file2Hash;
            result.Content = resultMsg;
        }

        private String getFileName() {
            var dialog = new OpenFileDialog();
            dialog.Title = "Open the file";
            dialog.Filter = "all(*.*)|*.*";
            dialog.CheckFileExists = false;
            dialog.ShowDialog();
            return dialog.FileName;
        }

        private static string MD5Sum(String filePath) {
            FileInfo file = new FileInfo(filePath);
            FileStream fs = new FileStream(file.FullName, FileMode.Open);
            string md5sum = BitConverter.ToString(MD5.Create().ComputeHash(fs)).ToLower().Replace("-", "");
            fs.Close();
            return md5sum;
        }

        private void file1_PreviewDragOver(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop, true))
            {
                e.Effects = System.Windows.DragDropEffects.Copy;
            }
            else
            {
                e.Effects = System.Windows.DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void file1_Drop(object sender, System.Windows.DragEventArgs e)
        {
            var dropFiles = e.Data.GetData(System.Windows.DataFormats.FileDrop) as string[];
            if (dropFiles == null) return;
            file1.Text = dropFiles[0];
        }

        private void file2_PreviewDragOver(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop, true))
            {
                e.Effects = System.Windows.DragDropEffects.Copy;
            }
            else
            {
                e.Effects = System.Windows.DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void file2_Drop(object sender, System.Windows.DragEventArgs e)
        {
            var dropFiles = e.Data.GetData(System.Windows.DataFormats.FileDrop) as string[];
            if (dropFiles == null) return;
            file2.Text = dropFiles[0];
        }
    }
}
