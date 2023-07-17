# -*- coding: utf-8 -*-
"""
Created on Fri Dec 30 21:24:39 2022

@author: thegr
"""

import mysql.connector
import datetime
import gc

maxdb = mysql.connector.connect(
    host="127.0.0.1",
    user="root",
    password="84018401",
    database="Library",
)
cursor = maxdb.cursor()

# Read

history_max_ssn = ""
cursor.execute("SELECT Ssn FROM history")
h = cursor.fetchall()
history_max_ssn = str(h[len(h) - 1])[2:10]

reserve_max_ssn = ""
cursor.execute("SELECT Ssn FROM reserve")
h = cursor.fetchall()
reserve_max_ssn = str(h[len(h) - 1])[2:9]

personal_max_id = ""
cursor.execute("SELECT Personal_id FROM person")
h = cursor.fetchall()
personal_max_id = str(h[len(h) - 1])[2:6]

member_max_id = ""
cursor.execute("SELECT Member_id FROM member")
h = cursor.fetchall()
member_max_id = str(h[len(h) - 1])[1:-2]

product_max_id = ""
cursor.execute("SELECT Product_id FROM product")
h = cursor.fetchall()
product_max_id = str(h[len(h) - 1])[1:-2]


today = datetime.datetime.strftime(datetime.datetime.now(), '%Y-%m-%d')


def get_products_by_id(product_id):

    if product_id == "":
        cursor.execute("SELECT * FROM product WHERE Is_delete=0")
        return cursor.fetchall()
    else:
        cursor.execute(
            "SELECT Title, Amount, ISBN, Author FROM product WHERE Is_delete=0 AND Product_id = " + product_id + ";")
        return cursor.fetchall()


def get_products_by_search(products_string):
    x = products_string.split(' ')
    command = "SELECT * FROM product WHERE Is_delete=0 AND ("
    for i in range(len(x)):
        if i == len(x) - 1:
            command += "Title = \'" + \
                str(x[i]) + "\' OR " + "Author = \'" + str(x[i]) + "\');"
        else:
            command += "Title = \'" + \
                str(x[i]) + "\' OR " + "Author = \'" + str(x[i]) + "\' OR "
    cursor.execute(command)
    return cursor.fetchall()


def get_admin_account(account, password):
    dict1 = {'Personal_id': "", 'User_name': "", 'Error': ""}
    error = ""
    command = "SELECT * FROM person WHERE Account = \'" + account + "\';"

    cursor.execute(command)
    #print("\n ---------------1------------\n")
    if cursor.fetchall() == []:
        error = "account error"

    else:
        command = "SELECT * FROM person WHERE Account = \'" + \
            account + "\' AND " + "Password = \'" + password + "\';"

        cursor.execute(command)
        #print("\n ---------------2------------\n")
        if cursor.fetchall() == []:
            error = "password error"
        else:
            command = "SELECT Admin_id FROM administrator WHERE Personal_id IN (" + "SELECT Personal_id FROM person WHERE Account = \'" + \
                account + "\' AND " + "Password = \'" + password + "\');"

            cursor.execute(command)
            #print("\n ---------------3------------\n")
            if cursor.fetchall() == []:
                error = "identity error"
            else:
                command = "SELECT Personal_id,User_name FROM person WHERE Account = \'" + account + "\';"

                cursor.execute(command)
                #print("\n ---------------4------------\n")
                for i in cursor.fetchall():
                    dict1['Personal_id'] = i[0]
                    dict1['User_name'] = i[1]
    dict1['Error'] = error
    return dict1


def get_member_account(account, password):
    dict1 = {'Personal_id': "", 'User_name': "", 'Error': ""}
    error = ""
    command = command = "SELECT * FROM member WHERE Personal_id IN (" + \
        "SELECT Personal_id FROM person WHERE Account = \'" + \
        account + "\') AND Is_delete=0;"

    cursor.execute(command)
    #print("\n ---------------1------------\n")
    if cursor.fetchall() == []:
        error = "account error"

    else:
        command = "SELECT * FROM person WHERE Account = \'" + \
            account + "\' AND " + "Password = \'" + password + "\';"

        cursor.execute(command)
        #print("\n ---------------2------------\n")
        if cursor.fetchall() == []:
            error = "password error"
        else:
            command = "SELECT Member_id FROM member WHERE Personal_id IN (" + "SELECT Personal_id FROM person WHERE Account = \'" + \
                account + "\' AND " + "Password = \'" + password + "\');"

            cursor.execute(command)
            #print("\n ---------------3------------\n")
            if cursor.fetchall() == []:
                error = "identity error"
            else:
                command = "SELECT Personal_id,User_name FROM person WHERE Account = \'" + account + "\';"

                cursor.execute(command)
                #print("\n ---------------4------------\n")
                for i in cursor.fetchall():
                    dict1['Personal_id'] = i[0]
                    dict1['User_name'] = i[1]
    dict1['Error'] = error
    return dict1


