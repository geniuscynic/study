import xlrd
from win32com.client import Dispatch    
import win32com.client 


def chaifen():
    xlApp = win32com.client.Dispatch('Excel.Application')
    xlApp.DisplayAlerts = False

    #filename = "D:\\workplace\\git\\study\\python\\youda\\biaogechaifen\\14.xls"    
    filename =  "D:\\sourcecode\\git\\study\\python\\youda\\biaogechaifen\\12.xls"    
    xlBook = xlApp.Workbooks.Open(filename)

    for i in range(1, xlBook.Sheets.Count+1):
        shts = xlBook.Sheets(i)
        newfilename= "D:\\sourcecode\\git\\study\\python\\youda\\biaogechaifen\\{}.xls".format(shts.Name)
        xlBook2 = xlApp.Workbooks.Add()
        #'print(xlBook2.Sheets.Count)
        #last_sheet = xlBook2.Sheets(xlBook2.Sheets.Count)
        last_sheet = xlBook2.Sheets(xlBook2.Sheets.Count)
        shts.Copy(None, last_sheet)
        xlBook2.Sheets(1).Delete()
        xlBook2.Sheets(1).Delete()
        xlBook2.Sheets(1).Delete()
        xlBook2.SaveAs(newfilename)
        xlBook2.Close() 
        print(shts.Name)

    xlBook.Close()  
    del xlApp 
# #new_sheet = xlBook2.Sheets( xlBook2.Sheets.Count)
# #new_sheet.Name = name

chaifen()

