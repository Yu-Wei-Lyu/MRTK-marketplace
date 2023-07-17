# -*- coding: utf-8 -*-
"""
Created on Mon Dec 26 10:07:12 2022

@author: thegr
"""
from flask import Flask, request, render_template, jsonify

from database_subsystem import get_products_by_id, get_products_by_search, get_admin_account, get_member_account, get_history, get_borrowed_product, get_reserve_product, set_borrowed_action
from database_subsystem import set_reserve_action, set_cancel_reserve_action, set_return_product_action, set_register_action, get_account, set_delete_account, set_save_edit_action, set_remove_product_action, set_add_product_action

import datetime
import gc

app = Flask(__name__, template_folder='templates', static_folder='static')


login_id = ""
edit_product_id = ""


@app.route("/")
def home():
    return render_template('main.html')


@app.route("/admin")
def admin():
    return render_template('main_admin.html')


@app.route("/member")
def member():
    return render_template('main_member.html')


@app.route("/login")
def login():
    return render_template('login.html')


@app.route("/register")
def register():
    return render_template('register.html')


@app.route("/register_send", methods=['GET', 'POST'])
def register_send():
    record = {}
    account = request.form.get('account')
    password = request.form.get('password')
    name = request.form.get('name')
    username = request.form.get('username')
    email = request.form.get('email')
    address = request.form.get('address')
    phone = request.form.get('phone')
    country = request.form.get('country')
    date = request.form.get('date')
    #print("-------",account, password, name, username, email, address, phone, country, date)

    set_register_action(account, password, name, username,
                        email, address, phone, country, date)
    return jsonify(record)


@app.route("/login_send", methods=['GET', 'POST'])
def login_send():
    global login_id
    login_info = {}
    account = request.form.get('account')
    password = request.form.get('password')
    identity = request.form.get('identity')
    if identity == "member":
        login_info = get_member_account(account, password)
        login_id = login_info['Personal_id']
        # print(login_id)
        return jsonify(login_info)
    elif identity == "admin":
        login_info = get_admin_account(account, password)
        login_id = login_info['Personal_id']
        # print(login_id)
        return jsonify(login_info)


@app.route("/initial_load", methods=['GET', 'POST'])
def initial_load():
    product = {}
    for row in get_products_by_id(""):
        product[row[0]] = row[1:]

    return jsonify(product)


@app.route("/search_load", methods=['GET', 'POST'])
def search_load():
    search = request.form.get('searchString')
    if search == "":
        product = {}
        for row in get_products_by_id(""):
            product[row[0]] = row[1:]
            # print(row[1:])
        return jsonify(product)
    product = {}
    for row in get_products_by_search(search):
        product[row[0]] = row[1:]
        # print(row[1:])
    # print(search)
    return jsonify(product)


@app.route("/history")
def history():
    return render_template('history.html')


@app.route("/history_load", methods=['GET', 'POST'])
def history_load():
    record = {}
    for row in get_history(login_id):

        t1 = datetime.datetime.strftime(row[2], '%Y-%m-%d')
        t2 = ""
        list1 = []
        #print(type(row[3]), row[3])
        if row[3] != None:
            t2 = datetime.datetime.strftime(row[3], '%Y-%m-%d')
        else:
            t2 = "未歸還"

        list1.append(row[1])
        list1.append(t1)
        list1.append(t2)

        if row[4] == 0:
            list1.append("未逾期")
        elif row[4] == 1:
            list1.append("  逾期")
        record[row[0]] = list1
    # print(record)
    return jsonify(record)


@app.route("/borrowed")
def borrowed():
    return render_template('borrowed.html')


@app.route("/borrowed_load", methods=['GET', 'POST'])
def borrowed_load():
    record = {}
    for row in get_borrowed_product(login_id):
        if row[5] == None:
            list1 = []
            t1 = datetime.datetime.strftime(row[2], '%Y-%m-%d')
            t2 = row[2] + datetime.timedelta(days=30)
            t3 = datetime.datetime.strftime(t2, '%Y-%m-%d')
            list1.append(row[1])
            list1.append(t1)
            list1.append(t3)
            list1.append(row[3])
            list1.append(row[4])
            record[row[0]] = list1
    return jsonify(record)


