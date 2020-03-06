import os
from util.Utils import Utils
from util.MyLogger import MyLogger
from util.CompressImage import compress_image

fileName = "process/test.xlsx"
newFileName = fileName.replace("xlsx", "zip")

if(os.path.exists(fileName)):
    os.rename(fileName, newFileName)

if(not os.path.exists(newFileName)):
    MyLogger.getLogger().debug("不存在文件{}".format(newFileName))



unzipFolder = Utils.un_zip(newFileName)
if(os.path.exists(newFileName)):
    os.remove(newFileName)

imageFolder = unzipFolder + "/xl/media"

if (not os.path.isdir(imageFolder)):
    MyLogger.getLogger().debug("excel 没有图片{}".format(imageFolder))

images = os.listdir(imageFolder)
for image in images:
    imageName = os.path.join(imageFolder, image)
    compress_image(imageName)


Utils.zip_file_path(unzipFolder, newFileName)

if(not os.path.exists(newFileName)):
    MyLogger.getLogger().debug("压缩失败{}".format(newFileName))


os.rename(newFileName, fileName)


    