import sys
from Ui_services import Ui_MainWindow
from PyQt5.QtWidgets import QApplication,QMainWindow,QAction

class GUI(QMainWindow):
    def __init__(self):
        super().__init__()
        self.iniUI()

    def iniUI(self):
        self.setWindowTitle("州的先生zmister.com PythonGUI教程")
        self.statusBar().showMessage("文本状态栏")
        self.resize(400, 300)

        # 创建一个菜单栏
        menu = self.menuBar()
        # 创建一个菜单
        file_menu = menu.addMenu("文件")

        # 创建一个行为
        new_action = QAction('新文件',self)
        # 添加一个行为到菜单
        file_menu.addAction(new_action)

        # 更新状态栏文本
        new_action.setStatusTip('新的文件11')

if __name__ == '__main__':
    app = QApplication(sys.argv)
    #gui = GUI()
    main = QMainWindow()

    content = Ui_MainWindow()

    content.setupUi(main)

    main.show()
    sys.exit(app.exec_())