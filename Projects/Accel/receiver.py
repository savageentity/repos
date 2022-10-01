#!/usr/bin/env python

from socket import *
import sys
import select

host="192.168.1.65"
port = 5001
s = socket(AF_INET,SOCK_DGRAM)
s.bind((host,port))

addr = (host,port)
buf=1024

data,addr = s.recvfrom(buf)
print ("Received File:",data.strip())
f = open(data.strip(),'wb')

data,addr = s.recvfrom(buf)
try:
    while(data):
        f.write(data)
        s.settimeout(10)
        data,addr = s.recvfrom(buf)
except timeout:
    f.close()
    s.close()
    print ("File Downloaded")