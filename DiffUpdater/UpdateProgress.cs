using Ionic.Zip;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace DiffUpdater
{
    public partial class UpdateProgress : Form
    {
        private static readonly string PasswordHash = "P@@Sw0rd";
        private static readonly string SaltKey = "S@LT&KEY";
        private static readonly string VIKey = "@1B2c3D4e5F6g7H8";
        private Stopwatch sw = new Stopwatch();
       
        
        private WebClient webClient;
        private static string url;
        private static string ftpuser;
        private static string ftppw;
        private static string[] inidataArray;
        private static int localVer;
        private static int latestVer;
        private static int verToDownload;
        public UpdateProgress()
        {
            this.InitializeComponent();
            if (!File.Exists("local.ver"))
                File.WriteAllText("local.ver", Encrypt("Version=0"));
            readComments();
            calcUpdateNumbers();
            if (localVer == latestVer)
                return;
            this.richTextBox1.Text = UpdateProgress.readComments();
            this.DownloadFile(string.Concat(new object[4]
            {
                url,"/Update",calcUpdateNumbers(),".gz"
            }), "Update" + calcUpdateNumbers() + ".gz");
        }
       
        public static string Encrypt(string plainText)
        {
            byte[] bytes1 = Encoding.UTF8.GetBytes(plainText);
            byte[] bytes2 = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(32);
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Mode = CipherMode.CBC;
            rijndaelManaged.Padding = PaddingMode.Zeros;
            ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(bytes2, Encoding.ASCII.GetBytes(VIKey));
            byte[] inArray;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(bytes1, 0, bytes1.Length);
                    cryptoStream.FlushFinalBlock();
                    inArray = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(inArray);
        }

        public static string Decrypt(string encryptedText)
        {
            byte[] buffer = Convert.FromBase64String(encryptedText);
            byte[] bytes = new Rfc2898DeriveBytes(UpdateProgress.PasswordHash, Encoding.ASCII.GetBytes(UpdateProgress.SaltKey)).GetBytes(32);
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Mode = CipherMode.CBC;
            rijndaelManaged.Padding = PaddingMode.None;
            ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(bytes, Encoding.ASCII.GetBytes(UpdateProgress.VIKey));
            MemoryStream memoryStream = new MemoryStream(buffer);
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] numArray = new byte[buffer.Length];
            int count = cryptoStream.Read(numArray, 0, numArray.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(numArray, 0, count).TrimEnd("\0".ToCharArray());
        }

        private static string GetChecksum(string fullfile)
        {
            using (FileStream fileStream = System.IO.File.OpenRead(fullfile))
                return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash((Stream)fileStream)).Replace("-", string.Empty);
        }

        private void deCompress()
        {
            foreach (FileInfo fileInfo in new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles("*.gz"))
            {
                using (ZipFile zipFile = ZipFile.Read(fileInfo.ToString()))
                {
                    foreach (ZipEntry zipEntry in zipFile)
                        zipEntry.Extract(Directory.GetCurrentDirectory(), ExtractExistingFileAction.OverwriteSilently);
                }
                System.IO.File.Delete(fileInfo.ToString());
            }
        }

        public static string readComments()
        {
            if (UpdateProgress.url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                UpdateProgress.inidataArray = UpdateProgress.formattedIni(ReadIniClass.readIni(UpdateProgress.url + "/UpdateList.ini"));
                return UpdateProgress.inidataArray[2].Split('=')[1];
            }
            if (!UpdateProgress.url.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase))
                return "";
            UpdateProgress.inidataArray = UpdateProgress.formattedIni(ReadIniClass.readIni(UpdateProgress.url + "/UpdateList.ini", UpdateProgress.ftpuser, UpdateProgress.ftppw));
            return UpdateProgress.inidataArray[2].Split('=')[1];
        }

        public static bool needUpdate(string url_, string ftpuser_, string ftppw_)
        {
            UpdateProgress.url = url_;
            UpdateProgress.ftpuser = ftpuser_;
            UpdateProgress.ftppw = ftppw_;
            if (!System.IO.File.Exists("local.ver"))
                System.IO.File.WriteAllText("local.ver", UpdateProgress.Encrypt("Version=0"));
            try
            {
                if (UpdateProgress.url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                    return int.Parse(UpdateProgress.Decrypt(System.IO.File.ReadAllText("local.ver").Replace("\r\n", "")).Split('=')[1]) != int.Parse(UpdateProgress.formattedIni(ReadIniClass.readIni(UpdateProgress.url + "/UpdateList.ini"))[1].Split('=')[1]);
                if (UpdateProgress.url.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase))
                    return int.Parse(UpdateProgress.Decrypt(System.IO.File.ReadAllText("local.ver").Replace("\r\n", "")).Split('=')[1]) != int.Parse(UpdateProgress.formattedIni(ReadIniClass.readIni(UpdateProgress.url + "/UpdateList.ini", UpdateProgress.ftpuser, UpdateProgress.ftppw))[1].Split('=')[1]);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
                Environment.Exit(0);
            }
            return false;
        }

        public static int calcUpdateNumbers()
        {
            string encryptedText = System.IO.File.ReadAllText("local.ver").Replace("\r\n", "");
            UpdateProgress.latestVer = int.Parse(UpdateProgress.inidataArray[1].Split('=')[1]);
            UpdateProgress.localVer = int.Parse(UpdateProgress.Decrypt(encryptedText).Split('=')[1]);
            UpdateProgress.verToDownload = UpdateProgress.localVer + 1;
            return UpdateProgress.verToDownload;
        }

        public void DownloadFile(string urlAddress, string location)
        {
            using (this.webClient = new WebClient())
            {
                this.webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(this.Completed);
                this.webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.ProgressChanged);
                try
                {
                    if (urlAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                    {
                        Uri address = new Uri(urlAddress);
                        this.sw.Start();
                        this.webClient.DownloadFileAsync(address, location);
                    }
                    else
                    {
                        if (!urlAddress.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase))
                            return;
                        this.webClient.Credentials = (ICredentials)new NetworkCredential(UpdateProgress.ftpuser, UpdateProgress.ftppw);
                        Uri address = new Uri(urlAddress);
                        this.sw.Start();
                        this.webClient.DownloadFileAsync(address, location);
                    }
                }
                catch (Exception ex)
                {
                    int num = (int)MessageBox.Show(ex.Message);
                }
            }
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            try
            {
                if (this.labelPerc.Text != (Convert.ToDouble(e.BytesReceived) / 1024.0 / this.sw.Elapsed.TotalSeconds).ToString("0"))
                    this.labelSpeed.Text = (Convert.ToDouble(e.BytesReceived) / 1024.0 / this.sw.Elapsed.TotalSeconds).ToString("0.00") + " kb/s";
                if (this.progressBarUpdate.Value != e.ProgressPercentage)
                    this.progressBarUpdate.Value = e.ProgressPercentage;
                if (this.labelPerc.Text != e.ProgressPercentage.ToString() + "%")
                    this.labelPerc.Text = e.ProgressPercentage.ToString() + "%";
                this.labelDownloaded.Text = (Convert.ToDouble(e.BytesReceived) / 1024.0 / 1024.0).ToString("0.00") + " Mbs  /  " + (Convert.ToDouble(e.TotalBytesToReceive) / 1024.0 / 1024.0).ToString("0.00") + " Mbs";
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            this.sw.Reset();
            if (e.Cancelled)
            {
                System.IO.File.Delete("Update" + (object)UpdateProgress.calcUpdateNumbers() + ".gz");
                int num = (int)MessageBox.Show("Canceled");
            }
            else
            {
                System.IO.File.WriteAllText("local.ver", UpdateProgress.Encrypt("Version=" + (object)UpdateProgress.verToDownload));
                this.downloadLabel.Text = "Download completed!";
                Application.Restart();
            }
            this.deCompress();
        }

        public static string[] formattedIni(string s)
        {
            return s.Split(new string[1]
            {
        "\r\n"
            }, StringSplitOptions.RemoveEmptyEntries);
        }

    }
}
