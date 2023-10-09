using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using UnityEngine;

namespace Assets.Scripts
{
    public static class MailSender
    {

        // Send gmail
        public static void sendGmail()
        {
            MailMessage mail = new MailMessage();

            //前面是發信email後面是顯示的名稱
            mail.From = new MailAddress("awaia732@gmail.com", "測試信件");

            //收信者email
            mail.To.Add("ntut109590004@gmail.com");

            //設定優先權
            mail.Priority = MailPriority.Normal;

            //標題
            mail.Subject = "AutoEmail";

            //內容
            mail.Body = "<h1>HIHI,Wellcome</h1>";

            //內容使用html
            mail.IsBodyHtml = true;

            //設定gmail的smtp (這是google的)
            SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);

            //您在gmail的帳號密碼
            MySmtp.Credentials = new System.Net.NetworkCredential("awaia732@gmail.com", "fpqdyhkyxxxfufbe");

            //開啟ssl
            MySmtp.EnableSsl = true;

            //發送郵件
            MySmtp.Send(mail);

            //放掉宣告出來的MySmtp
            MySmtp = null;

            //放掉宣告出來的mail
            mail.Dispose();
        }
    }
}