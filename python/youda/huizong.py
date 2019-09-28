import xlrd
import xlwt
from xlutils.copy import copy
import time
from datetime import date, timedelta

#from pystrich.code128 import Code128Encoder
# 1  table = data.sheets()[0]          #通过索引顺序获取
# 2  table = data.sheet_by_index(0) #通过索引顺序获取
# 3  table = data.sheet_by_name(u'Sheet1')#通过名称获取

# 4、获取整行和整列的值（返回数组）
#          table.row_values(i)
#          table.col_values(i)
# 5、获取行数和列数　
#         table.nrows
#         table.ncols

#now = datetime.datetime.now()
def getStyle():
    style = xlwt.XFStyle() # Create Style
    return style

def getBorderStyle():
    borders = xlwt.Borders() # Create Borders
    borders.left = xlwt.Borders.DASHED # May be: NO_LINE, THIN, MEDIUM, DASHED, DOTTED, THICK, DOUBLE, HAIR, MEDIUM_DASHED, THIN_DASH_DOTTED, MEDIUM_DASH_DOTTED, THIN_DASH_DOT_DOTTED, MEDIUM_DASH_DOT_DOTTED, SLANTED_MEDIUM_DASH_DOTTED, or 0x00 through 0x0D.
    borders.right = xlwt.Borders.DASHED
    borders.top = xlwt.Borders.DASHED
    borders.bottom = xlwt.Borders.DASHED
    borders.left_colour = 0x00
    borders.right_colour = 0x00
    borders.top_colour = 0x00
    borders.bottom_colour = 0x00
    
    style = getStyle()
    style.borders = borders # Add Borders to Style
    return style

def read_huizong():
    workbook = xlrd.open_workbook('huizong.xlsx')
    sheet1 = workbook.sheets()[0] 
    infos = []
    for i in range(1, sheet1.nrows):
        info = {
            'seq': sheet1.cell(i,0).value,           #序号
            'name': sheet1.cell(i,1).value,          #姓名
            'sex':  sheet1.cell(i,2).value,          #性别
            'idcard':  sheet1.cell(i,3).value,       #身份证号
            'phone':  sheet1.cell(i,4).value,        #联系电话
            'clothes': sheet1.cell(i,5).value,       #衣码
            'shoes': sheet1.cell(i,6).value,         #鞋码
            'address':  sheet1.cell(i,7).value,      #住址
            'fullname':  sheet1.cell(i,8).value,     #全称
            'date_mianshi':  sheet1.cell(i,9).value, #面试日期
            'referee': sheet1.cell(i,10).value,      #来源
            'factory': sheet1.cell(i,11).value,     #厂部
            'sale': sheet1.cell(i,12).value,         #工资
            'hid': sheet1.cell(i,13).value,          #马甲号
            'company': sheet1.cell(i,14).value,      #公司
            'date_baodao': sheet1.cell(i,15).value,  #报道日期
            'type': sheet1.cell(i,16).value,         #性质
            'note': sheet1.cell(i,17).value          #备注
        }

        infos.append(info)

    return infos

def export_huizong(infos):
    style = xlwt.XFStyle()
    pattern = xlwt.Pattern()
    pattern.pattern = xlwt.Pattern.SOLID_PATTERN
    pattern.pattern_fore_colour = xlwt.Style.colour_map['yellow'] #设置单元格背景色为黄色
    style.pattern = pattern

    huizong = xlrd.open_workbook('template/huizong.xls', formatting_info = True)
    #print(yuyue.sheets())

    workbook = copy(huizong)

    sheet1 = workbook.get_sheet(0)

    for i in range(len(infos)):
        info = infos[i]

        #print(info['company'])
        #print(info['hid'])
        print(info['idcard'], " : ", checkBirthDate(info['idcard']))
        #fullname = info['company'] + str(info['hid']) + info['name']
        sheet1.write(i+1, 0, label = info['seq'],style = style)
        sheet1.write(i+1, 1, label = info['name'],style = style)
        sheet1.write(i+1, 2, label = info['sex'],style = style)
        sheet1.write(i+1, 3, label = info['idcard'],style = style)
        sheet1.write(i+1, 4, label = info['phone'],style = style)
        sheet1.write(i+1, 5, label = info['clothes'],style = style)
        sheet1.write(i+1, 6, label = info['shoes'],style = style)
        sheet1.write(i+1, 7, label = info['address'],style = style)
        sheet1.write(i+1, 8, label = info['fullname'],style = style)
        sheet1.write(i+1, 9, label = info['date_mianshi'],style = style)
        sheet1.write(i+1, 10, label = info['referee'],style = style)
        sheet1.write(i+1, 11, label = info['factory'],style = style)
        sheet1.write(i+1, 12, label = info['sale'] ,style = style)
        sheet1.write(i+1, 13, label = info['hid'],style = style)
        sheet1.write(i+1, 14, label = info['company'],style = style)
        sheet1.write(i+1, 15, label = info['date_baodao'],style = style)
        sheet1.write(i+1, 16, label = info['type'],style = style)
        sheet1.write(i+1, 17, label = info['note'],style = style)
    #print(i)

    excelName = "{0}汇总.xls".format(time.strftime("%m.%d"))
    workbook.save(excelName)

