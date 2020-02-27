import requests
import logging
from PIL import Image
import os

logger = logging.getLogger('fib')

# 设置logger的level为DEBUG
logger.setLevel(logging.DEBUG)

# 创建一个输出日志到控制台的StreamHandler
hdr = logging.StreamHandler()
formatter = logging.Formatter('[%(asctime)s] %(name)s:%(levelname)s: %(message)s')
hdr.setFormatter(formatter)

# 给logger添加上handler
logger.addHandler(hdr)

def getLogger():
    return logger

def download(url, fileName):
    logger.debug("开始下载: {}, {}".format(fileName, url))
    down_res = requests.get(url) 
    with open(fileName,"wb") as code:
        code.write(down_res.content)

    logger.debug("下载完成: {}, {}".format(fileName, url))


def covert(fileName,  postfix):
    logging.debug("开始转换: {}".format(fileName))
    
    newFileName = fileName.replace("webp", postfix)
    im = Image.open(fileName)
    im.save(newFileName,"jpeg")
    os.remove(fileName)

    logging.debug("转换完成: {}".format(fileName))