@app.route("/borrowed_send", methods=['GET', 'POST'])
def borrowed_send():
    record = {}
    product_id = request.form.get('product_id')
    set_borrowed_action(product_id, login_id)
    return jsonify(record)


@app.route("/reserve")
def reserve():
    return render_template('reserve.html')


@app.route("/reserve_load", methods=['GET', 'POST'])
def reserve_load():
    record = {}
    for row in get_reserve_product(login_id):
        if row[5] == 0:
            list1 = []
            t1 = datetime.datetime.strftime(row[4], '%Y-%m-%d')
            list1.append(row[1])
            list1.append(row[2])
            list1.append(row[3])
            list1.append(t1)
            record[row[0]] = list1
    return jsonify(record)


@app.route("/reserve_send", methods=['GET', 'POST'])
def reserve_send():
    record = {}
    product_id = request.form.get('product_id')
    set_reserve_action(product_id, login_id)
    return jsonify(record)


@app.route("/cancel_reserve_send", methods=['GET', 'POST'])
def cancel_reserve_send():
    record = {}
    ssn = request.form.get('ssn')
    print(ssn)
    set_cancel_reserve_action(ssn)
    return jsonify(record)


@app.route("/return_product", methods=['GET', 'POST'])
def return_product():
    record = {}
    ssn = request.form.get('ssn')
    print(ssn)
    set_return_product_action(ssn)
    return jsonify(record)


@app.route("/edit_account")
def edit_account():
    return render_template('edit_account.html')


@app.route("/edit_account_load", methods=['GET', 'POST'])
def edit_account_load():
    record = {}
    for row in get_account():
        # print(row[0],row[1],row[2],row[3],row[4],row[5],row[6])
        list1 = []
        list1.append(row[1])
        list1.append(row[2])
        list1.append(row[3])
        list1.append(row[4])
        list1.append(row[5])
        list1.append(row[6])
        list1.append(row[7])
        record[row[0]] = list1

    return jsonify(record)


@app.route("/delete_account_send", methods=['GET', 'POST'])
def delete_account_send():
    record = {}
    personal_id = request.form.get('personal_id')
    print(personal_id)
    set_delete_account(personal_id)
    return jsonify(record)


@app.route("/edit_product")
def edit_product():
    return render_template('edit_product.html')


@app.route("/edit_product_send", methods=['GET', 'POST'])
def edit_product_send():
    record = {}
    global edit_product_id
    edit_product_id = request.form.get('product_id')
    print(edit_product_id)
    return jsonify(record)


@app.route("/edit_product_load", methods=['GET', 'POST'])
def edit_product_load():
    record = {}
    for row in get_products_by_id(edit_product_id):
        list1 = []
        list1.append(row[0])
        list1.append(row[1])
        list1.append(row[2])
        list1.append(row[3])
        record[edit_product_id] = list1
    return jsonify(record)


@app.route("/save_edit_product_send", methods=['GET', 'POST'])
def save_edit_product_send():
    record = {}
    title = request.form.get('title')
    count = request.form.get('count')
    isbn = request.form.get('isbn')
    author = request.form.get('author')
    set_save_edit_action(title, count, isbn, author, edit_product_id)
    return jsonify(record)


@app.route("/remove_product_send", methods=['GET', 'POST'])
def remove_product_send():
    record = {}
    set_remove_product_action(edit_product_id)
    return jsonify(record)


@app.route("/add_product")
def add_product():
    return render_template('add_product.html')


@app.route("/add_product_send", methods=['GET', 'POST'])
def add_product_send():
    record = {}
    title = request.form.get('title')
    count = request.form.get('count')
    isbn = request.form.get('isbn')
    author = request.form.get('author')
    set_add_product_action(title, count, isbn, author)
    return jsonify(record)


#app.debug = True
app.run(debug=False, threaded=True)
