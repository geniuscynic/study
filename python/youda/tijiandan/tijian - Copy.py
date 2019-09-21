
import os
import shutil
import threading
import time

import qrcode
import xlrd

from easyExcel import easyExcel
from myFtpClient import myFtpClient


class tijiandan:
    def __init__(self):
        self.__base_dir = os.path.split(os.path.realpath(__file__))[0]
        yuyue_file = os.path.join(self.__base_dir, 'yuyue.xls')
        self.__infos = self.__read_excel(yuyue_file)
        self.__folder = time.strftime("%Y%m%d", time.localtime())
        self.__ftp = myFtpClient()

    '''
    上传生成的html到ftp上
    '''
    def send_ftp(self):
       
        strXml = self.__read_html()

        #批量生成html 文件, 上传到ftp
        
        try:
            self.__ftp.connect(self.__folder)
            for info in self.__infos:
                file_name, full_name = self.__write_html(strXml, info)
                self.__ftp.upload(file_name, full_name)
                os.remove(full_name)
        except Exception as ex:
            print("上传ftp出错：", ex)
        finally:
            self.__ftp.close()
            print("上传完成")
        
    '''
    生成二维码
    更新各个sheet的信息
    '''
    def genetate_export(self):
        tijian_file_name = 'tijian.xlsx'
        
        file_tijian = os.path.join(self.__base_dir ,tijian_file_name)
        templete_file = os.path.join(self.__base_dir , 'template', tijian_file_name)

        if os.path.exists(file_tijian):
            os.remove(file_tijian)

        shutil.copy(templete_file, file_tijian)


        xls = easyExcel(file_tijian)
        try:
            for info in self.__infos:
                print("%s: 开始生成二维码..." %  info['name'])
                qcCodePath = self.__generate_qrcode(info)
                print("%s: 结束生成二维码..." %  info['name'])
                print("%s: 开始生成excel..." %  info['name'])
                sheet = self.__write_excel(xls, info)
                xls.addPicture(sheet, qcCodePath, 557,736,58,58)
                os.remove(qcCodePath)
                print("%s: 结束生成excel..." %  info['name'])
        except Exception as ex:
            print("出错了：", ex)  
        finally:
            xls.save()    
            xls.close()


    def __read_excel(self, fileName):
        # 打开文件
        workbook = xlrd.open_workbook(fileName)
        # 获取所有sheet
        #print workbook.sheet_names() # [u'sheet1', u'sheet2']
        sheet1 = workbook.sheets()[0] 
        infos = []
        for i in range(1, sheet1.nrows):
            info = {
                'name': sheet1.cell(i,1).value,
                'sex':  sheet1.cell(i,2).value,
                'id_card':  sheet1.cell(i,3).value,
                'height':  sheet1.cell(i,4).value,
                'weight':  sheet1.cell(i,5).value,
                'luoyan_left':  sheet1.cell(i,6).value,
                'luoyan_right':  sheet1.cell(i,7).value,
                'jiaozheng_left': sheet1.cell(i,8).value,
                'jiaozheng_right': sheet1.cell(i,9).value,
                'date_tijian': sheet1.cell(i,10).value,
                'date_print': sheet1.cell(i,11).value,
                'id_num': sheet1.cell(i,12).value
            }

            if(info['name'] == ""):
                break

            infos.append(info)

        return infos

    def __write_excel(self, xls, info):
        name = ' %s' % info['name']
        sex = info['sex']
        id_card = ' %s' % info['id_card']
        luoyan_left = ' %s' % info['luoyan_left']
        luoyan_right = ' %s' % info['luoyan_right']
        jiaozheng_left = ' %s' % info['jiaozheng_left']
        jiaozheng_right = ' %s' % info['jiaozheng_right']
        height = "身高：%s cm" % (info['height'])
        weight = '体重：%s Kg' % (info['weight'])
        date_tijian = '体检日期： %s' % (info['date_tijian'])
        date_print = '打印日期： %s' % (info['date_print'])
        id_num = ' * %s *' % (info['id_num'])

        sheet = xls.cpSheet(name)
        xls.setCell(sheet,4,'A',id_num) 
        xls.setCell(sheet,5,'B', name) 
        xls.setCell(sheet,5,'L',sex) 
        xls.setCell(sheet,5,'P',id_card) 
        xls.setCell(sheet,8,'D',luoyan_left)
        xls.setCell(sheet,9,'D',luoyan_right) 
        xls.setCell(sheet,8,'K',jiaozheng_left)
        xls.setCell(sheet,9,'K',jiaozheng_right) 
        xls.setCell(sheet,13,'B',height) 
        xls.setCell(sheet,14,'B',weight) 
        xls.setCell(sheet,33,'L',date_tijian) 
        xls.setCell(sheet,34,'L',date_print) 
        return sheet

    
    def __write_html(self, strXml, info):
        name = info['name']
        sex = info['sex']
        id_card = info['id_card']
        id_num = info['id_num']

        #tijian_file_name = 'tijian.html'
        
        file_tijian = os.path.join(self.__base_dir, 'tmp', id_card + ".html")

        strXml = strXml.replace("{name}", name).replace("{sex}", sex).replace("{no}", id_card).replace("{id}", id_num)
        with open(file_tijian, mode='w', encoding='utf-8') as f:
            strXml = f.write(strXml)

        return (id_card + ".html", file_tijian)

    def __read_html(self):
        tijian_file_name = 'tijian.html'
       
        templete_file = os.path.join(self.__base_dir, 'template', tijian_file_name)

        strXml = ""
        with open(templete_file, mode='r', encoding='utf-8') as f:
            strXml = f.read()

        return strXml
    
   
    def __generate_qrcode(self, info):
        id_card = info['id_card']

        qrcode_img = os.path.join(self.__base_dir, 'tmp', id_card + ".png")
       
        url = "http://www.588worker.cn/" + self.__folder + "/" + id_card + ".html"
        q=qrcode.main.QRCode()
        q.add_data(url)
        m=q.make_image()
        m.save(qrcode_img)
        return qrcode_img
    
    def __delete_all_file(self):
        folder =  os.path.join(self.__base_dir, 'tmp')
        files = os.listdir(folder)
        for deleteFile in files:
            os.remove(deleteFile)


    def start(self):
        #t1 = threading.Thread(target=tijiandan.genetate_export )
        #t1.start()
        #t1.join()

        t2 = threading.Thread(target=tijiandan.send_ftp )
        t2.start()
        

        tijiandan.genetate_export()
        t2.join()
        #

       



def calculate_pwd(pwd):
    now = time.localtime()
    #date_string = "2019-05-06"
    #now = time.strptime(date_string, "%Y-%m-%d")

    date = time.strftime("%m%d", now)
    first = int(date) * int(date[::-1])
    second = int(time.strftime("%Y%m%d", now))

    sub = str(abs(first - second))
    if (pwd == sub):
        return True
    
    return False

pwd = input("请输入密码：")
while(not calculate_pwd(pwd)):
    pwd = input("密码输入错误， 请重新输入密码：")



print("登入成功......")
tijiandan = tijiandan()
tijiandan.start()
#print("开始生成二维码................")
#yuyue_file = os.path.join(base_dir, 'yuyue.xls')
#infos = read_excel('yuyue.xls')
#start_generate_qrcode(infos)
#print("二维码生成结束................")
#print("开始生成体检单................")
#start_export(infos)
print("体检单生成完毕................")
#x = input("按任意键退出")
