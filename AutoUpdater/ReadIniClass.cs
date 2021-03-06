﻿using System.IO;
using System.Net;


namespace AutoUpdater
{
    public class ReadIniClass
    {
        public static string readIni(string iniUrlPath)
        {
            return new StreamReader(new WebClient().OpenRead(iniUrlPath)).ReadToEnd();
        }

        public static string readIni(string iniFtpPath, string ftpuser, string ftppw)
        {
            return new StreamReader(new WebClient()
            {
                Credentials = (new NetworkCredential(ftpuser, ftppw))
            }.OpenRead(iniFtpPath)).ReadToEnd();
        }
    }
}