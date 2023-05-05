### TCP Server Code

#import kinematics_starter
import math
import time

#import rtde_control
#import rtde_receive

import keyboard
import numpy as np
import warnings
import socket
import serial
import threading
import rtde_control
import rtde_receive

joint_list = [0,0,0,0,0,0]
position_list = [0,0,0,0,0,0]

#rtde_c = rtde_control.RTDEControlInterface("172.16.53.25")
#rtde_r = rtde_receive.RTDEReceiveInterface("172.16.53.25")
#rtde_c = rtde_control.RTDEControlInterface("192.168.50.7")
#rtde_r = rtde_receive.RTDEReceiveInterface("192.168.50.7")

pose = [0.3209,-0.11057,0.17505,2.23,-2.2195,0]
#pose = [0,-85,0,-90,0,0]
#rtde_c.moveL(pose)

host = '127.0.0.1'
#host = '172.16.53.12' #hololens airport5G
#host = '192.168.50.10' #asus801
#host = '127.168.100.1'

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
#s.bind((host,2000))
s.bind((host,2000))

s.listen(5)
print("Please connect the server first")     
clientsocket,address = s.accept()
print(f"Connection from {address} has been established!")

#robot = kinematics_starter.robot()

#t = threading.Thread(target = UR_Status)
#t.start()
previous_data = [0,-90,-90,-90,90,0]

velocity = 0.05
acceleration = 0.05
dt = 1.0/500  # 2ms
lookahead_time = 0.1
gain = 300

'''
def UR_Status():
    global joint_list,position_list
    target_pos = rtde_r.getActualTCPPose()
    target_joint = rtde_r.getTargetQ()
    for i in range(len(target_pos)):
        joint_list[i] = target_joint[i]
        position_list[i] = target_pos[i]
    #return target_pos[0],target_pos[1],target_pos[2],target_pos[3],target_pos[4],target_pos[5]
'''

while (True):
    try:
        if keyboard.is_pressed('Esc'):
            print("\nyou pressed Esc, so exiting...")
            break
        else:
            indata = clientsocket.recv(1024)
            print(indata)
            if len(indata) > 0: # connection closed
                #print(indata.decode())
                start = time.time()
                splitPacket=indata.decode().split(",")
                #print(splitPacket[0]+","+splitPacket[1]+","+splitPacket[2]+","+splitPacket[3]+","+splitPacket[4]+","+splitPacket[5]+","+splitPacket[6])
                if(splitPacket[6] == "1"):
                    pose = [(float)(splitPacket[0]),(float)(splitPacket[1]),(float)(splitPacket[2]),(float)(splitPacket[3]),(float)(splitPacket[4]),(float)(splitPacket[5])]
                    #print("pose: ",pose)
                    pose = [pose[0]*(math.pi/180),pose[1]*(math.pi/180),pose[2]*(math.pi/180),pose[3]*(math.pi/180),pose[4]*(math.pi/180),pose[5]*(math.pi/180)]
                    #rtde_c.servoJ(pose, velocity, acceleration, dt, lookahead_time, gain)
                    end = time.time()
                    duration = end - start
                    if duration < dt:
                        time.sleep(dt - duration)
                elif(splitPacket[6] == "0"):
                    pose = [(float)(splitPacket[0]),(float)(splitPacket[1]),(float)(splitPacket[2]),(float)(splitPacket[3]),(float)(splitPacket[4]),(float)(splitPacket[5])]
                    #print(pose)
                    pose = [pose[0]*(math.pi/180),pose[1]*(math.pi/180),pose[2]*(math.pi/180),pose[3]*(math.pi/180),pose[4]*(math.pi/180),pose[5]*(math.pi/180)]
                    #rtde_c.moveJ(pose)
                else:
                    print("Error Code")

                    
    except:
        
        pass


