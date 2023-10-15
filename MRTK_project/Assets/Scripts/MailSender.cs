using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class MailSender : MonoBehaviour
    {
        [SerializeField]
        private string _appScriptUrl;
        [SerializeField]
        private string _hostEmail;

        // Send post request
        private IEnumerator SendPostRequest(MailInfomation infomation)
        {
            var sendData = new { recipientSeller = _hostEmail, recipientBuyer = infomation.UserMail, subject = infomation.Username, body = infomation.MailContent };

            // �c�حn�o�e�� JSON �ƾڡA�o�i��]�A�A�Ʊ�b Apps Script ���B�z������H��
            string jsonData = JsonConvert.SerializeObject(sendData);
            Debug.Log(jsonData);
            UnityWebRequest request = UnityWebRequest.Post(_appScriptUrl, jsonData);
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                // �i�H�B�z Apps Script ��^������H��
                string responseText = request.downloadHandler.text;
                Debug.Log("Request sent successfully, response:\n" + responseText);
                if (responseText == "Email sent successfully")
                {
                    Debug.Log("�q�榨�\�o�e");
                }
                else if (responseText == "Email is not defined")
                {
                    Debug.Log("�Ȥ� Email ���~ : " + infomation.UserMail);
                }
                else
                {
                    Debug.Log(responseText);
                }
            }
        }

        // Send gmail
        public void SendGmail(MailInfomation mailInfo)
        {
            StartCoroutine(SendPostRequest(mailInfo));
        }
    }
}