using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlateOrderInfomation : Plate
    {
        private const string PRICE_FORMAT_TYPE = "N0";

        [SerializeField]
        private DataManager _dataManager;
        [SerializeField]
        private TMP_Text _username;
        [SerializeField]
        private TMP_Text _userMail;
        [SerializeField]
        private TMP_Text _userPhone;

        // Send Gmail
        public void SendMailToMarket()
        {
            ShoppingCart shoppingCart = _dataManager.GetShoppingCart();
            List<int> shoppingIDList = shoppingCart.GetIDList();
            FurnitureData furnitureData;
            MailInfomation mailInfo = new MailInfomation();
            string totalPriceFormat;
            double totalPrice = 0;
            mailInfo.Username = _username.text;
            mailInfo.UserMail = _userMail.text;
            mailInfo.UserPhone = _userPhone.text;
            mailInfo.SetUserInfo();
            shoppingIDList.ForEach(furnitureID =>
            {
                int quantity = shoppingCart.GetQuantityByID(furnitureID);
                furnitureData = _dataManager.GetFurnitureDataById(furnitureID);
                string unitPrice = GetPriceFormat(furnitureData.Price);
                string furniturePriceFormat = GetPriceFormat(furnitureData.Price * quantity);
                mailInfo.AddListItem(furnitureData.Name, quantity, unitPrice, furniturePriceFormat);
                totalPrice += furnitureData.Price * quantity;
            });
            totalPriceFormat = totalPrice.ToString(PRICE_FORMAT_TYPE);
            mailInfo.SetTotalPrice(totalPriceFormat);
            MailSender mailSender = _dataManager.GetMailSender();
            mailSender.SendGmail(mailInfo);
            PopupDialog popupDialog = _dataManager.GetDialogController();
            popupDialog.ConfirmDialog("訂單已發送！");
        }

        // Set number to string by format "N0"
        private string GetPriceFormat(double price)
        {
            return price.ToString(PRICE_FORMAT_TYPE);
        }
    }
}