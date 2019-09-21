from ftplib import FTP
import os

class myFtpClient: 
    def __init__(self, host='103.235.102.128', user='24880b64b2', passwd='jackly0909'):
        self.__host = host
        self.__user = user
        self.__passwd = passwd

        self.__ftp=FTP()
        # self.__ftp=FTP()
        # self.__ftp.set_debuglevel(2) #打开调试级别2，显示详细信息
        # self.__ftp.connect(host) #连接的ftp sever和端口
        # self.__ftp.login(user, passwd)#连接的用户名，密码


    def connect(self, folder = ''):
       
        #self.__ftp.set_debuglevel(2) #打开调试级别2，显示详细信息
        self.__ftp.connect(self.__host) #连接的ftp sever和端口
        self.__ftp.login(self.__user, self.__passwd)#连接的用户名，密码
        #print(ftp.getwelcome())

        
        if (folder != ""):
            lists = self.__ftp.nlst()
            if (folder not in lists):
                self.__ftp.mkd(folder)

            self.__ftp.cwd(folder)

    def close(self):
        self.__ftp.close()

    
    def upload(self,  fileName, fullName):
        
        bufsize = 1024
        
        with open(fullName, 'rb') as fp:
            self.__ftp.storbinary('STOR ' + fileName, fp, bufsize)

        #os.remove(fullName)