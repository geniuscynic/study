import requests
import time
import hashlib
import random

def get_md5(ori_string):
    """
    md5 字符串
    :param ori_string:
    :return:
    """
    if isinstance(ori_string, str):
        ori_string = ori_string.encode("utf-8")

    m = hashlib.md5()
    m.update(ori_string)
    return m.hexdigest()


class ximalaya(object):

    def __init__(self):
        self.headers = {
            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.90 Safari/537.36"
        }

    def __getServerTime(self):
        """
        获取喜马拉雅服务器的时间戳
        :return: 
        """
        # 这个地址就是返回服务器时间戳的接口
        serverTimeUrl = "https://www.ximalaya.com/revision/time"
        response = requests.get(serverTimeUrl,headers = self.headers)
        return response.text

    def getSign(self):
        """
        生成 xm-sign
        规则是 md5(ximalaya-服务器时间戳)(100以内随机数)服务器时间戳(100以内随机数)现在时间戳
        :param serverTime: 
        :return: 
        """
        #nowTime = str(round(time.time()*1000))
        serverTime = self.__getServerTime()

        #sign = str(hashlib.md5("ximalaya-{}".format(serverTime).encode()).hexdigest()) + "({})".format(str(round(random.random()*100))) + serverTime + "({})".format(str(round(random.random()*100))) + nowTime
        # 将xm-sign添加到请求头中
        #self.headers["xm-sign"] = sign
        # return sign
        signText = 'ximalaya-{}'.format(serverTime)
        #print(signText)
        signText = "{}({}){}({}){}".format(get_md5(signText),
                                         random.randint(1,100),
                                         serverTime,
                                         random.randint(1,100),
                                         int(time.time()*1000))

        return signText
