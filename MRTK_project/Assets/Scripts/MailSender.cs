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
        private DataManager _dataManager;
        [SerializeField]
        private string _appScriptUrl;
        [SerializeField]
        private string _hostEmail;

        // Send post request
        private IEnumerator SendPostRequest(MailInfomation infomation)
        {
            var sendData = new { recipientSeller = _hostEmail, recipientBuyer = infomation.UserMail, subject = infomation.Username, body = infomation.MailContent };
            string jsonData = JsonConvert.SerializeObject(sendData);
            PopupDialog popupDialog = _dataManager.GetDialogController();
            UnityWebRequest request = UnityWebRequest.Post(_appScriptUrl, jsonData);
            request.SetRequestHeader("Content-Type", "application/json");
            popupDialog.LoadingDialog("訂單發送中...");
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
                    Debug.Log("訂單成功寄出！ 已將訂單寄送給商家和您的信箱: " + infomation.UserMail);
                    popupDialog.ConfirmDialog("訂單成功寄出！", $"已將訂單寄送給商家和您的信箱: {infomation.UserMail}");
                }
                else if (responseText == "Email is not defined" || responseText.Contains("無效的電子郵件"))
                {
                    Debug.Log("訂單未成功寄出 確認您的信箱是否有誤: " + infomation.UserMail);
                    popupDialog.ConfirmDialog("訂單未成功寄出", $"確認您的信箱是否有誤: {infomation.UserMail}");
                }
                else
                {
                    Debug.Log(responseText);
                    popupDialog.ConfirmDialog("未知錯誤", $"{responseText}");
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