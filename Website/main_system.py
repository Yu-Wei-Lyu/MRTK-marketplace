# -*- coding: utf-8 -*-
"""
Created on Mon Dec 26 10:07:12 2022

@author: thegr
"""
from flask import Flask, request, render_template, jsonify

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


@app.route("/history")
def history():
    return render_template('history.html')

@app.route("/borrowed")
def borrowed():
    return render_template('borrowed.html')


@app.route("/reserve")
def reserve():
    return render_template('reserve.html')


@app.route("/edit_product")
def edit_product():
    return render_template('edit_product.html')

@app.route("/add_product")
def add_product():
    return render_template('add_product.html')


#app.debug = True
app.run(host='0.0.0.0', port=8765, debug=False, threaded=True)

