import logging, os
from utils.singleton import singleton

@singleton
class MyLog():
    def __init__(self):
        #LOG_FORMAT = "%(message)s"
        #print("aa")

        self.logger = logging.getLogger(__name__)
        self.logger.setLevel(logging.DEBUG)
        self.logger.propagate = False

        #formatter = logging.Formatter(LOG_FORMAT)


        ch = logging.StreamHandler()
        ch.setLevel(logging.INFO)
        #ch.setFormatter(formatter)

        self.logger.addHandler(ch)

    def debug(self, *args):
        self.logger.debug(args)

    def info(self, *args):
        self.logger.info(args)

def getLogger():
    return MyLog()

if (__name__ =="__main__"):
    logger = logging.getLogger("test")
    logger.setLevel(logging.DEBUG)

    ch = logging.StreamHandler()
    ch.setLevel(logging.DEBUG)
    logger.addHandler(ch)

    logger.info('test info')
    logger.debug('test debug')
    logger.warning('test warning')
    logger.error('test error')
    logger.critical('test critical')
