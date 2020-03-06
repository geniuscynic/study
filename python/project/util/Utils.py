import os
import zipfile
import sys

#sys.path.append(os.path.dirname(os.path.dirname(os.path.realpath(__file__))))

from util.MyLogger import MyLogger

class Utils():
    @staticmethod
    def getCurrentDir():
        return os.path.split(os.path.realpath(__file__))[0]

    @staticmethod
    def un_zip(file_name):
        """unzip zip file"""
        dir, suffix = os.path.splitext(file_name)
        zipFolder = dir.replace(suffix, "")

        if(os.path.isdir(zipFolder)):
            MyLogger.getLogger().debug("目录已经存在: {0}".format(zipFolder))
            return zipFolder
        else:
            MyLogger.getLogger().debug("解压文件: {0}".format(os.path.basename(file_name)))

        zip_file = zipfile.ZipFile(file_name)

    
        if os.path.isdir(zipFolder):
            pass
        else:
            os.mkdir(zipFolder)

        for names in zip_file.namelist():
            zip_file.extract(names,zipFolder + "/")

        return zipFolder

    @staticmethod
    def zip_file_path(input_path, output_name):
        """
        压缩文件
        :param input_path: 压缩的文件夹路径
        :param output_path: 解压（输出）的路径
        :param output_name: 压缩包名称
        :return:
        """
        MyLogger.getLogger().debug("压缩文件: {0}".format(input_path))

        filelists = []
       
        for root,dirs,files in os.walk(input_path): 
            for name in files:
                filelists.append(os.path.join(root,name))
                #print(os.path.join(root,name))

        f = zipfile.ZipFile(output_name, 'w', zipfile.ZIP_DEFLATED)
        
        for tar  in filelists:
            arcname = os.path.relpath(tar,input_path) #文件名
            f.write(tar,arcname)
        # 调用了close方法才会保证完成压缩
        f.close()



if __name__=="__main__":
    #compress_image("../process/image14.jpeg")
    dir = Utils.getCurrentDir()
    #print(os.listdir(dir + "/../process"))
    #Utils.un_zip(dir + "/../process/test.zip")
    #print(os.path.relpath(dir + "/../process/test/"))
    #os.path.isdir("../process/test/")
    
    Utils.zip_file_path(dir + "/../process/test/", dir + "/../process/test.zip")
    #print(os.path.relpath(dir + "/../process/test/", dir + "/../process/test/"))
   
    

