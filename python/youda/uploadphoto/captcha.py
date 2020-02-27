import requests
import time
from PIL import Image 
import cv2

path = "D:\\sourcecode\\git\\study\\python\\youda\\uploadphoto\\captcha\\"


def download():
    request = requests.get('https://auokm1.auo.com/ContractSafetyMgt/Webform/Vendor/Captcha.ashx')

    fileName = generentFileName()

    with open(fileName,"wb")as f:
        f.write(request.content)
    
    return fileName

def generentFileName():
    fileName = path + time.strftime("%Y%m%d%H%M%S", time.localtime()) + ".jpg"
    return fileName

# 自适应阀值二值化
def get_dynamic_binary_image(img, fileName):
  #filename =   './out_img/' + img_name.split('.')[0] + '-binary.jpg'
  #img_name = filedir + '/' + img_name
  #print('.....' + img_name)
  #im = cv2.imread(img_name)
  im = cv2.cvtColor(img,cv2.COLOR_BGR2GRAY) #灰值化
  # 二值化
  th1 = cv2.adaptiveThreshold(im, 255, cv2.ADAPTIVE_THRESH_GAUSSIAN_C, cv2.THRESH_BINARY, 21, 1)
  cv2.imwrite(fileName,th1)
  return th1

# 点降噪
def interference_point(img,fileName, x = 0, y = 0):
    """
    9邻域框,以当前点为中心的田字框,黑点个数
    :param x:
    :param y:
    :return:
    """
    #filename =  './out_img/' + img_name.split('.')[0] + '-interferencePoint.jpg'
    # todo 判断图片的长宽度下限
    cur_pixel = img[x,y]# 当前像素点的值
    height,width = img.shape[:2]

    for y in range(0, width - 1):
      for x in range(0, height - 1):
        if y == 0:  # 第一行
            if x == 0:  # 左上顶点,4邻域
                # 中心点旁边3个点
                sum = int(cur_pixel) \
                      + int(img[x, y + 1]) \
                      + int(img[x + 1, y]) 
                      #+ int(img[x + 1, y + 1])
                if sum <= 2 * 245:
                  img[x, y] = 255
            elif x == height - 1:  # 右上顶点
                sum = int(cur_pixel) \
                      + int(img[x, y + 1]) \
                      + int(img[x - 1, y]) 
                      #+ int(img[x - 1, y + 1])
                if sum <= 2 * 245:
                  img[x, y] = 255
            else:  # 最上非顶点,6邻域
                sum = int(img[x - 1, y]) \
                      #+ int(img[x - 1, y + 1]) \
                      + int(cur_pixel) \
                      + int(img[x, y + 1]) \
                      + int(img[x + 1, y]) \
                      #+ int(img[x + 1, y + 1])
                if sum <= 2 * 245:
                  img[x, y] = 255
        elif y == width - 1:  # 最下面一行
            if x == 0:  # 左下顶点
                # 中心点旁边3个点
                sum = int(cur_pixel) \
                      + int(img[x + 1, y]) \
                      #+ int(img[x + 1, y - 1]) \
                      + int(img[x, y - 1])
                if sum <= 2 * 245:
                  img[x, y] = 255
            elif x == height - 1:  # 右下顶点
                sum = int(cur_pixel) \
                      + int(img[x, y - 1]) \
                      + int(img[x - 1, y]) \
                      + int(img[x - 1, y - 1])

                if sum <= 2 * 245:
                  img[x, y] = 0
            else:  # 最下非顶点,6邻域
                sum = int(cur_pixel) \
                      + int(img[x - 1, y]) \
                      + int(img[x + 1, y]) \
                      + int(img[x, y - 1]) \
                      + int(img[x - 1, y - 1]) \
                      + int(img[x + 1, y - 1])
                if sum <= 3 * 245:
                  img[x, y] = 0
        else:  # y不在边界
            if x == 0:  # 左边非顶点
                sum = int(img[x, y - 1]) \
                      + int(cur_pixel) \
                      + int(img[x, y + 1]) \
                      + int(img[x + 1, y - 1]) \
                      + int(img[x + 1, y]) \
                      + int(img[x + 1, y + 1])

                if sum <= 3 * 245:
                  img[x, y] = 0
            elif x == height - 1:  # 右边非顶点
                sum = int(img[x, y - 1]) \
                      + int(cur_pixel) \
                      + int(img[x, y + 1]) \
                      + int(img[x - 1, y - 1]) \
                      + int(img[x - 1, y]) \
                      + int(img[x - 1, y + 1])

                if sum <= 3 * 245:
                  img[x, y] = 0
            else:  # 具备9领域条件的
                sum = int(img[x - 1, y - 1]) \
                      + int(img[x - 1, y]) \
                      + int(img[x - 1, y + 1]) \
                      + int(img[x, y - 1]) \
                      + int(cur_pixel) \
                      + int(img[x, y + 1]) \
                      + int(img[x + 1, y - 1]) \
                      + int(img[x + 1, y]) \
                      + int(img[x + 1, y + 1])
                if sum <= 4 * 245:
                  img[x, y] = 0
    
    cv2.imwrite(fileName, img)
    return img


imgName = path + "20191206152041.jpg"

fileName = generentFileName()

img = cv2.imread(imgName)

img = get_dynamic_binary_image(img, fileName)
interference_point(img, fileName)