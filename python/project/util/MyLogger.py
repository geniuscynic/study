import logging

class MyLogger:
    def __init__(self):
        self.__logger = logging.getLogger(__name__)
        handler = logging.StreamHandler()
        formatter = logging.Formatter(
                '%(asctime)s %(name)-12s %(levelname)-8s %(message)s')
        handler.setFormatter(formatter)
        self.__logger.addHandler(handler)
        self.__logger.setLevel(logging.DEBUG)

    @classmethod
    def getLogger(cls, *args, **kwargs):
        if not hasattr(MyLogger, "_instance"):
            MyLogger._instance = MyLogger(*args, **kwargs)
            print("实例化logger")
       
        return MyLogger._instance.__logger