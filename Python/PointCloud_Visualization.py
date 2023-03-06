import numpy as np 
import matplotlib.pyplot as plt 
from mpl_toolkits.mplot3d import Axes3D


#파일 입력
f1= open("./PointCloud/0505_A1N168.txt","rt")
f2= open("./PointCloud/0505_A1N168.txt","rt")


#점 입력
dot1=[]
dot2 =[]



lines1 = f1.readlines()
lines2 = f2.readlines()
for line1 in lines1:
    point1 = line1.split()
    dot1.append([float(point1[0]),float(point1[1]),float(point1[2])])
for line2 in lines2:
    point2 = line2.split()
    dot2.append([float(point2[0]),float(point2[1]),float(point2[2])])
  
print(point1)

#그림 준비
plt.figure()

#그림 모드?
ax1= plt.axes(projection='3d')
#사이즈
ax1.set_xlim(-0.8,0.8)
ax1.set_ylim(0.8,-0.8)
ax1.set_zlim(1,-3)
#색
color1=['r','r']
marker1=['1','o']

#그리
#for x in dot1:
   # ax1.scatter(x[0],x[1],x[2],c=color1[1],marker=marker1[1],linewidths=2,alpha=0.5)

for x in dot2:

    ax1.scatter(x[0],x[1],x[2],c=color1[0],marker=marker1[0],linewidths=2,alpha=0.5)


#plt.show()

f1.close()
f2.close()

f3= open("./PointCloud/0505_A1L226.txt","rt")
f4= open("./PointCloud/0505_A1R120.txt","rt")
dot3= []
dot4= []
lines3 = f3.readlines()
lines4 = f4.readlines()
for line3 in lines3:
    point3 = line3.split()
    dot3.append([float(point3[0]),float(point3[1]),float(point3[2])])
for line4 in lines4:
    point4 = line4.split()
    dot4.append([float(point4[0]),float(point4[1]),float(point4[2])])
  
for x in dot3:
#g
    ax1.scatter(x[0],x[1],x[2],c='r',marker=marker1[0],linewidths=2,alpha=0.5)

for x in dot4:
#darkmagenta
    ax1.scatter(x[0],x[1],x[2],c='r',marker=marker1[0],linewidths=2,alpha=0.5)

f3.close()
f4.close()

plt.show()
