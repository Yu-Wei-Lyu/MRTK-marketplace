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

            //�e���O�o�Hemail�᭱�O��ܪ��W��
            mail.From = new MailAddress("awaia732@gmail.com", "���իH��");

            //���H��email
            mail.To.Add("ntut109590004@gmail.com");

            //�]�w�u���v
            mail.Priority = MailPriority.Normal;

            //���D
            mail.Subject = "AutoEmail";

            //���e
            mail.Body = "<h1>HIHI,Wellcome</h1>";

            //���e�ϥ�html
            mail.IsBodyHtml = true;

            //�]�wgmail��smtp (�o�Ogoogle��)
            SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);

            //�z�bgmail���b���K�X
            MySmtp.Credentials = new System.Net.NetworkCredential("awaia732@gmail.com", "fpqdyhkyxxxfufbe");

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