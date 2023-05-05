import socket 
from threading import Thread
import time
import json
import math
from PoseHandler import PoseHandler

ADDRESS = ('140.124.182.79', 8712) 
 
g_socket_server = None  
 
g_conn_pool = {}  #儲存client IP

g_client_type_pool = {}#儲存對應IP的權限

poseHandler = PoseHandler()



def init():
    """
    初始化服务端
    """
    global g_socket_server
    g_socket_server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)  
    g_socket_server.bind(ADDRESS)
    g_socket_server.listen(5)  
    print("server start，wait for client connecting...")

def accept_client():
    """
    接收新连接
    """
    while True:
        client, info = g_socket_server.accept() 
        #print(client) 
        #print(info)
        thread = Thread(target=message_handle, args=(client, info))#開一個thread專門負責
        thread.setDaemon(True)
        thread.start()
 
 
def message_handle(client, info):
    """
    消息处理
    """
    client.sendall("connect server successfully!".encode(encoding='utf8'))
    while True:
        try:
            bytes = client.recv(1024)
            msg = bytes.decode(encoding='utf8')
            if checkJSON(msg):
                #msg = fixJSON(msg)
                jd = json.loads(msg) 
                client_id = jd['client_id']     # client ip address
                client_type = jd['client_type'] # client type : isMaster = true or false
                cmd = jd['cmd']                 # client command
                joint_list = jd['joint_list']   # joint list from master

                if  cmd == "CONNECT":  #client首次進行連線
                    g_conn_pool[client_id] = client
                    g_client_type_pool[client_id] = client_type
                    print('on client connect: ' + client_id + ":", client_type, cmd, joint_list)#info)

                elif cmd == 'SEND_DATA':    #master傳送資料給其他client，目前所有人都會送
                    print('recv client msg from ' + client_id + ":", client_type, cmd, joint_list)
                    poseHandler.currentPose = joint_list
                    SendPoseToSlave(msg)
                    poseHandler.TransformPose(joint_list,1)#傳送joint_list給手臂

                elif cmd == 'MASTER_REQUEST': #deal with master request
                    print('client',client_id,'request master')
                    master_id = GetMasterID()
                    switchMaster(client_id,master_id,msg)

                elif cmd == 'MASTER_REPLY': #Synchronize joint_list
                    print('Synchronize joint_list')
                    SendRequestToMaster(msg)

        except Exception as e:
            print(e)
            print(msg)
            print(len(msg))
            print(jd)
            remove_client(client_id)
            break

def SendPoseToSlave(msg):
    for client_id in g_conn_pool:
        if not(g_client_type_pool[client_id]):#加入判斷不傳master
            g_conn_pool[client_id].sendall(msg.encode(encoding='utf8'))
            #print("send client msg to: ",client_id,msg)

def SendRequestToMaster(msg):
    for client_id in g_conn_pool:
        if g_client_type_pool[client_id]:#只傳給master
            g_conn_pool[client_id].sendall(msg.encode(encoding='utf8'))
            #print("send client msg to: ",client_id,msg)

def GetMasterID():#回傳Master ID
    for client_id in g_client_type_pool:
        if g_client_type_pool[client_id]:
            return client_id
    return False

def switchMaster(client_id,master_id,msg):#交換master和client權限
    if  master_id != False:
        SendRequestToMaster(msg)
        g_client_type_pool[client_id] = True
        g_client_type_pool[master_id] = False
        print("master",master_id,"switch to slave")
    else:
        g_client_type_pool[client_id] = True
    print("client",client_id,"switch to master")
     
def checkJSON(msg):#檢查JSON是否完整
    if len(msg) != 1024:
        return False
    elif msg.find("{") != 0:
        return False
    else:
        return True



def remove_client(client_id):
    client = g_conn_pool[client_id]
    if None != client:
        client.close()
        g_conn_pool.pop(client_id)
        g_client_type_pool.pop(client_id)
        print("client offline: " + client_id)

if __name__ == '__main__':
    init()
    thread = Thread(target=accept_client)
    thread.setDaemon(True)
    thread.start()
    while True:
        #SendPoseToSlave()
        time.sleep(0.002)
