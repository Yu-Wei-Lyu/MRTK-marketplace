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

        private string _listContent;

        public MailInfomation()
        {
            MailContent = "";
        }

        public void SetUserInfo()
        {
            MailContent += $"<h1>�Ȥ�W��: {Username}</h1><h2>�Ȥ�H�c: {UserMail}</h2><h2>�Ȥ�q��: {UserPhone}</h2><h3>�q�椺�e</h3><ul>";
        }

        public void AddListItem(string furnitureName, int quantity, string unitPrice, string price)
        {
            MailContent += $"<li>{furnitureName}<p>�ƶq: {quantity} ����: {unitPrice} ���B: {price}</p></li>";
        }

        public void SetTotalPrice(string totalPrice)
        {
            MailContent += $"</ul><h2>�`�p: ${totalPrice}</h2>";
        }
    }
}