import sys
from PyQt5.QtWidgets import *

class GUI(QMainWindow):
    def __init__(self):
        super().__init__()
        self.iniUI()

    def iniUI(self):
        self.setWindowTitle("service demo")
        self.resize(600, 400)
        #self.initMenuAndStatus()
        self.initlayout()
        self.initWindow()

    # 添加菜单栏和状态栏
    def initMenuAndStatus(self):
        self.statusBar().showMessage("文本状态栏")
        # 创建一个菜单栏
        menu = self.menuBar()
        # 创建两个菜单
        file_menu = menu.addMenu("文件")
        file_menu.addSeparator()
        edit_menu = menu.addMenu('修改')

        # 创建一个行为
        new_action = QAction('新的文件',self)
        # 更新状态栏文本
        new_action.setStatusTip('打开新的文件')
        # 添加一个行为到菜单
        file_menu.addAction(new_action)

        # 创建退出行为
        exit_action = QAction('退出',self)
        # 退出操作
        exit_action.setStatusTip("点击退出应用程序")
        # 点击关闭程序
        exit_action.triggered.connect(self.close)
        # 设置退出快捷键
        exit_action.setShortcut('Ctrl+Q')
        # 添加退出行为到菜单上
        file_menu.addAction(exit_action)

    # 网格布局
    def initlayout(self):

        self.labelService = QLabel('Services')
        self.comboBoxServices = QComboBox()
        self.buttonSend = QPushButton('Send')
       
        hbox_1 = QHBoxLayout()
        hbox_1.addWidget( self.labelService)
        hbox_1.addWidget(self.comboBoxServices)
        hbox_1.addWidget(self.buttonSend)
        hbox_1.addStretch(1)


        self.textEditRequest = QTextEdit()
        self.textEditRespone = QTextEdit()

        splitter = QSplitter()
        splitter.addWidget( self.textEditRequest)
        splitter.addWidget( self.textEditRespone)

        vbox = QVBoxLayout()
        vbox.addLayout(hbox_1)
        vbox.addWidget(splitter)

        
        centralwidget = QWidget()
        centralwidget.setLayout(vbox)

        self.setCentralWidget(centralwidget)

    def initWindow(self):
        self.initComboBoxServices()

    def initComboBoxServices(self):
        self.comboBoxServices.addItem("--请选择--")
        self.comboBoxServices.addItem("GetJobStatus")
        self.comboBoxServices.addItem("AutoDispatchJobs")
        self.comboBoxServices.addItem("CancelJobs")


if __name__ == '__main__':
    app = QApplication(sys.argv)
    gui = GUI()
    gui.show()
    sys.exit(app.exec_())