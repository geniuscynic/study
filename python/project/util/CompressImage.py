from PIL import Image
import os
from util.Utils import Utils
from util.MyLogger import MyLogger


def get_size(file):
    # 获取文件大小:KB
    size = os.path.getsize(file)
    return size / 1024

def get_outfile(infile, outfile):
    if outfile:
        return outfile
    dir, suffix = os.path.splitext(infile)
    outfile = '{}{}'.format(dir, suffix)
    return outfile

def compress_image(infile, outfile='', mb=150, step=10, quality=30):
    """不改变图片尺寸压缩到指定大小
    :param infile: 压缩源文件
    :param outfile: 压缩文件保存地址
    :param mb: 压缩目标，KB
    :param step: 每次调整的压缩比率
    :param quality: 初始压缩比率
    :return: 压缩文件地址，压缩文件大小
    """
    if (not os.path.exists(infile)):
        MyLogger.getLogger().debug("文件不存在: {}".format(infile))
        return

    o_size = get_size(infile)
    if o_size <= mb:
        return infile
    outfile = get_outfile(infile, outfile)
    while o_size > mb and step > 0:
        im = Image.open(infile)
        im.save(outfile, quality=quality)
        # if quality - step < 0:
        #     break
        # quality -= step
        step -= 1

        o_size = get_size(outfile)
        infile = outfile
        MyLogger.getLogger().debug("compress_image: {} 压缩步骤/文件大小: {} / {}".format(os.path.basename(infile), step, o_size)) 
    return outfile, get_size(outfile)

if __name__=="__main__":
    #compress_image("../process/image14.jpeg")
    dir = Utils.getCurrentDir()
    #print(os.listdir(dir + "/../process"))
    compress_image(dir + "/../process/image14.jpeg")