from win32com.client import Dispatch    
import win32com.client 
import xlrd
import os, shutil


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


def read_excel(fileName):
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


def get_current_path():
    return os.path.dirname(__file__)

def write_excel(xls, info):
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

def start_export():
    base_dir  = get_current_path()
    file_tijian = os.path.join(base_dir,'tijian.xls')
    templete_file = os.path.join(base_dir, 'template', 'tijian.xls')

    yuyue_file = os.path.join(base_dir, 'yuyue.xls')

    if os.path.exists(file_tijian):
        os.remove(file_tijian)

    shutil.copy(templete_file, file_tijian)

    infos = read_excel(yuyue_file)

    xls = easyExcel(file_tijian)
    try:
        for info in infos:
            write_excel(xls, info)
            
    finally:
        xls.save()    
        xls.close()
    

start_export()
# if __name__ == "__main__":    
#       #PNFILE = r'c:/screenshot.bmp'  
#       name = '陈笑333'
#       sex = '女'
#       idCard = '360203198701212515'
#       luoyan_left = '1.1'
#       luoyan_right = '1.2'
#       jiaozheng_left = '1.3'
#       jiaozheng_right = '1.4'
#       height = "身高：172 cm"
#       weight = '体重：52.0 Kg'
#       date_tijian = '体检日期： 2019-04-31'
#       date_print = '打印日期： 2019-04-32'

#       xls = easyExcel(r'D:\\py\\youda\\tijian.xls')   
#       xls.addPicture('Sheet1', r'D:\\py\\youda\\test.png', 510.5,21.4,113,121)  
#       #xls.addPicture('Sheet1', PNFILE, 20,20,1000,1000)    
#       #xls.cpSheet('Sheet1')  
#       xls.setCell('sheet1',5,'B',name) 
#       xls.setCell('sheet1',5,'L',sex) 
#       xls.setCell('sheet1',5,'P',idCard) 
#       xls.setCell('sheet1',8,'D',luoyan_left)
#       xls.setCell('sheet1',9,'D',luoyan_right) 
#       xls.setCell('sheet1',8,'K',jiaozheng_left)
#       xls.setCell('sheet1',9,'K',jiaozheng_right) 
#       xls.setCell('sheet1',13,'B',height) 
#       xls.setCell('sheet1',14,'B',weight) 
#       xls.setCell('sheet1',33,'L',date_tijian) 
#       xls.setCell('sheet1',34,'L',date_print) 
     
#       #row=1  
#       #col=1  
#       print("*******beginsetCellformat********")  
#       # while(row<5):
#       #   while(col<5):
#       #       xls.setCellformat('sheet1',row,col)
#       #       col += 1
#       #       print("row=%s,col=%s" %(row,col))
#       #   row += 1
#       #   col=1
#       #   print("*******row********")
#       # print("*******endsetCellformat********")
#       # print("*******deleteRow********")
#       # xls.deleteRow('sheet1',5)
#       #xls.inserRow('sheet1',7)
#       xls.save()    
#       xls.close()  