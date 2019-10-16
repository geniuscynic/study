import logging, os


class MyLog():
    def __init__(self):
        #LOG_FORMAT = "%(message)s"

        self.logger = logging.getLogger()
        self.logger.setLevel(logging.DEBUG)
        self.logger.propagate = False

        #formatter = logging.Formatter(LOG_FORMAT)


        ch = logging.StreamHandler()
        ch.setLevel(logging.DEBUG)
        #ch.setFormatter(formatter)

        self.logger.addHandler(ch)

    def log(self, *args):
        logging.debug(args)

    @staticmethod
    def getLogger():
        return MyLog()

