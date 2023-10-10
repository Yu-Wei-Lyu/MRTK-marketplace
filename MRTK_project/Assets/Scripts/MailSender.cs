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
        const string GMAIL_HOST = "smtp.gmail.com";
        const int GMAIL_PORT = 587;
        const string SENDER_GMAIL = "ntut109590004@gmail.com";
        const string SENDER_VERITY_CODE = "rltgdebodxxtwhkm";
        const string RECEIVER_GMAIL = "awaia732@gmail.com";

        // Send gmail
        public static void sendGmail(MailInfomation mailInfomation)
        {
            MailMessage mail = new MailMessage();

            //前面是發信email後面是顯示的名稱
            mail.From = new MailAddress(SENDER_GMAIL, "客戶訂單資訊");

            //收信者email
            mail.To.Add(RECEIVER_GMAIL);

            //設定優先權
            mail.Priority = MailPriority.Normal;

            //標題
            mail.Subject = mailInfomation.Username;

            //內容
            mail.Body = mailInfomation.MailContent;

            //內容使用html
            mail.IsBodyHtml = true;

            //設定gmail的smtp (這是google的)
            SmtpClient MySmtp = new SmtpClient(GMAIL_HOST, GMAIL_PORT);

            //您在gmail的帳號密碼
            MySmtp.Credentials = new NetworkCredential(SENDER_GMAIL, SENDER_VERITY_CODE);

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