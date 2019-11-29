# Definition for singly-linked list.
import json

class ListNode:
    def __init__(self, x):
        self.val = x
        self.next = None

class Solution:
       
    def addTwoNumbers(self, l1: ListNode, l2: ListNode) -> ListNode:
        result = ListNode(None)
        temp = ListNode(None)

        carroy = 0
        while(l1 or l2):
            x= l1.val if l1 else 0
            y= l2.val if l2 else 0
            v = x + y + carroy
            if(v >= 10):
                carroy = 1
                v = v % 10
            else:
                carroy = 0
            
            if result.val == None:
                result.val = v
            elif temp.val == None:
                result.next = ListNode(v)
                temp = result.next
            else:
                temp.next = ListNode(v)
                temp = temp.next

            l1 = l1.next if l1 else None
            l2 = l2.next if l2 else None
            
        if(carroy > 0):
            v = carroy
            if result.val == None:
                result.val = v
            elif temp.val == None:
                result.next = ListNode(v)
                temp = result.next
            else:
                temp.next = ListNode(v)

        return result

            

        #result = l1.val + l2.val
        #return ListNode[result]

def stringToIntegerList(input):
    return json.loads(input)

def stringToListNode(input):
    # Generate list from the input
    numbers = stringToIntegerList(input)

    # Now convert that list into linked list
    dummyRoot = ListNode(0)
    ptr = dummyRoot
    for number in numbers:
        ptr.next = ListNode(number)
        ptr = ptr.next

    ptr = dummyRoot.next
    return ptr

def listNodeToString(node):
    if not node:
        return "[]"

    result = ""
    while node:
        result += str(node.val) + ", "
        node = node.next
    return "[" + result[:-2] + "]"

def main():
    import sys
    import io
    def readlines():
        for line in io.TextIOWrapper(sys.stdin.buffer, encoding='utf-8'):
            yield line.strip('\n')

    lines = readlines()
    while True:
        try:
            line = next(lines)
            l1 = stringToListNode(line);
            line = next(lines)
            l2 = stringToListNode(line);
            
            ret = Solution().addTwoNumbers(l1, l2)

            out = listNodeToString(ret);
            print(out)
        except StopIteration:
            break

if __name__ == '__main__':
    main()