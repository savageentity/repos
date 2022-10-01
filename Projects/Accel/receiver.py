#!/usr/bin/env python3

import socket
import sys
import select

host="192.168.1.65"
port = 5001
with socket.socket(socket.AF_INET,socket.SOCK_STREAM) as s:
    s.bind((host,port))
    s.listen()
    conn, addr = s.accept()
    #addr = (host,port)
    buf=1024

    conn.sendall(b'\x01')

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