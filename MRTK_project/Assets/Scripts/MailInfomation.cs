using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class MailInfomation
    {
        public string Username
        {
            get;
            set;
        }

        public string UserMail
        {
            get;
            set;
        }

        public string UserPhone
        {
            get;
            set;
        }

        public string MailContent
        {
            get;
            set;
        }

        public MailInfomation()
        {
            MailContent = "";
        }

        // Set user info prefix in mail content
        public void SetUserInfo()
        {
            MailContent += $"<h1>客戶名稱: {Username}</h1><h2>客戶信箱: {UserMail}</h2><h2>客戶電話: {UserPhone}</h2><h3>訂單內容</h3><ul>";
        }

        // Add shopping item into mail content
        public void AddListItem(string furnitureName, int quantity, string unitPrice, string price)
        {
            MailContent += $"<li>{furnitureName}<p>數量: {quantity} 價格: {unitPrice} 金額: {price}</p></li>";
        }

        // Add total price into mail content
        public void SetTotalPrice(string totalPrice)
        {
            MailContent += $"</ul><h2>總計: ${totalPrice}</h2>";
        }
    }
}