def export_yuyue(infos):
    yuyue = xlrd.open_workbook('template/yuyue.xls', formatting_info = True)
    #print(yuyue.sheets())

    workbook = copy(yuyue)

    sheet1 = workbook.get_sheet(0)

    for i in range(len(infos)):
        info = infos[i]

        #print(info['company'])
        #print(info['hid'])
        #print(info['name'])
        fullname = info['company'] + str(info['hid']) + info['name']
        sheet1.write(i+1, 0, label = str(i+1), )
        sheet1.write(i+1, 1, label = fullname)
        sheet1.write(i+1, 2, label = info['sex'])
        sheet1.write(i+1, 3, label = 'AUSZ')
        sheet1.write(i+1, 4, label = info['idcard'])
        sheet1.write(i+1, 5, label = info['card'])
        sheet1.write(i+1, 6, label = info['company'])
        sheet1.write(i+1, 7, label = info['phone'])
        sheet1.write(i+1, 8, label = '外包')
        sheet1.write(i+1, 9, label = 'S01FAB')
        sheet1.write(i+1, 10, label = 'F1-1F，F1-3F,F2-1F')
        sheet1.write(i+1, 11, label = time.strftime("%Y.%m.%d") )
        sheet1.write(i+1, 12, label = '2019.12.31' )
        sheet1.write(i+1, 14, label = 'S0305200' )
        sheet1.write(i+1, 15, label = '陈红' )
        sheet1.write(i+1, 16, label = '8690-1214' )
    #print(i)

    excelName = "S01厂{0}预约名单.xls".format(time.strftime("%m.%d"))
    workbook.save(excelName)


def export_ziliao(infos):
    yuyue = xlrd.open_workbook('template/ziliao.xls', formatting_info = True)
    #print(yuyue.sheets())

    workbook = copy(yuyue)

    sheet1 = workbook.get_sheet(0)
    
    for i in range(len(infos)):
        info = infos[i]

        sheet1.write(i+1, 0, label = str(i+1))
        sheet1.write(i+1, 1, label = info['name'])
        sheet1.write(i+1, 2, label = info['sex'])
        sheet1.write(i+1, 3, label = info['idcard'])
        sheet1.write(i+1, 4,  xlwt.Formula('MID(D2,7,4)&"/"&MID(D2,11,2)&"/"&MID(D2,13,2)'))
        sheet1.write(i+1, 5, label = '汉')
        sheet1.write(i+1, 6, label = info['address'])
        sheet1.write(i+1, 7, label = info['phone'])
        sheet1.write(i+1, 8, label = 'S01')
      
    #print(i)
    #S06训练组资料-培训人员
    excelName = "S01训练组资料-培训人员.xls"
    workbook.save(excelName)



def export_qiandao(infos):
    yuyue = xlrd.open_workbook('template/qiandao.xls', formatting_info = True)
    #print(yuyue.sheets())

    workbook = copy(yuyue)

    sheet1 = workbook.get_sheet(0)
    sheet1.insert_bitmap('template/qiandao.bmp',0,0,0,0,scale_x=1,scale_y=0.14)
    
    for i in range(len(infos)):
        info = infos[i]

        sheet1.write(i+2, 0, label = str(i+1))
        
        sheet1.write(i+2, 2, label = info['name'])
        
    #print(i)
    #S06训练组资料-培训人员
    excelName = "S01训练组签到表.xls"
    workbook.save(excelName)



def export_tijian(infos):
    yuyue = xlrd.open_workbook('template/tijian.xls', formatting_info = True)
    #print(yuyue.sheets())

    workbook = copy(yuyue)

    sheet1 = workbook.get_sheet(0)
    
    for row in range(1,45):
        for colunm in range(1,26):
            setOutCell(sheet1, colunm, row, '')
    # for i in range(len(infos)):
    #     info = infos[i]

    #     sheet1.write(i+1, 0, label = str(i+1))
    #     sheet1.write(i+1, 1, label = info['name'])
    #     sheet1.write(i+1, 2, label = info['sex'])
    #     sheet1.write(i+1, 3, label = info['idcard'])
    #     sheet1.write(i+1, 4,  xlwt.Formula('MID(D2,7,4)&"/"&MID(D2,11,2)&"/"&MID(D2,13,2)'))
    #     sheet1.write(i+1, 5, label = '汉')
    #     sheet1.write(i+1, 6, label = info['address'])
    #     sheet1.write(i+1, 7, label = info['phone'])
    #     sheet1.write(i+1, 8, label = 'S01')
      
    #print(i)
    #S06训练组资料-培训人员
    excelName = "tijian.xls"
    workbook.save(excelName)

#infos = read_huizong()

def _getOutCell(outSheet, colIndex, rowIndex):
    """ HACK: Extract the internal xlwt cell representation. """
    row = outSheet._Worksheet__rows.get(rowIndex)
    if not row: return None

    cell = row._Row__cells.get(colIndex)
    return cell

def setOutCell(outSheet, col, row, value):
    """ Change cell value without changing formatting. """
    # HACK to retain cell style.
    previousCell = _getOutCell(outSheet, col, row)
    # END HACK, PART I

    #outSheet.write(row, col, value)

    # HACK, PART II
    if previousCell:
        newCell = _getOutCell(outSheet, col, row)
        if newCell:
            newCell.xf_idx = previousCell.xf_idx
    # END HACK

def checkBirthDate(idCard):
    year = int(idCard[6:10])
    month = int(idCard[10:12])
    day = int(idCard[12:14])
    birthDate = date(year, month, day)
    timedelta = date.today() - birthDate
    
    return timedelta


#export_tijian(None)
infos = read_huizong()
export_huizong(infos)
#export_ziliao(infos)
#export_qiandao(infos)
# encoder = Code128Encoder("690123456789")
# encoder.save("pyStrich.png", 3)