def get_history(personal_id):
    # print(personal_id)
    command = "SELECT h.Ssn, b.Title, h.Borrowed_history, h.Return_history, h.Violated_history FROM product b, history h WHERE b.Product_id=h.Product_id AND h.Personal_id=\'" + personal_id + "\';"
    cursor.execute(command)
    return cursor.fetchall()


def get_borrowed_product(personal_id):
    command = "SELECT h.Ssn, b.Title, h.Borrowed_history, b.Author, b.ISBN, h.Return_history FROM product b, history h WHERE b.Product_id=h.Product_id AND h.Personal_id=\'" + personal_id + "\';"
    cursor.execute(command)
    return cursor.fetchall()


def get_reserve_product(personal_id):
    command = "SELECT r.Ssn, b.Title, b.Author, b.ISBN, r.Reserve_date, r.Is_cancel FROM product b, reserve r WHERE b.Is_delete=0 AND b.Product_id=r.Product_id AND r.Personal_id=\'" + personal_id + "\';"
    cursor.execute(command)
    return cursor.fetchall()


def add_history_ssn():
    global history_max_ssn
    history_max_ssn = "000000" + str(int(history_max_ssn) + 1)
    # print(history_max_ssn)


def add_reserve_ssn():
    global reserve_max_ssn
    reserve_max_ssn = "00000" + str(int(reserve_max_ssn) + 1)
    # print(history_max_ssn)


def add_personal_id():
    global personal_max_id
    personal_max_id = "00" + str(int(personal_max_id) + 1)


def add_member_id():
    global member_max_id
    member_max_id = str(int(member_max_id) + 1)


def add_product_id():
    global product_max_id
    product_max_id = str(int(product_max_id) + 1)


def set_borrowed_action(product_id, personal_id):
    add_history_ssn()
    command = "INSERT INTO history(Ssn, Personal_id, Product_id, Borrowed_history, Return_history, Violated_history) VALUES (\'" + \
        history_max_ssn + "\', \'" + personal_id + "\', \'" + \
        product_id + "\', \'" + today + "\', NULL, 0);"
    cursor.execute(command)
    # cursor.commit()


def set_reserve_action(product_id, personal_id):
    add_reserve_ssn()
    command = "INSERT INTO reserve(Ssn, Personal_id, Product_id, Reserve_date, Is_cancel) VALUES (\'" + \
        reserve_max_ssn + "\', \'" + personal_id + \
        "\', \'" + product_id + "\', \'" + today + "\', 0);"
    cursor.execute(command)
    # cursor.commit()


def set_cancel_reserve_action(ssn):
    # print(ssn)
    command = "UPDATE reserve SET Is_cancel=1 WHERE Ssn=\'" + ssn + "\';"
    cursor.execute(command)


def set_return_product_action(ssn):
    command = "UPDATE history SET Return_history=\'" + \
        today + "\' WHERE Ssn=\'" + ssn + "\';"
    cursor.execute(command)


def set_register_action(account, password, name, username, email, address, phone, country, date):
    add_personal_id()
    add_member_id()
    command = "INSERT INTO person(Personal_id, Country_name, Birthday, User_name, Account, Password, Email, Address, Name, Phone) VALUES (\'" + personal_max_id + "\', \'" + country + \
        "\', \'" + date + "\', \'" + username + "\', \'" + account + "\', \'" + password + \
        "\', \'" + email + "\', \'" + address + \
        "\', \'" + name + "\', \'" + phone + "\');"
    cursor.execute(command)

    command2 = "INSERT INTO member(Member_id, Personal_id, Is_delete) VALUES (\'" + \
        member_max_id + "\', \'" + personal_max_id + "\', 0);"
    cursor.execute(command2)


def get_account():
    command = "SELECT Personal_id, Name, User_name, Email, Address, Country_name, Birthday, Phone FROM person WHERE Personal_id IN ( SELECT Personal_id FROM member WHERE Is_delete=0 );"
    cursor.execute(command)
    return cursor.fetchall()


def set_delete_account(personal_id):
    command = "UPDATE member SET Is_delete=1 WHERE Personal_id=\'" + personal_id + "\';"
    cursor.execute(command)


def set_save_edit_action(title, count, isbn, author, product_id):
    command = "UPDATE product SET Title=\'" + title + "\', Amount=\'" + count + \
        "\', ISBN=\'" + isbn + "\', Author=\'" + \
        author + "\' WHERE Product_id=" + product_id + ";"
    cursor.execute(command)


def set_remove_product_action(product_id):
    command = "UPDATE product SET Is_delete=1 WHERE Product_id=\'" + product_id + "\';"
    cursor.execute(command)


def set_add_product_action(title, count, isbn, author):
    add_product_id()
    command = "INSERT INTO product(Product_id, ISBN, Author, Title, Amount, Borrowed_status, Is_delete) VALUES (\'" + \
        product_max_id + "\', \'" + isbn + "\', \'" + author + \
        "\', \'" + title + "\', \'" + count + "\', 0, 0);"
    cursor.execute(command)
