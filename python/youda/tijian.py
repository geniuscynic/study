from win32com.client import Dispatch    
import win32com.client 
import xlrd
import os, shutil
import time
from ftplib import FTP
import qrcode

class easyExcel:    
      """A utility to make it easier to get at Excel.    Remembering  
      to save the data is your problem, as is    error handling.  
      Operates on one workbook at a time."""    
      def __init__(self, filename=None):  #打开文件或者新建文件（如果不存在的话）  
          self.xlApp = win32com.client.Dispatch('Excel.Application')    
          if filename:    
              self.filename = filename    
              self.xlBook = self.xlApp.Workbooks.Open(filename)    
          else:    
              self.xlBook = self.xlApp.Workbooks.Add()    
              self.filename = ''  
        
      def save(self, newfilename=None):  #保存文件  
          if newfilename:    
              self.filename = newfilename    
              self.xlBook.SaveAs(newfilename)    
          else:    
              self.xlBook.Save()        
      def close(self):  #关闭文件  
          self.xlBook.Close(SaveChanges=0)    
          del self.xlApp    
      def getCell(self, sheet, row, col):  #获取单元格的数据  
          "Get value of one cell"    
          sht = self.xlBook.Worksheets(sheet)    
          return sht.Cells(row, col).Value    
      def setCell(self, sheet, row, col, value):  #设置单元格的数据  
          "set value of one cell"    
          sht = sheet
          #sht = self.xlBook.Worksheets(sheet)    
          sht.Cells(row, col).Value = value  
      def setCellformat(self, sheet, row, col):  #设置单元格的数据  
          "set value of one cell"    
          sht = self.xlBook.Worksheets(sheet)    
          sht.Cells(row, col).Font.Size = 15#字体大小  
          sht.Cells(row, col).Font.Bold = True#是否黑体  
          sht.Cells(row, col).Name = "Arial"#字体类型  
          sht.Cells(row, col).Interior.ColorIndex = 3#表格背景  
          #sht.Range("A1").Borders.LineStyle = xlDouble  
          sht.Cells(row, col).BorderAround(1,4)#表格边框  
          sht.Rows(3).RowHeight = 30#行高  
          sht.Cells(row, col).HorizontalAlignment = -4131 #水平居中xlCenter  
          sht.Cells(row, col).VerticalAlignment = -4160 #  
      def deleteRow(self, sheet, row):  
          sht = self.xlBook.Worksheets(sheet)  
          sht.Rows(row).Delete()#删除行  
          sht.Columns(row).Delete()#删除列
      def getRange(self, sheet, row1, col1, row2, col2):  #获得一块区域的数据，返回为一个二维元组  
          "return a 2d array (i.e. tuple of tuples)"    
          sht = self.xlBook.Worksheets(sheet)  
          return sht.Range(sht.Cells(row1, col1), sht.Cells(row2, col2)).Value    
      def addPicture(self, sheet, pictureName, Left, Top, Width, Height):  #插入图片  
          "Insert a picture in sheet"    
          sht = self.xlBook.Worksheets(sheet)    
          sht.Shapes.AddPicture(pictureName, 1, 1, Left, Top, Width, Height)    
      
      def cpSheet(self, name):  #复制工作表  
          "copy sheet"    
          shts = self.xlBook.Sheets(1)
          last_sheet = self.xlBook.Sheets(self.xlBook.Sheets.Count)
          shts.Copy(None, last_sheet)
          new_sheet = self.xlBook.Sheets( self.xlBook.Sheets.Count)
          new_sheet.Name = name
          return new_sheet

      def inserRow(self,sheet,row):
          sht = self.xlBook.Worksheets(sheet)
          sht.Rows(row).Insert(1)

      #下面是一些测试代码。


class tijiandan:
    def __init__(self):
        self.__base_dir = os.path.split(os.path.realpath(__file__))[0]
        yuyue_file = os.path.join(self.__base_dir, 'yuyue.xls')
        self.__infos = self.__read_excel(yuyue_file)
        self.__folder = time.strftime("%Y%m%d", time.localtime())

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
        name = info['name']
        sex = info['sex']
        id_card = info['id_card']
        luoyan_left = info['luoyan_left']
        luoyan_right = info['luoyan_right']
        jiaozheng_left = info['jiaozheng_left']
        jiaozheng_right = info['jiaozheng_right']
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

    def start_export(self):
        tijian_file_name = 'tijian.xlsx'
        
        file_tijian = os.path.join(self.__base_dir ,tijian_file_name)
        templete_file = os.path.join(self.__base_dir , 'template', tijian_file_name)

        if os.path.exists(file_tijian):
            os.remove(file_tijian)

        shutil.copy(templete_file, file_tijian)


        xls = easyExcel(file_tijian)
        try:
            for info in self.__infos:
                self.__write_excel(xls, info)
        except Exception as ex:
            print("出错了：", ex)  
        finally:
            xls.save()    
            xls.close()

    def __write_html(self, strXml, info):
        name = info['name']
        sex = info['sex']
        id_card = info['id_card']
        id_num = info['id_num']

        tijian_file_name = 'tijian.html'
        
        file_tijian = os.path.join(self.__base_dir, id_card + ".html")

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
    
    def __upload_html_to_ftp(self,  fileName, fullName):
        
        bufsize = 1024
        
        with open(fullName, 'rb') as fp:
            self.__ftp.storbinary('STOR ' + fileName, fp, bufsize)

        os.remove(fullName)
        
                
    def __ftp_connect(self):
        self.__ftp=FTP()
        self.__ftp.set_debuglevel(2) #打开调试级别2，显示详细信息
        self.__ftp.connect("103.235.102.128") #连接的ftp sever和端口
        self.__ftp.login("24880b64b2", "jackly0909")#连接的用户名，密码
        #print(ftp.getwelcome())

        
       
        lists = self.__ftp.nlst()
        if (self.__folder not in lists):
            self.__ftp.mkd(self.__folder)

        self.__ftp.cwd(self.__folder)

    def __ftp_close(self):
        self.__ftp.close()

    def start_send_ftp(self):
       
        strXml = self.__read_html()

        #批量生成html 文件, 上传到ftp
        try:
            self.__ftp_connect()
            for info in self.__infos:
                file_name, full_name = self.__write_html(strXml, info)
                self.__upload_html_to_ftp(file_name, full_name)
        except Exception as ex:
            print("上传ftp出错：", ex)
        finally:
            self.__ftp_close()
        
        
    def start_generate_qrcode(self):
        for info in self.__infos:
            id_card = info['id_card']
            url = "http://www.588worker.cn/" + self.__folder + "/" + id_card + ".html"
            q=qrcode.main.QRCode()
            q.add_data(url)
            m=q.make_image()
            m.save("hello.png")
            break


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

# pwd = input("请输入密码：")
# while(not calculate_pwd(pwd)):
#     pwd = input("密码输入错误， 请重新输入密码：")
tijiandan = tijiandan()
tijiandan.start_generate_qrcode()

print("登入成功......")
print("开始生成二维码................")
#yuyue_file = os.path.join(base_dir, 'yuyue.xls')
#infos = read_excel('yuyue.xls')
#start_generate_qrcode(infos)
print("二维码生成结束................")
print("开始生成体检单................")
#start_export(infos)
print("体检单生成完毕................")
#x = input("按任意键退出")
