from doctest import master
import math
import time
import json

DT = 1.0/500  # 2ms

class PoseHandler:
    def __init__(self):
        self.currentPose = []
        self.master = ""

    def GetPoseFromHololens(self, data):
        start = time.time()

    def GetPose(self):
        return self.currentPose

    def SetPose(self, jsonData):
        jd = jsonData
        if (self.master == jd['client_type']):
            pose = jd['pose'] #要注意格式
            self.TransformPose(pose)

    def TransformPose(self, splitPacket,mode):
        start = time.time()
        #print(splitPacket[0]+","+splitPacket[1]+","+splitPacket[2]+","+splitPacket[3]+","+splitPacket[4]+","+splitPacket[5]+","+splitPacket[6])
        if(mode == "1"):
            pose = [(float)(splitPacket[0]),(float)(splitPacket[1]),(float)(splitPacket[2]),(float)(splitPacket[3]),(float)(splitPacket[4]),(float)(splitPacket[5])]
            #print("pose: ",pose)
            pose = [pose[0]*(math.pi/180),pose[1]*(math.pi/180),pose[2]*(math.pi/180),pose[3]*(math.pi/180),pose[4]*(math.pi/180),pose[5]*(math.pi/180)]
            self.currentPose = pose
            #rtde_c.servoJ(pose, velocity, acceleration, dt, lookahead_time, gain)
            end = time.time()
            duration = end - start
            if duration < DT:
                time.sleep(DT - duration)
        elif(mode == "0"):
            pose = [(float)(splitPacket[0]),(float)(splitPacket[1]),(float)(splitPacket[2]),(float)(splitPacket[3]),(float)(splitPacket[4]),(float)(splitPacket[5])]
            #print(pose)
            pose = [pose[0]*(math.pi/180),pose[1]*(math.pi/180),pose[2]*(math.pi/180),pose[3]*(math.pi/180),pose[4]*(math.pi/180),pose[5]*(math.pi/180)]
            #rtde_c.moveJ(pose)
            self.currentPose = pose
        else:
            print("Error Code")