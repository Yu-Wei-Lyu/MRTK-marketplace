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

            //�e���O�o�Hemail�᭱�O��ܪ��W��
            mail.From = new MailAddress(SENDER_GMAIL, "�Ȥ�q���T");

            //���H��email
            mail.To.Add(RECEIVER_GMAIL);

            //�]�w�u���v
            mail.Priority = MailPriority.Normal;

            //���D
            mail.Subject = mailInfomation.Username;

            //���e
            mail.Body = mailInfomation.MailContent;

            //���e�ϥ�html
            mail.IsBodyHtml = true;

            //�]�wgmail��smtp (�o�Ogoogle��)
            SmtpClient MySmtp = new SmtpClient(GMAIL_HOST, GMAIL_PORT);

            //�z�bgmail���b���K�X
            MySmtp.Credentials = new NetworkCredential(SENDER_GMAIL, SENDER_VERITY_CODE);

            //�}��ssl
            MySmtp.EnableSsl = true;

            //�o�e�l��
            MySmtp.Send(mail);

            //�񱼫ŧi�X�Ӫ�MySmtp
            MySmtp = null;

            //�񱼫ŧi�X�Ӫ�mail
            mail.Dispose();
        }
    }
}