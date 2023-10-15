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

            // 構建要發送的 JSON 數據，這可能包括你希望在 Apps Script 中處理的任何信息
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
                // 可以處理 Apps Script 返回的任何信息
                string responseText = request.downloadHandler.text;
                Debug.Log("Request sent successfully, response:\n" + responseText);
                if (responseText == "Email sent successfully")
                {
                    Debug.Log("訂單成功發送");
                }
                else if (responseText == "Email is not defined")
                {
                    Debug.Log("客戶 Email 有誤 : " + infomation.UserMail);
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