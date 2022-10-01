data = b'\xff\xa1\xa2\x00\x17\x00\x00'
data_0 = b'\x00\x00\x17\x00\xa2\xa1\xff'

if data[0:3]==bytearray(b'\xff\xa1\xa2'):
    length = len(data)
    if data[length-3] & 15:
        print("data: yes")
    else:
        print("data: no")
if data_0[length-3:length]==bytearray(b'\xa2\xa1\xff'):
    length = len(data_0)
    if data_0[2] & 15:
        print("data_0: yes")
    else:
        print("data_0: no")
else:
    print(data_0[length-3:length])